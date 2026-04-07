using POS_OLDWAY_SALOON.MVVM.MODELS;
using System.Collections.ObjectModel;


namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Dashboard : FlyoutPage
{
    public int currentID;
	public Dashboard(int userId)
	{
		InitializeComponent();
        flyoutPage.collectionView.SelectionChanged += OnSelectionChanged;
        currentID = userId;
        // Inform flyout about current user so it can filter menu items
        flyoutPage.SetCurrentUser(userId);

        // If the current user is a cashier, show the ordering page as the initial detail
        if (Services.AuthService.IsInRole("Cashier"))
        {
            Detail = new NavigationPage(AppPages.NewOrderingManagementView());
        }
    }
    async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as FlyoutMenuItem;

        if (item != null)
        {
            var page = (Page)Activator.CreateInstance(item.TargetPage);

            // pass the ID
            if (page is Home home)
            {
                home.thisId = currentID;
            }
            

            if (page is UserManagement user)
            {
                user.thisId = currentID;

            }
            // If Inventory was selected but current user is a cashier, show the Ordering page instead
            if (page is InventoryManagementView && Services.AuthService.IsInRole("Cashier"))
            {
                page = new OrderingManagement();
            }
            Detail = new NavigationPage(page);

            if (!((IFlyoutPageController)this).ShouldShowSplitMode)
            IsPresented = false;
        }
    }
}