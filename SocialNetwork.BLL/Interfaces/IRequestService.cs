using SocialNetwork.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.BLL.Interfaces
{
    public interface IRequestService : ICRUDService<RequestDTO>
    {
        RequestDTO GetFriendRequest(string userName, string friendName);
    }
}
