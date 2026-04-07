using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using POS_OLDWAY_SALOON.Services;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS
{
    public partial class LoginViewModels : ObservableObject
    {
        private readonly APISERVICES _api = new();

        [ObservableProperty] public string email = string.Empty;
        [ObservableProperty] public string password = string.Empty;

        public Command LoginCommand { get; }

        public LoginViewModels()
        {
            LoginCommand = new Command(async () => await Login());
        }

        private async Task Login()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Email and Password required.", "OK");
                return;
            }

            var user = await _api.LoginAsync(Email, Password);

            if (user != null)
            {
                Application.Current!.MainPage = new Dashboard(user.Id);
            }
            else
            {
                await Application.Current!.MainPage!.DisplayAlert("Error", "Invalid Credentials", "OK");
            }
        }
    }
}