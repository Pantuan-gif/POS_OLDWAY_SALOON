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
    }

    void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as FlyoutMenuItem;

        if (item != null)
        {
            var page = (Page)Activator.CreateInstance(item.TargetPage);

            // pass the ID
            if (page is Home home)
                home.thisId = currentID;

            if (page is UserManagement user)
                user.thisId = currentID;
            
            Detail = new NavigationPage(page);

            if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                IsPresented = false;
        }
    }
}