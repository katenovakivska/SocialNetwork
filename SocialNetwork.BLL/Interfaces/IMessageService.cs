using SocialNetwork.BLL.DTOs;
using System.Collections.Generic;

namespace SocialNetwork.BLL.Interfaces
{
    public interface IMessageService : ICRUDService<MessageDTO>
    {
        IEnumerable<MessageDTO> GetChatMessages(string userName, string friendName);
    }
}
