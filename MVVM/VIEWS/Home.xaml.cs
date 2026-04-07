using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Home : ContentPage
{
    public int thisId { get; set; }

    public Home()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }
}