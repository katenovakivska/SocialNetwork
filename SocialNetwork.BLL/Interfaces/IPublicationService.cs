using SocialNetwork.BLL.DTOs;
using System.Collections.Generic;

namespace SocialNetwork.BLL.Interfaces
{
    public interface IPublicationService: ICRUDService<PublicationDTO>
    {
        IEnumerable<PublicationDTO> GetPage(int pageNumber, int pageElementCount);
        IEnumerable<PublicationDTO> GetAllUserPublications(string userName);
        PublicationDTO AddPhoto(int id, byte[] item);
    }
}
