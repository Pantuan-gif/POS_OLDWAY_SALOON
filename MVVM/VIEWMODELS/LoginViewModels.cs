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
using POS_OLDWAY_SALOON.MVVM.VIEWS;

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
            if (User.Count == 0) 
            {
                LoginViewModels.User.Add(new User
                {
                    Id = 0,
                    FirstName = "john",
                    LastName = "admin",
                    Email = "admin",
                    Password = "admin",
                    Role = "Admin",
                    ImageSource = "nullprofile.png"
                });
            }
            LoginCommand = new RelayCommand(Login);
            GoToRegisterCommand = new RelayCommand(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Registration("Register"));
            });
        }
        private async void Login()
        {
            // Check registered users
            var user = User.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                var navi = Application.Current.MainPage.Navigation;
                //await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.FirstName}!", "OK");
                //await navi.PushModalAsync(new MVVM.VIEWS.Dashboard(user.Id));
                Application.Current.MainPage = new Dashboard(user.Id);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }

    }
}
