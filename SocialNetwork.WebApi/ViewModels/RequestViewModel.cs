using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.PL.ViewModels
{
    public class RequestViewModel
    {
        public int RequestId { get; set; }
        public string ReceiverName { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public virtual UserViewModel Sender { get; set; }
        public virtual UserViewModel Receiver { get; set; }
    }
}
