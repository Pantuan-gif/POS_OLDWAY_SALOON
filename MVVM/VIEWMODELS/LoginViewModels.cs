using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS
{
    public partial class LoginViewModels : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public static ObservableCollection<User> User = new();

        public LoginViewModels()
        {
            LoginCommand = new RelayCommand(Login);
            GoToRegisterCommand = new RelayCommand(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MVVM.VIEWS.Registration());
            });
        }

        private async void Login()
        {
            // Check hard-coded admin
            if (email == "admin" && password == "admind")
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Login Successful!", "OK");
                await Application.Current.MainPage.Navigation.PushModalAsync(new MVVM.VIEWS.Dashboard());
                return;
            }

            // Check registered users
            var user = User.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.FirstName}!", "OK");
                await Application.Current.MainPage.Navigation.PushModalAsync(new MVVM.VIEWS.Dashboard());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }

    }
}
