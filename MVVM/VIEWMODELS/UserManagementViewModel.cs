using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_OLDWAY_SALOON.MVVM.MODELS;

namespace POS_OLDWAY_SALOON.MVVM.VIEWMODELS
{
    public partial class UserManagementViewModel : ObservableObject
    {
        public int CurrentId;

        [ObservableProperty]
        private string fullName;
        [ObservableProperty]
        private string role;

        public ObservableCollection<User> User => LoginViewModels.User;

        public UserManagementViewModel() 
        {
            var user = LoginViewModels.User.FirstOrDefault(x => x.Id == CurrentId);

            if (user != null) 
            {
                fullName = user.FirstName + " " + user.LastName;
                role = user.Role;
            }
        }
    }
}
