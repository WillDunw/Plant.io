using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schlime_Mobile_App.Models;
using Azure.Messaging.EventHubs.Consumer;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Newtonsoft.Json.Linq;

namespace Schlime_Mobile_App.Services
{
    public class CloudToDeviceReadingService
    {
        public async Task StartProcessing(CancellationToken ct)
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType != NetworkAccess.Internet)
            {
                return;
            }
            string connectionString = App.Settings.GetEventHubConnectionString();

            var storageClient = new BlobContainerClient(App.Settings.StorageConnectionString, App.Settings.BlobContainerName);
            var processor = new EventProcessorClient(storageClient, "$Default", connectionString, App.Settings.EventHubName);

            processor.ProcessEventAsync += ProcessEventHandler;
            processor.ProcessErrorAsync += ProcessErrorHandler;
            processor.PartitionInitializingAsync += initializeEventHandler;

            try
            {
                await processor.StartProcessingAsync(ct);

                await Task.Delay(Timeout.Infinite);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error connection to processor.");
            }

            try
            {
                await processor.StopProcessingAsync();
            }
            finally
            {
                processor.ProcessEventAsync -= ProcessEventHandler;
                processor.ProcessErrorAsync -= ProcessErrorHandler;
                processor.PartitionInitializingAsync -= initializeEventHandler;
            }
        }

        private async Task initializeEventHandler(PartitionInitializingEventArgs args)
        {
            try
            {
                if (args.CancellationToken.IsCancellationRequested)
                {
                    return;
                }

                Debug.WriteLine($"Initialize partition: {args.PartitionId}");

                // If no checkpoint was found, start processing
                // events enqueued now or in the future.

                EventPosition startPositionWhenNoCheckpoint =
                    EventPosition.FromEnqueuedTime(DateTimeOffset.UtcNow);

                args.DefaultStartingPosition = startPositionWhenNoCheckpoint;
            }
            catch
            {
                // Take action to handle the exception.
                // It is important that all exceptions are
                // handled and none are permitted to bubble up.
            }
        }

        private async Task ProcessEventHandler(ProcessEventArgs args)
        {
            try
            {
                string data = args.Data.EventBody.ToString();
                JArray parsed = JArray.Parse(data);
                foreach(JObject item in parsed.Children<JObject>())
                {
                    string unit = item.GetValue("unit").ToString();
                    string value = item.GetValue("value").ToString();
                    string reading_type = item.GetValue("reading-type").ToString();

                    if (App.FarmRepo.GeoLocationRepo.IsValidReading(reading_type))
                    {
                        //add reading to geolocation
                        App.FarmRepo.GeoLocationRepo.GenerateReadingFromString(reading_type, value, unit);
                    }
                    else if (App.FarmRepo.PlantRepo.IsValidReading(reading_type))
                    {
                        //add reading to plant repo
                        App.FarmRepo.PlantRepo.GenerateReadingFromString(reading_type, value);
                    }
                    else if (App.FarmRepo.SecurityRepo.IsValidReading(reading_type))
                    {
                        //add reading to security repo
                        App.FarmRepo.SecurityRepo.GenerateReadingFromString(reading_type, value);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async Task ProcessErrorHandler(ProcessErrorEventArgs args)
        {
            Debug.WriteLine("Error with processor");
        }
    }
}
