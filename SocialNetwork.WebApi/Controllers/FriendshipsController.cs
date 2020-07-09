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
        private ILikeService _likeService;
        private ICommentService _commentService;
        private IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;

        public FriendshipsController(UserManager<ApplicationUser> userManager,IFriendshipService friendshipService, IRequestService requestService,IPublicationService publicationService, IMapper mapper, ILikeService likeService, ICommentService commentService)
        {
            _userManager = userManager;
            _friendshipService = friendshipService;
            _publicationService = publicationService;
            _likeService = likeService;
            _commentService = commentService;
            _mapper = mapper;
            _requestService = requestService;
        }

      
        [Authorize]
        [HttpGet]
        public IActionResult GetFriends()
        {
            IEnumerable<FriendshipDTO> friendshipDtos;

            friendshipDtos = _friendshipService.GetAllFriends(User.Identity.Name);
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
                
                if (friend.Avatar != null)
                {
                    userViewModel.Avatar = ConvertPicture(friend.Avatar);
                }
                friends.Add(userViewModel);
            }
            return Ok(friends);

        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateFriendship([FromBody] FriendshipViewModel friendshipViewModel)
        {
            var friendshipDto = _mapper.Map<FriendshipDTO>(friendshipViewModel);

            friendshipDto = _friendshipService.Create(friendshipDto);

            RequestDTO requestDto = _requestService.GetFriendRequest(User.Identity.Name, friendshipViewModel.FriendName);
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
                userViewModel.Avatar = ConvertPicture(user.Avatar);
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

            foreach (var p in publicationDtos.Reverse())
            {
                var publication = _mapper.Map<PublicationViewModel>(p);
                var likesDtos = _likeService.GetAll().Where(x => x.PublicationId == p.PublicationId);
                var commentDtos = _commentService.GetAll().Where(x => x.PublicationId == p.PublicationId);
                publication.Likes = new List<LikeViewModel>();
                publication.Comments = new List<CommentViewModel>();
                if (p.Photo != null)
                {
                    publication.Photo = ConvertPicture(p.Photo);
                }
                foreach (var l in likesDtos)
                {
                    var like = _mapper.Map<LikeViewModel>(l);
                    var user = _userManager.Users.Where(x => x.UserName == l.UserName).FirstOrDefault();
                    like.Owner = new UserViewModel();
                    if (user.Avatar != null)
                    {
                        like.Owner.Avatar = ConvertPicture(user.Avatar);
                    }
                    publication.Likes.Add(like);
                }
                foreach (var c in commentDtos)
                {
                    var comment = _mapper.Map<CommentViewModel>(c);
                    var user = _userManager.Users.Where(x => x.UserName == c.UserName).FirstOrDefault();
                    comment.Owner = new UserViewModel();
                    if (user.Avatar != null)
                    {
                        comment.Owner.Avatar = ConvertPicture(user.Avatar);
                    }
                    publication.Comments.Add(comment);
                }
                publications.Add(publication);
            }

            return Ok(_mapper.Map<List<PublicationViewModel>>(publications));
        }

        private string ConvertPicture(byte[] picture)
        {
            string imageBase64Data = Convert.ToBase64String(picture);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            return imageDataURL;
        }

    }
}