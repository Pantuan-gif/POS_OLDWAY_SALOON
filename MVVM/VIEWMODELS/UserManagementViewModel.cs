using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS
{
    public partial class UserManagementViewModel : ObservableObject
    {
        public int CurrentId;

        [ObservableProperty]
        private string fullName;
        [ObservableProperty]
        private string role;
        [ObservableProperty]
        private User selectedUser;

        public ObservableCollection<User> User => LoginViewModels.User;

        public UserManagementViewModel() 
        {
            var user = LoginViewModels.User.FirstOrDefault(x => x.Id == CurrentId);

            if (user != null) 
            {
                fullName = user.FirstName + " " + user.LastName;
                role = user.Role;
            }
        }
        [RelayCommand]
        private async Task AddUser() 
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Registration("Add User"));
        }

        [RelayCommand]
        private async Task EditUser(User user)
        {
            if (user == null)
                return;

            await Application.Current.MainPage.Navigation.PushModalAsync(
                new EditUserPage(user));
        }


        [RelayCommand]
        private async Task DeleteUser(User user)
        {
            if (user == null)
                return;

            bool confirm = await Application.Current.MainPage.DisplayAlert(
                "Delete User",
                $"Are you sure you want to delete {user.FirstName}?",
                "Yes",
                "No");

            if (!confirm)
                return;

            User.Remove(user);

            // Optional: clear profile card if deleted user was selected
            FullName = string.Empty;
            Role = string.Empty;
        }
    }
}
