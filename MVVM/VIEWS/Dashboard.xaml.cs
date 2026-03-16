using POS_OLDWAY_SALOON.MVVM.MODELS;


namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Dashboard : FlyoutPage
{
    int currentID;
	public Dashboard(int Id)
	{
        currentID = Id;
		InitializeComponent();
        flyoutPage.collectionView.SelectionChanged += OnSelectionChanged;
    }

    void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var item = e.CurrentSelection.FirstOrDefault() as FlyoutMenuItem;
        if (item != null)
        {
            Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetPage,currentID));
            if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                IsPresented = false;
        }
    }
}