namespace POS_OLDWAY_SALOON
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new MVVM.VIEWS.Login()));
        }
    }
}