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
using POS_OLDWAY_SALOON.Services;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS
{
    public partial class LoginViewModels : ObservableObject
    {
        //APISERVICES api = new APISERVICES();

        //[ObservableProperty]
        //public string email;
        //[ObservableProperty]

        //public string password;
        //public Command LoginCommand { get; set; }

        //public LoginViewModels()
        //{
        //    LoginCommand = new Command(Login);
        //}

        //private async void Login(object obj)
        //{
        //    var user = await api.Login(Email, Password);

        //    if (user != null)
        //    {
        //        var navi = Application.Current.MainPage.Navigation;
        //        //await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.FirstName}!, "OK");
        //        //await navi.PushModalAsync(new MVVM.VIEWS.Dashboard(user.Id));
        //        Application.Current.MainPage = new Dashboard(user.Id);
        //    }
        //    else
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error", "Invalid Credentials", "Okay");
        //    }
        //}
        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private bool _isPasswordHidden = true;

        [RelayCommand]
        private void TogglePassword()
        {
            IsPasswordHidden = !IsPasswordHidden;
        }

        public ICommand LoginCommand { get; }
        public ICommand GoToRegisterCommand { get; }

        public static ObservableCollection<User> User = new();

        public LoginViewModels()
        {
            // Load persisted users from local JSON (if any); otherwise fall back to seeded defaults
            _ = LoadUsersAsync();
            LoginCommand = new RelayCommand(Login);
            GoToRegisterCommand = new RelayCommand(async () =>
            {
                await Application.Current.MainPage.Navigation.PushModalAsync(new Registration("Register"));
            });
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                var list = await MVVM.SERVICES.UserService.GetAllAsync();
                if (list != null && list.Count > 0)
                {
                    User.Clear();
                    foreach (var u in list)
                        User.Add(u);
                }
                else
                {
                    // seed defaults if no persisted users
                    if (User.Count == 0)
                    {
                        User.Add(new User
                        {
                            Id = 0,
                            FirstName = "john",
                            LastName = "admin",
                            Email = "admin",
                            Password = "admin",
                            Role = "Admin",
                            ImageSource = "nullprofile.png",
                            IsActive = true
                        });

                        User.Add(new User
                        {
                            Id = 1,
                            FirstName = "alex",
                            LastName = "cashier",
                            Email = "cashier",
                            Password = "cashier",
                            Role = "Cashier",
                            ImageSource = "nullprofile.png",
                            IsActive = true
                        });
                    }
                }
            }
            catch { /* ignore load errors */ }
        }
        private async void Login()
        {
            // Check registered users
            var user = User.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                // set centralized current user
                AuthService.CurrentUser = user;

                var navi = Application.Current.MainPage.Navigation;
                //await Application.Current.MainPage.DisplayAlert("Success", $"Welcome {user.FirstName}!, "OK");
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
