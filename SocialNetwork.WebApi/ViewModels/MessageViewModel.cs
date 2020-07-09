using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.PL.ViewModels
{
    public class MessageViewModel
    {
        public int MessageId { get; set; }
        public string ReceiverName { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
        public string UserName { get; set; }
        public virtual UserViewModel Sender { get; set; }
    }
}
