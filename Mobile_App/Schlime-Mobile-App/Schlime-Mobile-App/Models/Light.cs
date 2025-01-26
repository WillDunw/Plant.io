using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Models
{
    public class Light
    {
        private bool _isOn;
        public Light(bool isOn) 
        {
            _isOn = isOn;
        }
        public bool IsOn { get { return _isOn; } set { _isOn = value; } }

        public void SetLightState(bool value)
        {
            IsOn = value;
        }
    }
}
