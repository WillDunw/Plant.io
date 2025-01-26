using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Models
{
    public class Fan
    {
        private bool _isOn;
        public Fan(bool isOn)
        {
            _isOn = isOn;
        }
        public bool IsOn { get { return _isOn; } set { _isOn = value; } }

        public void SetFanState(bool isOn)
        {
            IsOn = isOn;
        }
    }
}
