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
    /// A class that contains information regarding the surroundings of a plant.
    /// </summary>
    public class PlantViewModel : INotifyPropertyChanged
    {
        public double Moisture { get { return GetMoistureReading(); } }
        public double Temperature { get { return GetTemperatureReading(); } }
        public double Humidity { get { return GetHumidityReading(); } }
        public double WaterLevel { get { return GetWaterLevelReading(); } }

        public bool FanIsOn { get { return App.FarmRepo.PlantRepo.GetFanState(); } }
        public bool LightIsOn { get { return App.FarmRepo.PlantRepo.GetLightState(); } }
        public string Name { get; set; }

        public PlantViewModel()
        {
            Name = App.FarmRepo.PlantRepo.GeneratePlantName();
            App.FarmRepo.PlantRepo.HumidityHistory.CollectionChanged += OnHumidityCollectionChanged;
            App.FarmRepo.PlantRepo.MoistureHistory.CollectionChanged += OnMoistureCollectionChanged;
            App.FarmRepo.PlantRepo.TemperatureHistory.CollectionChanged += OnTemperatureCollectionChanged;
            App.FarmRepo.PlantRepo.WaterLevelHistory.CollectionChanged += OnWaterLevelCollectionChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnHumidityCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        { 
            OnPropertyChanged(nameof(Humidity));
        }

        private void OnTemperatureCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Temperature));
        }

        private void OnMoistureCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Moisture));
        }

        private void OnWaterLevelCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(WaterLevel));
        }

        /// <summary>
        /// Controls the state of the plant fan.
        /// </summary>
        /// <param name="value">Boolean value representing the fan state. True for on, False for off.</param>
        public void SetFanState(bool value)
        {
            App.FarmRepo.PlantRepo.SetFanState(value);
            OnPropertyChanged(nameof(FanIsOn));
        }

        /// <summary>
        /// Controls the state of the plant light.
        /// </summary>
        /// <param name="value">Boolean value representing the light state. True for on, False for off.</param>
        public void SetLightState(bool value)
        {
            App.FarmRepo.PlantRepo.SetLightState(value);
            OnPropertyChanged(nameof(LightIsOn));
        }

        /// <summary>
        /// Retrieves the moisture values of the air surrounding the plant.
        /// </summary>
        /// <returns>A value between 0 and 1, representing the moisture percentage.</returns>
        private double GetMoistureReading()
        {
            try
            {
                return App.FarmRepo.PlantRepo.GetRecentMoisture();
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("Oops! An Error occured", $"The following error occured: {ex.Message}", "OK");
                return -1;
            }
        }

        /// <summary>
        /// Retrieves the temperature of the air surrounding the plant.
        /// </summary>
        /// <returns>The temperature in Celsius of the surrounding air.</returns>
        private double GetTemperatureReading()
        {
            try
            {
                return App.FarmRepo.PlantRepo.GetRecentTemperature();
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("Oops! An Error occured", $"The following error occured: {ex.Message}", "OK");
                return -1;
            }
        }

        /// <summary>
        /// Retrieves the humidity of the air surrounding the plant.
        /// </summary>
        /// <returns>A value between 0 and 1, representing the humidity percentage.</returns>
        private double GetHumidityReading()
        {
            try
            {
                return App.FarmRepo.PlantRepo.GetRecentHumidity();
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("Oops! An Error occured", $"The following error occured: {ex.Message}", "OK");
                return -1;
            }
        }

        /// <summary>
        /// Retrieves the relative water level of the soil near the plant.
        /// </summary>
        /// <returns>A value in ?measurement? of the water level (REWRITE THIS)</returns>
        private double GetWaterLevelReading()
        {
            try
            {
                return App.FarmRepo.PlantRepo.GetRecentWaterLevel();
            }
            catch (Exception ex)
            {

                App.Current.MainPage.DisplayAlert("Oops! An Error occured", $"The following error occured: {ex.Message}", "OK");
                return -1;
            }
        }
    }
}
