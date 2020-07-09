using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    public class PublicationsController : ControllerBase
    {
        private IPublicationService _publicationService;
        private IFriendshipService _friendshipService;
        private ILikeService _likeService;
        private ICommentService _commentService;
        private IMapper _mapper;
        private UserManager<ApplicationUser> _userManager;

        public PublicationsController(IPublicationService publicationService, IFriendshipService friendshipService, IMapper mapper, UserManager<ApplicationUser> userManager, ILikeService likeService, ICommentService commentService)
        {
            _publicationService = publicationService;
            _friendshipService = friendshipService;
            _likeService = likeService;
            _commentService = commentService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUserPublications()
        {
            IEnumerable<PublicationDTO> publicationDtos;

            publicationDtos = _publicationService.GetAllUserPublications(User.Identity.Name);

            List<PublicationViewModel> publications = new List<PublicationViewModel>();
            var user = _userManager.Users.Where(x => x.UserName == User.Identity.Name).FirstOrDefault();
            foreach (var p in publicationDtos)
            {
                var publication = _mapper.Map<PublicationViewModel>(p);
                publication.Owner = new UserViewModel();
                var likesDtos = _likeService.GetAll().Where(x => x.PublicationId == p.PublicationId);
                var commentDtos = _commentService.GetAll().Where(x => x.PublicationId == p.PublicationId);
                publication.Likes = new List<LikeViewModel>();
                publication.Comments = new List<CommentViewModel>();
                publication.Owner.UserName = User.Identity.Name;
                if (p.Photo != null)
                {
                    publication.Photo = ConvertPicture(p.Photo);
                }
                if (user.Avatar != null)
                {
                    publication.Owner.Avatar = ConvertPicture(user.Avatar);
                }
                foreach (var l in likesDtos)
                {
                    var like = _mapper.Map<LikeViewModel>(l);
                    var liker = _userManager.Users.Where(x => x.UserName == l.UserName).FirstOrDefault();
                    like.Owner = new UserViewModel();
                    if (liker.Avatar != null)
                    {
                        like.Owner.Avatar = ConvertPicture(liker.Avatar);
                    }
                    publication.Likes.Add(like);
                }
                foreach (var c in commentDtos)
                {
                    var comment = _mapper.Map<CommentViewModel>(c);
                    var commenter = _userManager.Users.Where(x => x.UserName == c.UserName).FirstOrDefault();
                    comment.Owner = new UserViewModel();
                    if (commenter.Avatar != null)
                    {
                        comment.Owner.Avatar = ConvertPicture(commenter.Avatar);
                    }
                    publication.Comments.Add(comment);
                }

                publications.Add(publication);
            }
            publications.Reverse();
            return Ok(_mapper.Map<List<PublicationViewModel>>(publications));
        }

        [Authorize]
        [HttpGet("news")]
        public IActionResult GetNews()
        {
            var friends = _friendshipService.GetAll()
                .Where(x => x.UserName == User.Identity.Name || x.FriendName == User.Identity.Name);

            List<PublicationViewModel> news = new List<PublicationViewModel>();
            DateTime now = DateTime.Now;
            DateTime oneDay = now.AddHours(-24);
            IEnumerable<PublicationDTO> publicationDtos;

            foreach (var friend in friends)
            {
                if (friend.UserName == User.Identity.Name)
                {
                    publicationDtos = _publicationService.GetAll().Where(x => x.UserName == friend.FriendName
                    && x.PublicationDate < DateTime.Now && x.PublicationDate > oneDay);

                }
                else
                {
                    publicationDtos = _publicationService.GetAll().Where(x => x.UserName == friend.UserName
                    && x.PublicationDate < DateTime.Now && x.PublicationDate > oneDay);
                }

                foreach (var p in publicationDtos.Reverse())
                {
                    PublicationViewModel publication = new PublicationViewModel();
                    var user = _userManager.Users.Where(x => x.UserName == p.UserName).FirstOrDefault();
                    publication = _mapper.Map<PublicationViewModel>(p);
                    publication.Owner = new UserViewModel();
                    var likesDtos = _likeService.GetAll().Where(x => x.PublicationId == p.PublicationId);
                    var commentDtos = _commentService.GetAll().Where(x => x.PublicationId == p.PublicationId);
                    publication.Likes = new List<LikeViewModel>();
                    publication.Comments = new List<CommentViewModel>();

                    if (user.Avatar != null)
                    {
                        publication.Owner.Avatar = ConvertPicture(user.Avatar);
                    }
                    if (p.Photo != null)
                    {
                        publication.Photo = ConvertPicture(p.Photo);
                    }
                    foreach (var l in likesDtos)
                    {
                        var like = _mapper.Map<LikeViewModel>(l);
                        var liker = _userManager.Users.Where(x => x.UserName == l.UserName).FirstOrDefault();
                        like.Owner = new UserViewModel();
                        if (liker.Avatar != null)
                        {
                            like.Owner.Avatar = ConvertPicture(liker.Avatar);
                        }
                        publication.Likes.Add(like);
                    }
                    foreach (var c in commentDtos)
                    {
                        var comment = _mapper.Map<CommentViewModel>(c);
                        var commenter = _userManager.Users.Where(x => x.UserName == c.UserName).FirstOrDefault();
                        comment.Owner = new UserViewModel();
                        if (commenter.Avatar != null)
                        {
                            comment.Owner.Avatar = ConvertPicture(commenter.Avatar);
                        }
                        publication.Comments.Add(comment);
                    }
                    news.Add(publication);
                }
            }
                return Ok(_mapper.Map<List<PublicationViewModel>>(news));
            }

        private string ConvertPicture(byte[] picture)
        {
            string imageBase64Data = Convert.ToBase64String(picture);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            return imageDataURL;
        }
        [Authorize]
        [HttpPost("{id}")]
        public IActionResult AddPublicationPhoto(int id)
        {
            var photo = Request.Form.Files[0];
            
            byte[] imageData = null;

            using (var binaryReader = new BinaryReader(photo.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)photo.Length);
            }

            var publicationDto = _publicationService.AddPhoto(id,imageData);

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePublication([FromBody] PublicationViewModel publicationViewModel)
        {
            var publicationDto = _mapper.Map<PublicationDTO>(publicationViewModel);
            publicationDto.PublicationDate = DateTime.Now;
                        publicationDto = _publicationService.Create(publicationDto);

            if (publicationDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }
        
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdatePublication(int id, [FromBody] PublicationViewModel publicationViewModel)
        {

            var publicationDto = _mapper.Map<PublicationDTO>(publicationViewModel);
            publicationDto = _publicationService.Update(id, publicationDto);

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeletePublicationById(int id)
        {
            if (!IsUserOwner(id, User.Identity.Name))
            {
                return Forbid();
            }

            var publicationDto = _publicationService.Delete(id);

            if (publicationDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }

        private bool IsUserOwner(int publicationId, string userName)
        {
            var publicationDto = _publicationService.Get(publicationId);

            if (publicationDto == null)
            {
                return false;
            }

            return publicationDto.UserName == userName;
        }
    }
}