using Microsoft.Maui.Controls.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using Microsoft.Maui.Maps;
using Schlime_Mobile_App.ViewModels;
using System.ComponentModel;

namespace Schlime_Mobile_App.Views;

public partial class SecurityPage : ContentPage
{
    private Map main_map;
    public SecurityViewModel SecurityViewModel { get; set; }
    public GeoLocationViewModel GeoLocationViewModel { get; set; }
    public SecurityPage()
    {
        SecurityViewModel = new SecurityViewModel();
        GeoLocationViewModel = new GeoLocationViewModel(); 
        BindingContext = this;
        InitializeComponent();

        GeoLocationViewModel.PropertyChanged += ContainerLocationChanged;
        WaitForMapCoordinates();
    }

    private void Toggle_Lock_Switch(object sender, EventArgs e)
    {
        SecurityViewModel.SetLockState(!SecurityViewModel.IsDoorLocked);
    }

    private void Toggle_Buzzer_Switch(object sender, EventArgs e)
    {
        SecurityViewModel.SetBuzzerState(!SecurityViewModel.IsBuzzing);
    }

    private async Task WaitForMapCoordinates()
    {
        while (true)
        {
            if (GeoLocationViewModel.RecentLocation.Latitude != 0 && GeoLocationViewModel.RecentLocation.Longitude != 0)
            {
                Location location = new Location(GeoLocationViewModel.RecentLocation.Latitude / 100, GeoLocationViewModel.RecentLocation.Longitude / -100);
                MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
                mapSpan = mapSpan.WithZoom(5);
                main_map = new Map(mapSpan);

                main_map.HeightRequest = 500;

                Pin pin = new Pin
                {
                    Label = "Container Farm",
                    Address = "The container's current location.",
                    Type = PinType.Place,
                    Location = new Location(GeoLocationViewModel.RecentLocation.Latitude / 100, GeoLocationViewModel.RecentLocation.Longitude / -100)
                };
                main_map.Pins.Add(pin);

                loading_spinner.IsVisible = false;
                lbl_vibration.IsVisible = true;
                mainLayout.Children.Add(main_map);

                if (GeoLocationViewModel.IsVibrating)
                {
                    lbl_vibration.Text = $"Last vibration detected on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}.";
                }
                else
                {
                    lbl_vibration.Text = "No vibration detected yet.";
                }
                break;
            }

            await Task.Delay(1000);
        }
    }

    private void ContainerLocationChanged(object sender, PropertyChangedEventArgs e)
    {
        main_map.Pins[0].Location = new Location(GeoLocationViewModel.RecentLocation.Latitude / 100, GeoLocationViewModel.RecentLocation.Longitude / -100);
        if (GeoLocationViewModel.IsVibrating)
        {
            lbl_vibration.Text = $"Last vibration detected on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}.";
        }
    }
}