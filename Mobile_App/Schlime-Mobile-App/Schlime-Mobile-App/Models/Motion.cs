using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Models
{
    public class Motion
    {
        private string _value;
        private string _unit;

        public Motion(string value, string unit)
        {
            _value = value;
            _unit = unit;
        }

        public string Unit { get { return _unit; } }
        public string Value { get { return _value; } }
    }
}
