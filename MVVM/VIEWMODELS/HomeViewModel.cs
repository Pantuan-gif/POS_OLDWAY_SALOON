using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using POS_OLDWAY_SALOON.MVVM.VIEWS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

public partial class HomeViewModel : ObservableObject
{
    // Navigation Commands for Quick Access Buttons on Home Screen

    [RelayCommand]
    private async Task GoToOrdering()
    {
        await NavigateTo(new OrderingManagement());
    }

    [RelayCommand]
    private async Task GoToCart()
    {
        await NavigateTo(new Cart());
    }

    [RelayCommand]
    private async Task GoToReports()
    {
        await NavigateTo(new Reports());
    }

    [RelayCommand]
    private async Task GoToInventory()
    {
        await NavigateTo(AppPages.Inventory);
    }

    private static async Task NavigateTo(Page page)
    {
        if (Application.Current?.MainPage is FlyoutPage flyout 
            && flyout.Detail is NavigationPage nav)
        {
            await nav.PushAsync(page);
            flyout.IsPresented = false;   // close flyout if open
        }
    }
}