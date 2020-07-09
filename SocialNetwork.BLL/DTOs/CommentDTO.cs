using System;

namespace SocialNetwork.BLL.DTOs
{
    public class CommentDTO
    {
        public int CommentId { get; set; }
        public string CommentText { get; set; }
        public DateTime CommentDate { get; set; }
        public string UserName { get; set; }
        public int PublicationId { get; set; }
        public PublicationDTO Publication { get; set; }
    }
}
