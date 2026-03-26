using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWS;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private string image;
        [ObservableProperty]
        private User selectedUser;

        public ObservableCollection<User> User => LoginViewModels.User;
        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private string selectedFilter; // "Name", "Role", "Status"

        public ObservableCollection<User> SearchResults { get; set; }

        public UserManagementViewModel()
        {
            var user = LoginViewModels.User.FirstOrDefault(x => x.Id == CurrentId);

            if (user != null)
            {
                fullName = user.FirstName + " " + user.LastName;
                image = user.ImageSource;
                role = user.Role;
            }
            SearchResults = new ObservableCollection<User>(User);

            var user1 = User.FirstOrDefault(x => x.Id == CurrentId);

            if (user1 != null)
            {
                FullName = user1.FirstName + " " + user1.LastName;
                Image = user1.ImageSource;
                Role = user1.Role;
            }
        }

        [RelayCommand]
        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                SearchResults = new ObservableCollection<User>(User);
                OnPropertyChanged(nameof(SearchResults));
                return;
            }

            var filtered = User.Where(u =>
            {
                switch (SelectedFilter)
                {
                    case "Role":
                        return u.Role?.ToLower().Contains(SearchText.ToLower()) == true;

                    case "Status":
                        return u.IsActive.ToString().ToLower().Contains(SearchText.ToLower());

                    default: // Name
                        return (u.FirstName + " " + u.LastName)
                            .ToLower()
                            .Contains(SearchText.ToLower());
                }
            });

            SearchResults = new ObservableCollection<User>(filtered);
            OnPropertyChanged(nameof(SearchResults));
        }

        partial void OnSearchTextChanged(string value)
        {
            Search();
        }
        [RelayCommand]
        private async Task AddUser()
        {
            var page = new Registration("Add user");

            await Application.Current.MainPage.Navigation.PushModalAsync(page);

            // Wait until modal closes
            await page.WaitForCloseAsync();

            // Refresh search / UI
            Search();
        }

        [RelayCommand]
        private async Task EditUser(User user)
        {
            if (user == null)
                return;

            var page = new EditUserPage(user);

            await Application.Current.MainPage.Navigation.PushModalAsync(page);

            // Wait until modal closes
            await page.WaitForCloseAsync();

            // Refresh search / UI
            Search();
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
