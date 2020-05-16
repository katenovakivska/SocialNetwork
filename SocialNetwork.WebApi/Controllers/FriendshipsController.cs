using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BLL.DTOs;
using SocialNetwork.BLL.Interfaces;
using SocialNetwork.DAL.Entities;
using SocialNetwork.PL.ViewModels;

namespace SocialNetwork.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private IFriendshipService _friendshipService;
        private IRequestService _requestService;
        private IPublicationService _publicationService;
        private IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;

        public FriendshipsController(UserManager<ApplicationUser> userManager,IFriendshipService friendshipService, IRequestService requestService,IPublicationService publicationService, IMapper mapper)
        {
            _userManager = userManager;
            _friendshipService = friendshipService;
            _publicationService = publicationService;
            _mapper = mapper;
            _requestService = requestService;
        }

      
        [Authorize]
        [HttpGet]
        public IActionResult GetFriends()
        {
            IEnumerable<FriendshipDTO> friendshipDtos;

            friendshipDtos = _friendshipService.GetAll().Where(x => x.FriendName == User.Identity.Name || x.UserName == User.Identity.Name);
            List<UserViewModel> friends = new List<UserViewModel>();
            foreach (var f in friendshipDtos)
            {
                ApplicationUser friend;
                if (f.FriendName == User.Identity.Name)
                {
                    friend = _userManager.Users.Where(x => x.UserName == f.UserName).FirstOrDefault();
                }
                else 
                {
                    friend = _userManager.Users.Where(x => x.UserName == f.FriendName).FirstOrDefault();
                }
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.Email = friend.Email;
                userViewModel.UserName = friend.UserName;
                userViewModel.PhoneNumber = friend.PhoneNumber;
                userViewModel.Password = friend.PasswordHash;
                if (friend.Avatar != null)
                {
                    string imageBase64Data = Convert.ToBase64String(friend.Avatar);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    userViewModel.Avatar = imageDataURL;
                }
                friends.Add(userViewModel);
            }
            return Ok(_mapper.Map<IEnumerable<UserViewModel>>(friends));

        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateFriendship([FromBody] FriendshipViewModel friendshipViewModel)
        {
            var friendshipDto = _mapper.Map<FriendshipDTO>(friendshipViewModel);
            friendshipDto.UserName = User.Identity.Name;
            friendshipDto.FriendName = friendshipViewModel.FriendName;

            friendshipDto = _friendshipService.Create(friendshipDto);

            RequestDTO requestDto = _requestService.GetAll().Where(x => x.ReceiverName == User.Identity.Name && x.UserName == friendshipViewModel.FriendName).FirstOrDefault();
            requestDto.Status = "confirmed";

            requestDto = _requestService.Update(requestDto.RequestId, requestDto);


            if (friendshipDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<FriendshipViewModel>(friendshipDto));
        }


        [Authorize]
        [HttpGet("{userName}")]
        public IActionResult GetFriendInfoById(string userName)
        {
            var user = _userManager.Users.Where(x => x.UserName == userName).FirstOrDefault();
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.Email = user.Email;
            userViewModel.UserName = user.UserName;
            userViewModel.PhoneNumber = user.PhoneNumber;

            if (user.Avatar != null)
            {
                string imageBase64Data = Convert.ToBase64String(user.Avatar);
                string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                userViewModel.Avatar = imageDataURL;
            }

            return Ok(userViewModel);
        }

        [Authorize]
        [HttpGet("{userName}/publications")]
        public IActionResult GetFriendPublications(string userName)
        {
            IEnumerable<PublicationDTO> publicationDtos;

            publicationDtos = _publicationService.GetAllUserPublications(userName);

            List<PublicationViewModel> publications = new List<PublicationViewModel>();

            foreach (var p in publicationDtos)
            {
                if (p.Photo != null)
                {
                    string imageBase64Data = Convert.ToBase64String(p.Photo);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    PublicationViewModel publication = new PublicationViewModel();
                    publication.PublicationId = p.PublicationId;
                    publication.PublicationDate = p.PublicationDate;
                    publication.PublicationText = p.PublicationText;
                    publication.UserName = p.UserName;
                    publication.Photo = imageDataURL;
                    publication.Likes = _mapper.Map<ICollection<LikeViewModel>>(p.Likes);
                    publication.Comments = _mapper.Map<ICollection<CommentViewModel>>(p.Comments);

                    publications.Add(publication);
                }

            }

            return Ok(_mapper.Map<List<PublicationViewModel>>(publications));
        }
    }
}