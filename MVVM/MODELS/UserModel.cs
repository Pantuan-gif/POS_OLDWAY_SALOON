using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.MVVM.MODELS
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }

        public int PhoneNumber { get; set; }
        public string Role { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string ProfileImage { get; set; }


    }
}
