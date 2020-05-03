using AutoMapper;
using SocialNetwork.BLL.DTOs;
using SocialNetwork.DAL.Entities;

namespace SocialNetwork.BLL.MapperProfiles
{
    public class BLLAutoMapperProfile : Profile
    {
        public BLLAutoMapperProfile()
        {
            CreateTwoWaysMap<Publication, PublicationDTO>();
            CreateTwoWaysMap<Comment, CommentDTO>();
            CreateTwoWaysMap<Like, LikeDTO>();
            CreateTwoWaysMap<Friendship, FriendshipDTO>();
            CreateTwoWaysMap<Message, MessageDTO>();
            CreateTwoWaysMap<Request, RequestDTO>();
        }

        private void CreateTwoWaysMap<T1, T2>()
        {
            CreateMap<T1, T2>();
            CreateMap<T2, T1>();
        }
    }
}
