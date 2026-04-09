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

    [ObservableProperty]
    private string role = "Cashier";

    [ObservableProperty]
    private bool _isRoleSelectable;


    public RegistrationViewModels()
    {
        // When used as Add User from User Management, callers will set Role = "Cashier"
        // Default for regular registration is Cashier but role is not selectable via UI.
        IsRoleSelectable = false;
    }

    // Roles removed from UI; default role will be "Cashier" when called from AddUser flow.
    [ObservableProperty]
    private bool _isPasswordHidden = true;

    [ObservableProperty]
    private bool _isConfirmPasswordHidden = true;

    [RelayCommand]
    private void TogglePassword()
    {
        IsPasswordHidden = !IsPasswordHidden;
    }

    [RelayCommand]
    private void ToggleConfirmPassword()
    {
        IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
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
            Role = role ?? "Cashier",
            Password = password,
            ImageSource = "nullprofile.png"
        });

        await Application.Current.MainPage.DisplayAlert("Success", "Registered Successfully!", "OK");
    }
}
