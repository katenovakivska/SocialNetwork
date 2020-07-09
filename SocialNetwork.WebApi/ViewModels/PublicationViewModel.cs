using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace SocialNetwork.PL.ViewModels
{
    public class PublicationViewModel
    {
        public int PublicationId { get; set; }
        public string PublicationText { get; set; }
        public string Photo { get; set; }
        public DateTime PublicationDate { get; set; }
        public string UserName { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public virtual ICollection<LikeViewModel> Likes { get; set; }
        public virtual UserViewModel Owner { get; set; }
    }
}
