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
    public class LikeController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private ILikeService _likeService;
        private IPublicationService _publicationService;
        private IMapper _mapper;

        public LikeController(UserManager<ApplicationUser> userManager,IMapper maper, ILikeService likeService, IPublicationService publicationService)
        {
            _userManager = userManager;
            _mapper = maper;
            _likeService = likeService;
            _publicationService = publicationService;
        }



        [Authorize]
        [HttpPost]
        public IActionResult CreateLike([FromBody] LikeViewModel likeViewModel)
        {
            var likeDto = _mapper.Map<LikeDTO>(likeViewModel);
            var like = _likeService.GetAll()
                .Where(x => x.PublicationId == likeViewModel.PublicationId && x.UserName == likeViewModel.UserName)
                .FirstOrDefault();
            if (like != null)
            {
                if (!IsUserOwner(like.LikeId, User.Identity.Name))
                {
                    return Forbid();
                }
                _likeService.Delete(like.LikeId);
            }
            else 
            {
                likeDto.LikeDate = DateTime.Now;
                likeDto.PublicationId = likeViewModel.PublicationId;
                likeDto.UserName = User.Identity.Name;

                likeDto = _likeService.Create(likeDto);
                if (likeDto == null)
                {
                    return BadRequest();
                }
            }

            return Ok(_mapper.Map<LikeViewModel>(likeDto));
        }
        [Authorize]
        [HttpGet("exists/{id}")]
        public IActionResult LikeExists(int id)
        {
            var likeDto = _likeService.Get(id);

            if (likeDto == null)
                return Ok(false);
            else
                return Ok(true);
        }
        private bool IsUserOwner(int likeId, string userName)
        {
            var likeDto = _likeService.Get(likeId);

            if (likeDto == null)
            {
                return false;
            }

            return likeDto.UserName == userName;
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetPublicationLikes(int id)
        {
            var likesDtos = _likeService.GetAll().Where(x => x.PublicationId == id);

            List<UserViewModel> likers = new List<UserViewModel>();

            if (likesDtos == null)
            {
                return BadRequest();
            }

            foreach(var like in likesDtos)
            {
                var user = _userManager.Users.Where(x => x.UserName == like.UserName).FirstOrDefault();
                UserViewModel userViewModel = new UserViewModel();
                userViewModel.UserName = user.UserName;
                if (user.Avatar != null)
                {
                    string imageBase64Data = Convert.ToBase64String(user.Avatar);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    userViewModel.Avatar = imageDataURL;
                }
                likers.Add(userViewModel);
            }

            return Ok(likers);
        }
    }
}