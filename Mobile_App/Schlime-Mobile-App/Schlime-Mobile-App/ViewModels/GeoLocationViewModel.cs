using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Location = Schlime_Mobile_App.Models.Location;
using Schlime_Mobile_App.Models;
using Schlime_Mobile_App.Repos;
using System.ComponentModel;
using System.Collections.Specialized;

namespace Schlime_Mobile_App.ViewModels
{
    /*
     Team Name: Schlime
     Semester: Winter 2024
     Course: Application Development 3

     The GeoLocation ViewModel used to interact with the DataRepo and the views.
     */

    public class GeoLocationViewModel : INotifyPropertyChanged
    {
        public Location RecentLocation { get {
                if (App.FarmRepo.GeoLocationRepo.Locations.Count == 0)
                    return new Location(0,0,0); 
                return App.FarmRepo.GeoLocationRepo.Locations.Last(); } }
        public Orientation RecentOrientation { get {
                if (App.FarmRepo.GeoLocationRepo.Orientations.Count == 0)
                    return new Orientation(0, 0);
                return App.FarmRepo.GeoLocationRepo.Orientations.Last(); } }
        public bool IsVibrating { get { return App.FarmRepo.GeoLocationRepo.IsVibrating; } }

        public GeoLocationViewModel()
        {
            App.FarmRepo.GeoLocationRepo.Locations.CollectionChanged += OnLocationCollectionChanged;
            App.FarmRepo.GeoLocationRepo.Orientations.CollectionChanged += OnOrientationCollectionChanged;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnLocationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RecentLocation));
        }

        private void OnOrientationCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(RecentOrientation));
            OnPropertyChanged(nameof(IsVibrating));
        }
    }
}
