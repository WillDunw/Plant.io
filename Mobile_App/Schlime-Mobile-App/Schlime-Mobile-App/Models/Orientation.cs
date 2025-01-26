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
     
     A class that contains the information regarding the orientation of the farm container at a single point in time.
     */
    public class Orientation : INotifyPropertyChanged
    {
        public double Pitch { get; set; }
        public double Roll { get; set; }
        public DateTime TimeRead { get; private set; }

        public Orientation(double pitch, double roll)
        {
            Pitch = pitch;
            Roll = roll;
            TimeRead = DateTime.Now;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Calculates the difference of 2 orientations.
        /// </summary>
        /// <param name="newOrientation">The orientation to compare with the current.</param>
        /// <returns>An orientation with the values of the difference between both.</returns>
        private Orientation GetOrientationDifference(Orientation newOrientation)
        {
            return new Orientation(this.Pitch - newOrientation.Pitch, this.Roll - newOrientation.Roll);
        }

        /// <summary>
        /// Determines if the container is vibrating using the difference in orientations.
        /// </summary>
        /// <param name="newOrientation">The orientation following the previous.</param>
        /// <returns>True if the container is vibrating, false if it is not.</returns>
        public bool IsVibrating(Orientation newOrientation)
        {
            Orientation orientationDifference = GetOrientationDifference(newOrientation);

            //assuming this is in degrees
            return orientationDifference.Pitch > 0.5 || orientationDifference.Roll > 0.5 || orientationDifference.Pitch < -0.5 || orientationDifference.Roll < -0.5;
        }
    }
}
