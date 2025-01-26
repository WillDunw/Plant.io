using Schlime_Mobile_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;

namespace Schlime_Mobile_App.Services
{
    static class SendCommandToDeviceService
    {
        private static ServiceClient serviceClient = ServiceClient.CreateFromConnectionString(App.Settings.IotHubServicePrimaryString);
        private static string deviceId = "raspberry-pi";
        public static async Task SendCommand(ACommand command)
        {
            if (command.Type == ACommand.CommandType.LIGHT_ON_OFF)
            {
                var commandMessage = new Message(Encoding.ASCII.GetBytes(command.Data));
                commandMessage.Properties.Add(new KeyValuePair<string, string>("command-type", "light-on-off"));
                await serviceClient.SendAsync(deviceId, commandMessage);
            }
            else
            {
                var commandMessage = new Message(Encoding.ASCII.GetBytes(command.Data));
                commandMessage.Properties.Add(new KeyValuePair<string, string>("command-type", command.Type.ToString().ToLower()));
                await serviceClient.SendAsync(deviceId, commandMessage);
            }
        }
    }
}
