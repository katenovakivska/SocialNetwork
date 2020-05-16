using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.PL.ViewModels
{
    public class UserCommentViewModel
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
    }
}
