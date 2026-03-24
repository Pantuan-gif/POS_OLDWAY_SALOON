using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Registration : ContentPage
{
    private TaskCompletionSource<bool> _tcs = new();
    RegistrationViewModels vm = new RegistrationViewModels();
    public Task WaitForCloseAsync() => _tcs.Task;
    public Registration(string mode)
	{
		InitializeComponent();
		BindingContext = vm;
		Title = mode;
	}

	public async void Button_Clicked(object sender,EventArgs e)
	{

        Button btn = sender as Button;

        switch(btn.Text)
        {

            case "Register":

                vm.Register();
                _tcs.TrySetResult(true);
                
                await Navigation.PopModalAsync();
                break;
            case "Cancel":

                _tcs.TrySetResult(true);

                await Navigation.PopModalAsync();
                break ;
        }
    }

}