using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        public string ReceiverName { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
        public string UserName { get; set; }
    }
}
