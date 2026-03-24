using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using POS_OLDWAY_SALOON.MVVM.MODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

[QueryProperty(nameof(User), "User")]
public partial class EditUserViewModel : ObservableObject
{
    private User originalUser;

    [ObservableProperty] private string firstname;
    [ObservableProperty] private string lastname;
    [ObservableProperty] private string email;
    [ObservableProperty] private string role;
    [ObservableProperty] private string username;
    [ObservableProperty] private string password;
    [ObservableProperty] private string imageSource;
    [ObservableProperty] private bool isPasswordHidden = true;
    [ObservableProperty] private bool isActive;
    [ObservableProperty] private bool isInactive;

    public ObservableCollection<string> Roles { get; } =
        new() { "Admin", "Cashier" };

    public User User
    {
        set
        {
            originalUser = value;

            if (value == null) return;

            Firstname = value.FirstName;
            Lastname = value.LastName;
            Email = value.Email;
            Role = value.Role;
            Password = value.Password;
            ImageSource = value.ImageSource;
            IsActive = value.IsActive;
            IsInactive = !value.IsActive;
        }
    }

    [RelayCommand]
    private void TogglePassword()
    {
        IsPasswordHidden = !IsPasswordHidden;
    }

    [RelayCommand]
    private async Task ChangePhoto()
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select Profile Image",
            FileTypes = FilePickerFileType.Images
        });

        if (result != null)
        {
            ImageSource = result.FullPath;
        }
    }
    public async void Save()
    {
        if (originalUser == null)
            return;


        originalUser.FirstName = Firstname;
        originalUser.LastName = Lastname;
        originalUser.Email = Email;
        originalUser.Role = Role;
        originalUser.Password = Password;
        originalUser.ImageSource = ImageSource;
        originalUser.IsActive = IsActive;

        await Application.Current.MainPage.DisplayAlert("Success", "User Updated", "OK");
        
        
       // await Application.Current.MainPage.Navigation.PopModalAsync(); ;

    }
}