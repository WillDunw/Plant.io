using Schlime_Mobile_App.ViewModels;
using Microsoft.Maui.Maps;
using Map = Microsoft.Maui.Controls.Maps.Map;
using System.ComponentModel;
using Microsoft.Maui.Controls.Maps;

namespace Schlime_Mobile_App.Views;

public partial class GeoLocationView : ContentPage
{
    private Map main_map;
    public GeoLocationViewModel ViewModel { get; set; }
    public GeoLocationView()
    {
        ViewModel = new GeoLocationViewModel();
        InitializeComponent();

        BindingContext = this;

        ViewModel.PropertyChanged += ContainerLocationChanged;

        WaitForMapCoordinates();
    }

    private async Task WaitForMapCoordinates()
    {
        while(true)
        {
            if(ViewModel.RecentLocation.Latitude != 0 && ViewModel.RecentLocation.Longitude != 0)
            {
                Location location = new Location(ViewModel.RecentLocation.Latitude / 100, ViewModel.RecentLocation.Longitude / -100);
                MapSpan mapSpan = new MapSpan(location, 0.01, 0.01);
                mapSpan = mapSpan.WithZoom(5);
                main_map = new Map(mapSpan);

                main_map.HeightRequest = 500;

                Pin pin = new Pin
                {
                    Label = "Container Farm",
                    Address = "The container's current location.",
                    Type = PinType.Place,
                    Location = new Location(ViewModel.RecentLocation.Latitude / 100, ViewModel.RecentLocation.Longitude / -100)
                };
                main_map.Pins.Add(pin);

                loading_spinner.IsVisible = false;
                lbl_vibration.IsVisible = true;
                mainLayout.Children.Add(main_map);

                if (ViewModel.IsVibrating)
                {
                    lbl_vibration.Text = $"Last vibration detected on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}.";
                } else
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
        main_map.Pins[0].Location = new Location(ViewModel.RecentLocation.Latitude / 100, ViewModel.RecentLocation.Longitude / -100);
        if (ViewModel.IsVibrating)
        {
            lbl_vibration.Text = $"Last vibration detected on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}.";
        }
    }
}