using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.BLL.DTOs
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public string ReceiverName { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
        public string UserName { get; set; }
    }
}
