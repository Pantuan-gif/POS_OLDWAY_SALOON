using Microsoft.Maui.ApplicationModel.Communication;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class Registration : ContentPage
{
    private TaskCompletionSource<bool> _tcs = new();
    RegistrationViewModels vm = new RegistrationViewModels();
    public Task WaitForCloseAsync() => _tcs.Task;
    // mode: "Register" or "Add User"; initialRole sets the role when used from User Management
    public Registration(string mode, string initialRole = null)
    {
        InitializeComponent();
        if (!string.IsNullOrEmpty(initialRole))
        {
            vm.Role = initialRole;
            vm.IsRoleSelectable = false;
        }
        BindingContext = vm;
        Title = mode;
    }

	public async void Button_Clicked(object sender,EventArgs e)
	{
        Button btn = sender as Button;

        switch (btn.Text)
        {
            case "Register":
                // Common validation for registration
                if (string.IsNullOrWhiteSpace(vm.FirstName)
                    || string.IsNullOrWhiteSpace(vm.LastName)
                    || string.IsNullOrWhiteSpace(vm.Email)
                    || string.IsNullOrWhiteSpace(vm.Password)
                    || string.IsNullOrWhiteSpace(vm.ConfirmPassword))
                {
                    await DisplayAlert("Error", "All fields are required.", "OK");
                    return;
                }

                // Email format
                var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(vm.Email.Trim(), emailPattern))
                {
                    await DisplayAlert("Validation Error", "Please enter a valid email address.", "OK");
                    return;
                }

                // Duplicate email
                if (LoginViewModels.User.Any(u => string.Equals(u.Email, vm.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Validation Error", "An account with this email already exists.", "OK");
                    return;
                }

                // Password strength
                if (vm.Password.Length < 6
                    || !System.Text.RegularExpressions.Regex.IsMatch(vm.Password, "[A-Z]")
                    || !System.Text.RegularExpressions.Regex.IsMatch(vm.Password, "[a-z]")
                    || !System.Text.RegularExpressions.Regex.IsMatch(vm.Password, "[0-9]"))
                {
                    await DisplayAlert("Validation Error", "Password must be at least 6 characters and include upper-case, lower-case letters and a digit.", "OK");
                    return;
                }

                if (vm.Password != vm.ConfirmPassword)
                {
                    await DisplayAlert("Validation Error", "Passwords do not match.", "OK");
                    return;
                }

                // All validations passed — register
                vm.Register();
                _tcs.TrySetResult(true);
                await Navigation.PopModalAsync();
                break;

            case "Add User":
                // When used as Add User modal, ensure role is set to Cashier then reuse validations
                vm.Role = vm.Role ?? "Cashier";

                // run the same validation as Register
                if (string.IsNullOrWhiteSpace(vm.FirstName)
                    || string.IsNullOrWhiteSpace(vm.LastName)
                    || string.IsNullOrWhiteSpace(vm.Email)
                    || string.IsNullOrWhiteSpace(vm.Password)
                    || string.IsNullOrWhiteSpace(vm.ConfirmPassword))
                {
                    await DisplayAlert("Error", "All fields are required.", "OK");
                    return;
                }

                var emailPattern2 = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(vm.Email.Trim(), emailPattern2))
                {
                    await DisplayAlert("Validation Error", "Please enter a valid email address.", "OK");
                    return;
                }

                if (LoginViewModels.User.Any(u => string.Equals(u.Email, vm.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    await DisplayAlert("Validation Error", "An account with this email already exists.", "OK");
                    return;
                }

                if (vm.Password.Length < 6
                    || !System.Text.RegularExpressions.Regex.IsMatch(vm.Password, "[A-Z]")
                    || !System.Text.RegularExpressions.Regex.IsMatch(vm.Password, "[a-z]")
                    || !System.Text.RegularExpressions.Regex.IsMatch(vm.Password, "[0-9]"))
                {
                    await DisplayAlert("Validation Error", "Password must be at least 6 characters and include upper-case, lower-case letters and a digit.", "OK");
                    return;
                }

                if (vm.Password != vm.ConfirmPassword)
                {
                    await DisplayAlert("Validation Error", "Passwords do not match.", "OK");
                    return;
                }

                vm.Register();
                await MVVM.SERVICES.UserService.SaveAsync(LoginViewModels.User.ToList());
                _tcs.TrySetResult(true);
                await Navigation.PopModalAsync();
                break;

            case "Cancel":

                _tcs.TrySetResult(true);

                await Navigation.PopModalAsync();
                break;
        }
    }

}