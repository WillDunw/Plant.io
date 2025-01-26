using Schlime_Mobile_App.Models;
using Schlime_Mobile_App.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Repos
{
    /*
     Team Name: Schlime
     Semester: Winter 2024
     Course: Application Development 3
     
     Mock repo for plant sensor readings and information 
     */
    public class PlantRepo
    {
        private string[] reading_types = ["temperature", "humidity", "moisture", "waterLevel"];
        private static string[] _plantNames = ["Corn", "Wheat", "Tomato", "Carrot", "Spinach"];

        private Fan fan;
        private Light light;
        public ObservableCollection<AReading> HumidityHistory { get; set; } = new ObservableCollection<AReading>();
        public ObservableCollection<AReading> TemperatureHistory { get; set; } = new ObservableCollection<AReading>();
        public ObservableCollection<AReading> MoistureHistory { get; set; } = new ObservableCollection<AReading>();
        public ObservableCollection<AReading> WaterLevelHistory { get; set; } = new ObservableCollection<AReading>();

        public PlantRepo()
        {
            fan = new Fan(true);
            light = new Light(false);

            CheckFanState();
            CheckLightState();

            //GenerateTestData();
        }

        private async void CheckFanState()
        {
            string fanState = await SecureStorage.Default.GetAsync("fan_on");

            if (fanState != null)
            {
                SetFanState(fanState == "on" ? true : false);
            }
        }

        private async void CheckLightState()
        {
            string lightState = await SecureStorage.Default.GetAsync("light_on");

            if (lightState != null)
            {
                SetLightState(lightState == "on" ? true : false);
            }
        }

        private void GenerateTestData()
        {
            HumidityHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Humidity, 30, DateTime.Now));
            HumidityHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Humidity, 40, DateTime.Now));
            HumidityHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Humidity, 62, DateTime.Now));

            TemperatureHistory.Add(new AReading(AReading.Unit.Celsius, AReading.Type.Temperature, 30, DateTime.Now));
            TemperatureHistory.Add(new AReading(AReading.Unit.Celsius, AReading.Type.Temperature, 27, DateTime.Now));
            TemperatureHistory.Add(new AReading(AReading.Unit.Celsius, AReading.Type.Temperature, 45, DateTime.Now));

            MoistureHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Moisture, 55, DateTime.Now));
            MoistureHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Moisture, 64, DateTime.Now));
            MoistureHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Moisture, 71, DateTime.Now));

            WaterLevelHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.WaterLevel, 30, DateTime.Now));
            WaterLevelHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.WaterLevel, 32, DateTime.Now));
            WaterLevelHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.WaterLevel, 31, DateTime.Now));
        }

        public void GenerateReadingFromString(string readingType, string value)
        {
            try
            {
                switch (readingType)
                {
                    case "temperature":
                        TemperatureHistory.Add(new AReading(AReading.Unit.Celsius, AReading.Type.Temperature, float.Parse(value), DateTime.Now));
                        break;
                    case "moisture":
                        MoistureHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Moisture, float.Parse(value), DateTime.Now));
                        break;
                    case "humidity":
                        HumidityHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.Humidity, float.Parse(value), DateTime.Now));
                        break;
                    case "waterLevel":
                        WaterLevelHistory.Add(new AReading(AReading.Unit.Percent, AReading.Type.WaterLevel, float.Parse(value), DateTime.Now));
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public bool IsValidReading(string reading_type)
        {
            return reading_types.Contains(reading_type);
        }

        /// <summary>
        /// Generates a random name for the plant.
        /// </summary>
        /// <returns>A random name for the plant.</returns>
        public string GeneratePlantName()
        {
            Random random = new Random();
            return _plantNames[random.Next(_plantNames.Length)];
        }

        #region Mock Methods
        /// <summary>
        /// Generates a mock humidity value.
        /// </summary>
        /// <returns>A random value between 0 and 100, representing the humidity percentage.</returns>
        public double GenerateMockHumidity()
        {
            //TODO: Implement IotHub connection and get actual reading
            Random random = new Random();

            AReading humidityReading = new AReading(AReading.Unit.Percent,
                AReading.Type.Humidity, 
                random.Next(101), 
                DateTime.Now);

            HumidityHistory.Add(humidityReading);

            return humidityReading.Value;
        }

        /// <summary>
        /// Generates a mock temperature value
        /// </summary>
        /// <returns>A random temperature value in Celsius.</returns>
        public double GenerateMockTemperature()
        {
            //TODO: Implement IotHub connection and get actual reading
            Random random = new Random();

            AReading tempReading = new AReading(AReading.Unit.Celsius,
                AReading.Type.Temperature,
                random.Next(50),
                DateTime.Now);

            TemperatureHistory.Add(tempReading);

            return tempReading.Value;
        }

        /// <summary>
        /// Generates a mock moisture value.
        /// </summary>
        /// <returns>A random moisture value between 0 and 1.</returns>
        public double GenerateMockMoisture()
        {
            //TODO: Implement IotHub connection and get actual reading
            Random random = new Random();

            AReading moistureReading = new AReading(AReading.Unit.Percent,
                AReading.Type.Moisture,
                random.Next(101),
                DateTime.Now);

            MoistureHistory.Add(moistureReading);

            return moistureReading.Value;
        }

        /// <summary>
        /// Generates a mock relative water level value.
        /// </summary>
        /// <returns>A random value in ?measurement? of the water level (REWRITE THIS)</returns>
        public double GenerateMockWaterLevel()
        {
            //TODO: Implement IotHub connection and get actual reading
            Random random = new Random();

            AReading waterLevelReading = new AReading(AReading.Unit.Percent,
                AReading.Type.WaterLevel,
                random.Next(101),
                DateTime.Now);

            WaterLevelHistory.Add(waterLevelReading);

            return waterLevelReading.Value;
        }
        #endregion

        public double GetRecentTemperature()
        {
            if (TemperatureHistory.Count == 0)
                throw new InvalidDataException("Could not retrieve the current temperature, replacing value with -1");
            else
                return TemperatureHistory.Last().Value;
        }
        public double GetRecentMoisture() 
        {
            if (MoistureHistory.Count == 0)
                throw new InvalidDataException("Could not retrieve the current moisture, replacing value with -1");
            else
                return MoistureHistory.Last().Value;
        }
        public double GetRecentHumidity()
        {
            if (HumidityHistory.Count == 0)
                throw new InvalidDataException("Could not retrieve the current humidity, replacing value with -1");
            else
                return HumidityHistory.Last().Value;
        }
        public double GetRecentWaterLevel()
        {
            if (WaterLevelHistory.Count == 0)
                throw new InvalidDataException("Could not retrieve the current water level, replacing value with -1");
            else
                return WaterLevelHistory.Last().Value;
        }

        public bool GetFanState()
        {
            return fan.IsOn;
        }
        public bool GetLightState()
        {
            return light.IsOn;
        }

        public void SetFanState(bool value)
        {
            string fanState = value ? "on" : "off";
            SendCommandToDeviceService.SendCommand(new ACommand("{\"value\":\"" + fanState + "\"}", ACommand.CommandType.FAN));

            SecureStorage.Default.SetAsync("fan_on", fanState);

            fan.SetFanState(value);
        }
        public void SetLightState(bool value)
        {
            string lightState = value ? "on" : "off";
            SendCommandToDeviceService.SendCommand(new ACommand("{\"value\":\"" + lightState + "\"}", ACommand.CommandType.LIGHT_ON_OFF));

            SecureStorage.Default.SetAsync("light_on", lightState);

            light.SetLightState(value);
        }
    }
}
