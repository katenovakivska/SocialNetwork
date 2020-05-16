using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.BLL.DTOs
{
    public class PublicationDTO
    {
        public int PublicationId { get; set; }
        public string PublicationText { get; set; }
        public byte[] Photo { get; set; }
        public DateTime PublicationDate { get; set; }
        public string UserName { get; set; }
        public ICollection<CommentDTO> Comments { get; set; }
        public ICollection<LikeDTO> Likes { get; set; }
    }
}
