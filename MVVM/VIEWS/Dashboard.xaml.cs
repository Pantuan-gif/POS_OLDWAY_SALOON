using POS_OLDWAY_SALOON.MVVM.MODELS;


namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Dashboard : FlyoutPage
{
	public Dashboard()
	{
		InitializeComponent();
        flyoutPage.collectionView.SelectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as FlyoutMenuItem;
        if (item != null)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage));
            if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                IsPresented = false;
        }
    }
}