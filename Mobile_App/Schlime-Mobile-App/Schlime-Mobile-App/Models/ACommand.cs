using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Models
{
    public class ACommand
    {
        public enum CommandType
        {
            FAN,
            LIGHT_ON_OFF,
            SERVO,
            BUZZER
        }

        public string Data { get; set; }
        public CommandType Type { get; set; }

        public ACommand(string data, CommandType type)
        {

            Data = data;
            Type = type;
        }
    }
}
