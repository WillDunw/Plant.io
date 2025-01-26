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

   A mock repo for the security subsystem
   */
    
    public class SecurityRepo
    {
        private readonly string[] reading_types = { "detection", "voltage", "motion" };
        public SoundSensor Noise;
        public ObservableCollection<AReading> noises = new ObservableCollection<AReading>();
        public ObservableCollection<AReading> motionsDetected = new ObservableCollection<AReading>();
        public AReading doorOpened;
        private bool IsBuzzing { get; set; }
        private bool IsDoorLocked{ get; set; }

        public SecurityRepo()
        {
            CheckBuzzerState();
            CheckDoorLockState();
            Noise = new SoundSensor();
        }
        /// <summary>
        /// Gets the value of the latest sound reading and converts it to a dB and returns it.
        /// </summary>
        /// <returns>A double representing the decibel level that the sound sensor captured.</returns>

        private async void CheckBuzzerState()
        {
            string buzzerState = await SecureStorage.Default.GetAsync("buzzer_on");

            if(buzzerState != null)
            {
                SetBuzzerState(buzzerState == "on" ? true : false);
            }
        }
       
        public void CheckActuatorStates()
        {
            CheckDoorLockState();
            CheckBuzzerState();
        }
       
        private async void CheckDoorLockState()
        {
            string doorLockState = await SecureStorage.Default.GetAsync("doorLock_on");

            if (doorLockState != null)
            {
                SetDoorLockState(doorLockState == "on" ? true : false);
            }
        }
        /// <summary>
        /// Gets the value of the latest sound reading and converts it to a dB and returns it.
        /// </summary>
        /// <returns>A double representing the decibel level that the sound sensor captured.</returns>

        public double GetCurrentNoise()
        {
            if (noises.Count == 0 ) {

                throw new InvalidDataException("Could not retrieve the current noise, replacing value with 0");
            }

            Noise.DigitalReading = noises[noises.Count - 1].Value;
            return Noise.DigitalTodB();
        }
        /// <summary>
        /// Gets a random state of the door
        /// </summary>
        /// <returns>True if door is open, otherwise false</returns>
        public bool GetDoorState()
        {
            if (doorOpened == null)
            {
                throw new InvalidDataException("Could not retrieve the door state, replacing value with false");

            }
            try
            {
                return doorOpened.Value == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error", ex.ToString(), "OK");
                return false;
            }
        }
        /// <summary>
        /// Gets a random state of the lock
        /// </summary>
        /// <returns>True if door is locked, otherwise false</returns>
        public bool GetDoorLockState()
        {
            return IsDoorLocked;
        }
        public void SetDoorLockState(bool state)
        {
            string doorLockState = state ? "on" : "off";
            SendCommandToDeviceService.SendCommand(new ACommand("{\"value\":\"" + doorLockState + "\"}", ACommand.CommandType.SERVO));

            SecureStorage.Default.SetAsync("doorLock_on", doorLockState);

            IsDoorLocked = state;
        }
        public void GenerateReadingFromString(string readingType, string value)
        {
            try
            {
                switch (readingType)
                {
                    case "detection": //currently there is a typo in our events lol
                        doorOpened = new AReading(AReading.Unit.Detected, AReading.Type.Detection, float.Parse(value), DateTime.Now);
                        break;
                    case "voltage":
                        noises.Add(new AReading(AReading.Unit.Volts, AReading.Type.Voltage, float.Parse(value), DateTime.Now));
                        break;
                    case "motion":
                        motionsDetected.Add(new AReading(AReading.Unit.Detected, AReading.Type.Motion, float.Parse(value), DateTime.Now));
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
        /// Generates mock motion 50% of the time
        /// </summary>
        /// <returns>True if motion is detected, otherwise false</returns>
        public bool GenerateRandomMotion()
        {
            Random rand = new Random();
            return rand.NextDouble() >= 0.5;
        }
        public bool GetCurrentMotion()
        {
            if (motionsDetected.Count == 0)
            {
                throw new InvalidDataException("Could not retrieve the current motion, replacing value with 0");
            }


            return motionsDetected[motionsDetected.Count - 1].Value == 0 ? false: true ;
        }
        public bool GetBuzzerState()
        {
            return IsBuzzing;
        }
        public void SetBuzzerState(bool b)
        {
            string buzzerState = b ? "on" : "off";
            SendCommandToDeviceService.SendCommand(new ACommand("{\"value\":\"" + buzzerState + "\"}", ACommand.CommandType.BUZZER));

            SecureStorage.Default.SetAsync("buzzer_on", buzzerState);

            IsBuzzing = b;
        }

    }
}
