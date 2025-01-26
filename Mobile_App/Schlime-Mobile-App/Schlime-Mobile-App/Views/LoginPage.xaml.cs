using Firebase.Auth;
using Schlime_Mobile_App.Repos;
using Schlime_Mobile_App.Services;

namespace Schlime_Mobile_App.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{

        InitializeComponent();
        NetworkAccess accessType = Connectivity.Current.NetworkAccess;
        if (accessType == NetworkAccess.Internet)
        {
            CloudToDeviceReadingService service = new CloudToDeviceReadingService();
            service.StartProcessing(App.CancellationToken.Token);
        }
    }

    private async void Btn_Login_Clicked(object sender, EventArgs e)
    {
        try
        {
            NetworkAccess accessType = Connectivity.Current.NetworkAccess;
            if (accessType == NetworkAccess.Internet)
            {
                AuthService.UserCreds = await AuthService.Client.SignInWithEmailAndPasswordAsync(user_name.Text, password.Text);
                App.UserTypeRepo.DetermineUserType(user_name.Text);
 
                password.Text = "";
                user_name.Text = "";
                if (App.UserTypeRepo.Type == UserTypeRepo.UserType.Technician)
                {
                    await Shell.Current.GoToAsync($"//Technician");
                }
                else
                {
                    await Shell.Current.GoToAsync($"//FleetOwner");
                }
                
            } else
            {
                await DisplayAlert("Error", "No internet", "OK");
            }
        }
        catch (FirebaseAuthException ex)
        {
            if (ex.Reason == AuthErrorReason.MissingEmail || ex.Reason == AuthErrorReason.InvalidEmailAddress) {
                lblError.Text = "Please enter a valid email address";
            }
            else if (ex.Reason == AuthErrorReason.MissingPassword)
            {
                lblError.Text = "Please enter a password";
            }
            else if (ex.Reason == AuthErrorReason.UserNotFound || ex.Reason == AuthErrorReason.AccountExistsWithDifferentCredential || ex.Reason == AuthErrorReason.Unknown)
            {
                lblError.Text = "Email or password is wrong. Please try again";
            }
            else
            {
                await DisplayAlert("Error", string.Format($"Oops! An Error occured: {ex.Reason}"), "OK");
            }


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await DisplayAlert("Error", string.Format($"Oops! An Error occured: {ex.Message}"), "OK");
        }
    }

    private async void Btn_Logout_Clicked(object sender, EventArgs e)
    {
        try
        {
            AuthService.Client.SignOut();
            password.Text = "";
            user_name.Text = "";
            LoginView.IsVisible = true;
            LogoutView.IsVisible = false;
            await Shell.Current.GoToAsync($"//LoginPage");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Oops, an error occured!", ex.Message, "OK");
        }
    }

    private void user_name_Focused(object sender, FocusEventArgs e)
    {
        username_border.BorderColor = Colors.Red;
        //reset text
        lblError.Text = "";
    }

    private void password_Focused(object sender, FocusEventArgs e)
    {
        password_border.BorderColor = Colors.Red;
        lblError.Text = "";

    }

    private void password_Unfocused(object sender, FocusEventArgs e)
    {
        password_border.BorderColor = Colors.Transparent;
    }

    private void user_name_Unfocused(object sender, FocusEventArgs e)
    {
        username_border.BorderColor = Colors.Transparent;
    }
}