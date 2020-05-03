using AutoMapper;
using SocialNetwork.BLL.DTOs;
using SocialNetwork.PL.ViewModels;

namespace SocialNetwork.PL.MapperProfile
{
    public class WebAPIAutoMapperProfile : Profile
    {
        public WebAPIAutoMapperProfile()
        {
            //CreateTwoWaysMap<UserDTO, UserViewModel>();
            //CreateTwoWaysMap<UserDTO, RegistrationViewModel>();
            CreateTwoWaysMap<PublicationDTO, PublicationViewModel>();
            CreateTwoWaysMap<CommentDTO, CommentViewModel>();
            CreateTwoWaysMap<LikeDTO, LikeViewModel>();
            CreateTwoWaysMap<FriendshipDTO, FriendshipViewModel>();
            CreateTwoWaysMap<MessageDTO, MessageViewModel>();
            CreateTwoWaysMap<RequestDTO, RequestViewModel>();
        }

        private void CreateTwoWaysMap<T1, T2>()
        {
            CreateMap<T1, T2>();
            CreateMap<T2, T1>();
        }
    }
}

