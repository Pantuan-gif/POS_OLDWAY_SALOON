using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
using POS_OLDWAY_SALOON.MVVM.MODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class EditUserPage : ContentPage
{
    private TaskCompletionSource<bool> _tcs = new();

    public Task WaitForCloseAsync() => _tcs.Task;
    EditUserViewModel vm = new EditUserViewModel();
    public EditUserPage(User user)
	{
		InitializeComponent();
		
		vm.User = user;
        BindingContext = vm;

    }
    private async void OnSaveCommand(object sender, EventArgs e)
    {
        // Save logic here (modify the SAME user)
        _tcs.TrySetResult(true);
        vm.Save();
        await Navigation.PopModalAsync();
    }
}