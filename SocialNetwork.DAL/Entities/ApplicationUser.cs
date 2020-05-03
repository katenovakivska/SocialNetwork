using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public byte[] Avatar { get; set; }
    }
}
