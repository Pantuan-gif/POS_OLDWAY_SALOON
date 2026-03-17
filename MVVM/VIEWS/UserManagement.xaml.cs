using CommunityToolkit.Mvvm.ComponentModel;
using POS_OLDWAY_SALOON.MVVM.MODELS;
using POS_OLDWAY_SALOON.MVVM.VIEWMODELS;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class UserManagement : ContentPage
{

	public int thisId {  get; set; }
    public UserManagement()
    {
        InitializeComponent();

        var vm = new UserManagementViewModel();
        vm.CurrentId = thisId;

        BindingContext = vm;
    }


}