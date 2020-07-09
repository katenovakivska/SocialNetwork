using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Collections.Generic;

namespace SocialNetwork.PL.ViewModels
{
    public class UserViewModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public virtual ICollection<PublicationViewModel> Publications { get; set; }
    }
}
