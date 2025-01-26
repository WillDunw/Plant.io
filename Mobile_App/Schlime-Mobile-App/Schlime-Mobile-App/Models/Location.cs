using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Models
{
    /*
     Team Name: Schlime
     Semester: Winter 2024
     Course: Application Development 3
     
     A class that contains the information regarding the location of the farm container at a single point in time.
     */
    public class Location : INotifyPropertyChanged
    {
        public double Altitude { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime TimeRead { get; set; }

        public Location(double altitude, double latitude, double longitude)
        {
            Longitude = longitude;
            Latitude = latitude;
            Altitude = altitude;
            TimeRead = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public override string ToString()
        {
            return $"{Latitude}, {Latitude} (Altidue: {Altitude})";
        }
    }
}
