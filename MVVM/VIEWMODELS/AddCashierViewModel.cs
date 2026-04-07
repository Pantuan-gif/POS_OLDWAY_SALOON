using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class AddCashierViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    [ObservableProperty] private string employeeId = string.Empty;
    [ObservableProperty] private string fullName = string.Empty;
    [ObservableProperty] private string email = string.Empty;
    [ObservableProperty] private string username = string.Empty;
    [ObservableProperty] private string password = string.Empty;
    [ObservableProperty] private string confirmPassword = string.Empty;
    [ObservableProperty] private string photoPath = "nullprofile.png";

    [RelayCommand]
    public async Task Register()
    {
        if (string.IsNullOrWhiteSpace(FullName) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            await Shell.Current.DisplayAlert("Error", "All fields are required.", "OK");
            return;
        }

        if (Password != ConfirmPassword)
        {
            await Shell.Current.DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        var newUser = new User
        {
            FirstName = FullName.Split(' ').FirstOrDefault() ?? "",
            LastName = FullName.Split(' ').Skip(1).FirstOrDefault() ?? "",
            Email = Email,
            Username = string.IsNullOrEmpty(Username) ? Email : Username,
            Password = Password,
            Role = "Cashier",
            ImageSource = PhotoPath,
            IsActive = true
        };

        bool success = await _api.AddUserAsync(newUser);

        if (success)
        {
            await Application.Current!.MainPage!.DisplayAlert("Success", "Cashier registered successfully!", "OK");
            await Application.Current!.MainPage!.Navigation.PopModalAsync();
        }
        else
        {
            await Application.Current!.MainPage!.DisplayAlert("Error", "Failed to register cashier.", "OK");
        }
    }

    [RelayCommand]
    private void ChoosePhoto()
    {
        // TODO: Implement real file picker later
        PhotoPath = "default_photo.png";
    }
}