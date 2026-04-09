using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
using System.Collections.ObjectModel;
using System.Linq;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class FlyoutMenu : ContentPage
{
	public string passId { get; set; }

	public FlyoutMenu()
	{
		InitializeComponent();
	}

	// Called by Dashboard to set the current user id and build menu
	public void SetCurrentUser(int userId)
	{
		passId = userId.ToString();

        var menu = new ObservableCollection<FlyoutMenuItem>
		{
            new FlyoutMenuItem { Title = "Home", Icon = "home.svg", TargetPage = typeof(DashboardView) },
			new FlyoutMenuItem { Title = "Inventory", Icon = "inventory.svg", TargetPage = typeof(InventoryManagementView) },
			new FlyoutMenuItem { Title = "Orders", Icon = "order.svg", TargetPage = typeof(POS_OLDWAY_SALOON.MVVM.VIEWS.OrderingManagement) }
		};

		// Find user and add User Management only for Admin
        var user = LoginViewModels.User.FirstOrDefault(u => u.Id == userId);
		if (user != null)
		{
			// Admin gets User Management
			if (string.Equals(user.Role, "Admin", StringComparison.OrdinalIgnoreCase))
			{
				menu.Add(new FlyoutMenuItem { Title = "User Management", Icon = "usermngt.svg", TargetPage = typeof(UserManagement) });
			}

            // If the user is a Cashier, prefer Orders over Inventory in the menu
			if (string.Equals(user.Role, "Cashier", StringComparison.OrdinalIgnoreCase))
			{
                // remove the Inventory item so Dashboard "Inventory" button maps to Orders visually
				var inv = menu.FirstOrDefault(m => m.TargetPage == typeof(InventoryManagementView));
				if (inv != null) menu.Remove(inv);

				// Remove the Home item so cashiers don't see the dashboard
				var home = menu.FirstOrDefault(m => m.Title == "Home");
				if (home != null) menu.Remove(home);
			}
		}

		collectionView.ItemsSource = menu;

		// Update profile header
		var active = Services.AuthService.CurrentUser ?? LoginViewModels.User.FirstOrDefault(u => u.Id == userId);
		if (active != null)
		{
			ProfileName.Text = $"{active.FirstName} {active.LastName}";
			ProfileRole.Text = active.Role;
			if (!string.IsNullOrWhiteSpace(active.ImageSource))
				ProfileImage.Source = active.ImageSource;
		}
	}

	private async void OnLogoutClicked(object sender, EventArgs e)
	{
		bool ok = await Application.Current.MainPage.DisplayAlert("Logout", "Are you sure you want to log out?", "Yes", "No");
		if (!ok) return;

		// Clear centralized auth
		Services.AuthService.CurrentUser = null;

		// Navigate back to Login as root
		Application.Current.MainPage = new NavigationPage(new MVVM.VIEWS.Login());
	}

	private void OnBackClicked(object sender, EventArgs e)
	{
		if (Application.Current?.MainPage is FlyoutPage flyout)
			flyout.IsPresented = false;
	}
}
