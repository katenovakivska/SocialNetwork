using SocialNetwork.BLL.DTOs;

namespace SocialNetwork.BLL.Interfaces
{
    public interface ILikeService : ICRUDService<LikeDTO>
    {
        LikeDTO GetLike(int publicationId, string userName);
    }
}
