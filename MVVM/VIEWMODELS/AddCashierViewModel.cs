using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS
{
    public partial class AddCashierViewModel : ObservableObject
    {
        [ObservableProperty] private string employeeId;
        [ObservableProperty] private string fullName;
        [ObservableProperty] private string email;
        [ObservableProperty] private string username;
        [ObservableProperty] private string password;
        [ObservableProperty] private string confirmPassword;
        [ObservableProperty] private string photoPath;

        public AddCashierViewModel()
        {
            // Default role is always Cashier
        }

        [RelayCommand]
        public async Task Register()
        {
            // Validation
            if (string.IsNullOrEmpty(EmployeeId) ||
                string.IsNullOrEmpty(FullName) ||
                string.IsNullOrEmpty(Email) ||
                string.IsNullOrEmpty(Username) ||
                string.IsNullOrEmpty(Password) ||
                string.IsNullOrEmpty(ConfirmPassword))
            {
                await Shell.Current.DisplayAlert("Error", "All fields are required.", "OK");
                return;
            }

            if (Password != ConfirmPassword)
            {
                await Shell.Current.DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Create new cashier user
            var newUser = new User
            {
                Id = LoginViewModels.User.Count + 1,
                FirstName = FullName.Split(' ')[0],
                LastName = FullName.Contains(" ") ? FullName.Split(' ')[1] : "",
                Email = Email,
                Role = "Cashier", // fixed role
                Username = Username,
                Password = Password,
                ImageSource = string.IsNullOrEmpty(PhotoPath) ? "nullprofile.png" : PhotoPath,
                IsActive = true
            };

            LoginViewModels.User.Add(newUser);

            await Application.Current.MainPage.DisplayAlert("Success", "Cashier registered successfully!", "OK");

            // Close modal after success
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        [RelayCommand]
        private void ChoosePhoto()
        {
            // Implement file picker logic here
            PhotoPath = "default_photo.png";
        }
    }
}
