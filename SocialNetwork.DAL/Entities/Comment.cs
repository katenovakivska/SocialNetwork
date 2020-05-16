using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserName { get; set; }
        public int PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
