using SocialNetwork.BLL.DTOs;
using System.Collections.Generic;

namespace SocialNetwork.BLL.Interfaces
{
    public interface IFriendshipService: ICRUDService<FriendshipDTO>
    {
        IEnumerable<FriendshipDTO> GetAllFriends(string userName);
    }
}
