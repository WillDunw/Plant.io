using Schlime_Mobile_App.Repos;
using Schlime_Mobile_App.ViewModels;
using System.ComponentModel;

namespace Schlime_Mobile_App.Views;

public partial class PlantPage : ContentPage
{
	public PlantViewModel ViewModel { get; set; }
	public PlantPage()
	{
		ViewModel = new PlantViewModel();

		InitializeComponent();
        BindingContext = this;
        Chart1.BindingContext = ChartsRepo.GetTemperatureChart(App.FarmRepo.PlantRepo.TemperatureHistory);

        ViewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void Toggle_Fan_Btn(object sender, EventArgs e)
    {
		ViewModel.SetFanState(!ViewModel.FanIsOn);
    }

    private void Toggle_Light_Btn(object sender, EventArgs e)
    {

		ViewModel.SetLightState(!ViewModel.LightIsOn);
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateChart();
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        UpdateChart();
    }

    private void UpdateChart()
    {
        switch (Chart_Picker.SelectedItem)
        {
            case "Temperature":
                Chart1.BindingContext = ChartsRepo.GetTemperatureChart(App.FarmRepo.PlantRepo.TemperatureHistory);
                break;
            case "Moisture":
                Chart1.BindingContext = ChartsRepo.GetMoistureChart(App.FarmRepo.PlantRepo.MoistureHistory);
                break;
            case "Humidity":
                Chart1.BindingContext = ChartsRepo.GetHumidityChart(App.FarmRepo.PlantRepo.HumidityHistory);
                break;
            case "Water Level":
                Chart1.BindingContext = ChartsRepo.GetWaterLevelChart(App.FarmRepo.PlantRepo.WaterLevelHistory);
                break;
        }
    }
}