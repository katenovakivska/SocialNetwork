using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Entities
{
    public class Publication
    {
        public int PublicationId { get; set; }
        public string PublicationText { get; set; }
        public byte[] Photo { get; set; }
        public DateTime PublicationDate { get; set; }
        public string UserName { get; set; }
      
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        
    }
}
