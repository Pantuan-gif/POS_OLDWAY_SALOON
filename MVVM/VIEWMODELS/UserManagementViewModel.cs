using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.Services;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class UserManagementViewModel : ObservableObject
{
    private readonly APISERVICES _api = new APISERVICES();

    public ObservableCollection<User> Users { get; } = new();

    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private User? selectedUser;

    public UserManagementViewModel()
    {
        LoadUsers();
    }

    private async void LoadUsers()
    {
        var users = await _api.GetAllUsersAsync();
        Users.Clear();
        foreach (var user in users)
            Users.Add(user);
    }

    partial void OnSearchTextChanged(string value)
    {
        // Simple client-side filter (you can improve with API query later)
        // For now, just reload and let UI binding handle
    }

    [RelayCommand]
    private async Task AddUser()
    {
        var page = new AddUserCashier();
        await Application.Current!.MainPage!.Navigation.PushModalAsync(page);
        await page.WaitForCloseAsync();
        LoadUsers(); // Refresh list
    }

    [RelayCommand]
    private async Task EditUser(User user)
    {
        if (user == null) return;
        var page = new EditUserPage(user);
        await Application.Current!.MainPage!.Navigation.PushModalAsync(page);
        await page.WaitForCloseAsync();
        LoadUsers();
    }

    [RelayCommand]
    private async Task DeleteUser(User user)
    {
        if (user == null) return;
        bool confirm = await Application.Current!.MainPage!.DisplayAlert("Delete", $"Delete {user.FullName}?", "Yes", "No");
        if (!confirm) return;

        bool success = await _api.DeleteUserAsync(user.Id);
        if (success) LoadUsers();
    }
}