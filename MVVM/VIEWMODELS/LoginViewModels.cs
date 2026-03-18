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
            if (User.Count == 0) 
            {
                LoginViewModels.User.Add(new User
                {
                    Id = 0,
                    FirstName = "john",
                    LastName = "admin",
                    Email = "admin",
                    Password = "admin",
                    Role = "admin",
                    ImageSource = "default.png"
                });
            }
            LoginCommand = new RelayCommand(Login);
            GoToRegisterCommand = new RelayCommand(async () =>
            {
                await Application.Current.MainPage.Navigation.PushAsync(new MVVM.VIEWS.Registration());
            });
        }
        private async void Login()
        {
            // Check registered users
            var user = User.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.FirstName}!", "OK");
                await Application.Current.MainPage.Navigation.PushModalAsync(new MVVM.VIEWS.Dashboard(user.Id));
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }

    }
}
