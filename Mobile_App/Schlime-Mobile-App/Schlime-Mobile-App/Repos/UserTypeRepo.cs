using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schlime_Mobile_App.Repos
{
    public class UserTypeRepo : INotifyPropertyChanged
    {
        public enum UserType
        {
            Technician,
            FleetOwner
        }

        public UserType? Type { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void DetermineUserType(string userEmail)
        {
            int index = userEmail.IndexOf("@");
            string username = "";

            if(index >= 0)
                username = userEmail.Substring(0, index);

            if(username == "technician")
                Type = UserType.Technician;
            else 
                Type = UserType.FleetOwner;

            OnPropertyChanged(nameof(Type));
        }

        public void ResetUserType()
        {
            Type = null;
        }
    }
}
