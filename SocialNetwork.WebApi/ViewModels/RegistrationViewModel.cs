using System;
using System.Collections.Generic;

namespace SocialNetwork.PL.ViewModels
{
    public class RegistrationViewModel
    {
        public int UserId { get; set; }

        public string Email { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Town { get; set; }
        public string Password { get; set; }

    }
}
