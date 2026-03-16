using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace POS_OLDWAY_SALOON.MVVM.VIEWS;

public partial class UserManagement : ContentPage
{

	public UserManagement(int id)
	{
		localID = Convert.ToInt16(id);
		InitializeComponent();
	}


}