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

   A class that contains the information regarding the sound sensor
   */
    public class SoundSensor : INotifyPropertyChanged
    {
        private double _sensitivity; //sensitivity of the sensor
        private double _refVoltage; //voltage ADC operates at
        private int _resolution; //resolution of ADC
        private double _digitalReading; //the last reading provided by the sensor
        public double Sensitivity
        {
            get { return _sensitivity; }
            set {
                if (value == 0)
                {
                    throw new ArgumentException("Sensitivity cannot be 0");
                }
                _sensitivity = value; }
        }
        public double RefVoltage
        {
            get { return _refVoltage; }
            set { _refVoltage = value; }
        }
        public int Resolution
        {
            get { return _resolution; }
            set { 
                if (value < 2)
                {
                    throw new ArgumentException("Resolution cannot be less than 2");
                }
                _resolution = value; }
        }
        public double DigitalReading
        {
            get { return _digitalReading; }
            set { _digitalReading = value; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SoundSensor(double sensitivity = -58.0, double refVoltage = 3.3, int resolution = 4096/*how many bits*/) //defaulting to -58 dBV/Pa since that is the average
        {
            this.Sensitivity = sensitivity;
            this.RefVoltage = refVoltage;
            this.Resolution = resolution;
        }
        public double DigitalTodB()
        {
            double voltage = DigitalReading * (RefVoltage / (Resolution-1));
            Console.WriteLine(voltage);

            double spl = voltage / Math.Pow(10, Sensitivity / 20);

            double dB = 20 * Math.Log10(spl / 20e-6); //Used chatgpt to get this formula

            return dB;
        }
    }
}
