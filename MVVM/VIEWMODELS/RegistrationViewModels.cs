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
    public partial class RegistrationViewModels : ObservableObject
    {
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        public ICommand RegisterCommand { get; }

        public RegistrationViewModels()
        {
            RegisterCommand = new RelayCommand(Register);
        }
        public static ObservableCollection<User> User = new();


        private async void Register()
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "All fields required", "OK");
                return;
            }

            LoginViewModels.User.Add(new User
            {
                Email = email,
                Password = password
            });

            await Application.Current.MainPage.DisplayAlert("Success", "Registered Successfully!", "OK");

            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
