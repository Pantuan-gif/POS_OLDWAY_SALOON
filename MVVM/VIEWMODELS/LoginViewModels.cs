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
    public class LoginViewModels : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public static ObservableCollection<Posmodel> posmodels = new();

        public LoginViewModels()
        {
            LoginCommand = new RelayCommand(Login);
            GoToRegisterCommand = new RelayCommand(async () =>
            {
                await Shell.Current.GoToAsync("//RegisterPage");
            });
        }

        private async void Login()
        {
            var user = posmodels.FirstOrDefault(u =>
                u.Email == email && u.Password == password);

            if (user != null)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Login Successful!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Invalid credentials", "OK");
            }
        }
    }
}
