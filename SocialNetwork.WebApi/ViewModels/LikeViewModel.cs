using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.PL.ViewModels
{
    public class LikeViewModel
    {
        public int LikeId { get; set; }
        public DateTime LikeDate { get; set; }
        public string UserName { get; set; }
        public int PublicationId { get; set; }
        public PublicationViewModel Publication { get; set; }
    }
}
