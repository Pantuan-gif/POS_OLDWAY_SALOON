using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

public partial class RegistrationViewModels : ObservableObject
{
    [ObservableProperty]
    private string firstName;

    [ObservableProperty]
    private string lastName;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string password;

    [ObservableProperty]
    private string confirmPassword;


    public RegistrationViewModels()
    {
       
    }

    public async void Register()
    {
        if (string.IsNullOrEmpty(firstName) ||
            string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(confirmPassword))
        {
            await Shell.Current.DisplayAlert("Error", "All fields required", "OK");
            return;
        }

        if (password != confirmPassword)
        {
            await Shell.Current.DisplayAlert("Error", "Passwords do not match", "OK");
            return;
        }

        LoginViewModels.User.Add(new User
        {
            Id = LoginViewModels.User.Count,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Role = "Cashier",
            Password = password,
            ImageSource = "nullprofile.png"
        });

        await Application.Current.MainPage.DisplayAlert("Success", "Registered Successfully!", "OK");
    }
}
