using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.MVVM.MODELS
{
    public class User
    {

        public int Id { get; set; }

        public string Role { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}"; // computed property
        public string? ImageSource { get; set; }

        public bool IsActive { get; set; }


    }
}
