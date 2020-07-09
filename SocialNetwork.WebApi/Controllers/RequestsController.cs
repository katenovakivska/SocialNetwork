using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class RequestsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private IRequestService _requestService;
        private IFriendshipService _friendshipService;
        private IMapper _mapper;

        public RequestsController(UserManager<ApplicationUser> userManager,IFriendshipService friendshipService, IRequestService requestService, IMapper mapper)
        {
            _requestService = requestService;
            _friendshipService = friendshipService;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult CreateRequest([FromBody] RequestViewModel requestViewModel)
        {
            var requestDto = _mapper.Map<RequestDTO>(requestViewModel);

            var request = _requestService.GetAll()
                .Where(x => (x.UserName == requestViewModel.UserName && x.ReceiverName == requestViewModel.ReceiverName)
                || (x.UserName == requestViewModel.ReceiverName && x.ReceiverName == requestViewModel.UserName)).FirstOrDefault();

            if (request == null)
            {
                requestDto = _requestService.Create(requestDto);
            }
            
            if (requestDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<RequestViewModel>(requestDto));
        }

        [Authorize]
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.Users.Where(x => x.UserName != User.Identity.Name);

            var friends = _friendshipService.GetAll()
                .Where(x => x.UserName == User.Identity.Name || x.FriendName == User.Identity.Name);

            var requests = _requestService.GetAll()
                .Where(x => x.UserName == User.Identity.Name || x.ReceiverName == User.Identity.Name);

            foreach (var friend in friends)
            {
                users = users.Where(x => x.UserName != friend.UserName && x.UserName != friend.FriendName);
            }

            foreach (var request in requests)
            {
                users = users.Where(x => x.UserName != request.UserName && x.UserName != request.ReceiverName);
            }

            List<UserViewModel> usersViewModel = new List<UserViewModel>();

            foreach (var u in users)
            {
                UserViewModel user = new UserViewModel();
                user.UserName = u.UserName;
                user.Email = u.Email;
                user.PhoneNumber = u.PhoneNumber;
                if (u.Avatar != null)
                {
                    user.Avatar = ConvertPicture(u.Avatar);
                }
                usersViewModel.Add(user);
            }
            return Ok(usersViewModel);
        }

        private string ConvertPicture(byte[] picture)
        {
            string imageBase64Data = Convert.ToBase64String(picture);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            return imageDataURL;
        }

        [Authorize]
        [HttpGet("send")]
        public IActionResult GetSendRequests()
        {
            IEnumerable<RequestDTO> requestDtos;
            List<RequestViewModel> requestViewModels = new List<RequestViewModel>();
            requestDtos = _requestService.GetAll().Where(x => x.UserName == User.Identity.Name && x.Status == "not confirmed");

            foreach (var request in requestDtos)
            {
                var requestViewModel = _mapper.Map<RequestViewModel>(request);
                var user = _userManager.Users.Where(x => x.UserName == request.ReceiverName).FirstOrDefault();
                requestViewModel.Receiver = new UserViewModel();

                if (user.Avatar != null)
                {
                    requestViewModel.Receiver.Avatar = ConvertPicture(user.Avatar);
                }

                requestViewModels.Add(requestViewModel);
            }

            return Ok(requestViewModels);
        }

        [Authorize]
        [HttpGet("received")]
        public IActionResult GetReceivedRequests()
        {
            IEnumerable<RequestDTO> requestDtos;
            List<RequestViewModel> requestViewModels = new List<RequestViewModel>();
            requestDtos = _requestService.GetAll().Where(x => x.ReceiverName == User.Identity.Name && x.Status == "not confirmed");

            foreach (var request in requestDtos)
            {
                var requestViewModel = _mapper.Map<RequestViewModel>(request);
                var user = _userManager.Users.Where(x => x.UserName == request.UserName).FirstOrDefault();
                requestViewModel.Sender = new UserViewModel();

                if (user.Avatar != null)
                {
                    requestViewModel.Sender.Avatar = ConvertPicture(user.Avatar);
                }

                requestViewModels.Add(requestViewModel);
            }
            
            return Ok(requestViewModels);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteRequestById(int id)
        {

            var requestDto = _requestService.Delete(id);

            if (requestDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<RequestViewModel>(requestDto));
        }
    }
}