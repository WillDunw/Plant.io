using Schlime_Mobile_App.Repos;

namespace Schlime_Mobile_App.Views;

public partial class AccountPage : ContentPage
{
    public UserTypeRepo UserType { get; set; }
    public AccountPage()
	{
		InitializeComponent();

        UserType = App.UserTypeRepo;

        BindingContext = this;
    }

    private async void btn_logout_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("//LoginView");
    }
}