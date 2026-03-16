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
        [ObservableProperty]
        private int id;
        
        private string name;
        private string role;

        public static ObservableCollection<User> User = new();

        public ObservableCollection<User> currentUser { get; set; }

        public UserManagementViewModel() 
        {
            var user = User.FirstOrDefault(x => x.Id == id);

            if (user == null) 
            {
                name = user.FirstName + " " + user.LastName;
                role = user.Role;
            }
        }
    }
}
