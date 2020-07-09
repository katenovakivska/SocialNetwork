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
        private IMapper _mapper;

        public LikeController(UserManager<ApplicationUser> userManager,IMapper maper, ILikeService likeService)
        {
            _userManager = userManager;
            _mapper = maper;
            _likeService = likeService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateLike([FromBody] LikeViewModel likeViewModel)
        {
            var likeDto = _mapper.Map<LikeDTO>(likeViewModel);
            var like = _likeService.GetLike(likeViewModel.PublicationId, likeViewModel.UserName);
                
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

                likeDto = _likeService.Create(likeDto);
                if (likeDto == null)
                {
                    return BadRequest();
                }
            }

            return Ok(_mapper.Map<LikeViewModel>(likeDto));
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

            List<LikeViewModel> likers = new List<LikeViewModel>();

            if (likesDtos == null)
            {
                return BadRequest();
            }

            foreach(var like in likesDtos)
            {
                var user = _userManager.Users.Where(x => x.UserName == like.UserName).FirstOrDefault();
                LikeViewModel likeViewModel = new LikeViewModel();
                likeViewModel = _mapper.Map<LikeViewModel>(like);
                likeViewModel.Owner = new UserViewModel();
                if (user.Avatar != null)
                {
                    likeViewModel.Owner.Avatar = ConvertPicture(user.Avatar);
                }
                likers.Add(likeViewModel);
            }

            return Ok(likers);
        }
        private string ConvertPicture(byte[] picture)
        {
            string imageBase64Data = Convert.ToBase64String(picture);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            return imageDataURL;
        }
    }
}