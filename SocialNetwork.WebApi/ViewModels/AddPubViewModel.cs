using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.PL.ViewModels
{
    public class AddPubViewModel
    {
        public IFormFile file { get; set; }
        public PublicationViewModel publicationViewModel { get; set; }
    }
}
