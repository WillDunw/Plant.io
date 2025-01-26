using Schlime_Mobile_App.Models;
using Schlime_Mobile_App.Repos;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.ViewModels
{
    /// <summary>
    /// Team Name: Schlime
    /// Semester: Winter 2024
    /// Course: Application Development 3
    /// 
    /// A class that contains is used to interact between the security datarepo and the views.
    /// </summary>
    public class SecurityViewModel : INotifyPropertyChanged
    {
        private SecurityRepo _securityRepo;
        public double SoundLevel { get { return GetNoiseReading(); } }
        public bool IsMotionDetected { get { return GetMotionReading(); } }
        public bool IsDoorLocked { get { return App.FarmRepo.SecurityRepo.GetDoorLockState(); }}
        public bool IsDoorOpen { get { return GetDoorReading(); } }
        public bool IsBuzzing { get { return App.FarmRepo.SecurityRepo.GetBuzzerState(); }}

        
        public SecurityViewModel()
        {
            _securityRepo = App.FarmRepo.SecurityRepo;
            App.FarmRepo.SecurityRepo.noises.CollectionChanged += OnNoiseCollectionChanged;
            App.FarmRepo.SecurityRepo.motionsDetected.CollectionChanged += OnMotionCollectionChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnNoiseCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SoundLevel));
        }

        private void OnMotionCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IsMotionDetected));
        }
        /// <summary>
        /// Generates a set of random values for the sound, lock, door and motion.
        /// </summary>
        public double GetNoiseReading()
        {
            try
            {
                return App.FarmRepo.SecurityRepo.GetCurrentNoise();
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("Oops! An Error occured", $"The following error occured: {ex.Message}", "OK");
                return 0;
            }
        }
        public bool GetMotionReading()
        {
            try
            {
                return App.FarmRepo.SecurityRepo.GetCurrentMotion();
            }
            catch (Exception ex) {

                App.Current.MainPage.DisplayAlert("Oops! An Error occured", $"The following error occured: {ex.Message}", "OK");
                return false;
            }

        }

        public bool GetDoorReading()
        {
            try
            {
                return App.FarmRepo.SecurityRepo.GetDoorState();
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("Oops! An Error occured", $"The following error occured: {ex.Message}", "OK");
                return false;
            }
        }
        public void SetBuzzerState(bool state)
        {
            App.FarmRepo.SecurityRepo.SetBuzzerState(state);
            OnPropertyChanged(nameof(IsBuzzing));
        }

        public void SetLockState(bool state)
        {
            App.FarmRepo.SecurityRepo.SetDoorLockState(state);
            OnPropertyChanged(nameof(IsDoorLocked));
        }
    }
}
