using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Entities
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public string FriendName { get; set; }
        public string UserName { get; set; }
    }
}
