using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class AddUserCashier : ContentPage
{
    private TaskCompletionSource<bool> _tcs = new();
    public Task WaitForCloseAsync() => _tcs.Task;
    public AddUserCashier()
	{
		InitializeComponent();
		BindingContext = new AddCashierViewModel();
	}
}