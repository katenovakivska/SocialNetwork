using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Entities
{
    public class Request
    {
        public int RequestId { get; set; }
        public string ReceiverName { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
    }
}
