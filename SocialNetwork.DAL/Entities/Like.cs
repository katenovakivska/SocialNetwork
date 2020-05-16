using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Entities
{
    public class Like
    {
        public int LikeId { get; set; }
        public DateTime LikeDate { get; set; }
        public string UserName { get; set; }
        public int PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
