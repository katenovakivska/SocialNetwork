using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.BLL.DTOs
{
    public class LikeDTO
    {
        public int LikeId { get; set; }
        public DateTime LikeDate { get; set; }
        public string UserName { get; set; }
        public PublicationDTO Publication { get; set; }
    }
}
