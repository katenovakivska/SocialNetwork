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
    public class CommentsController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private ICommentService _commentService;
        private IMapper _mapper;

        public CommentsController(UserManager<ApplicationUser> userManager, IMapper maper, ICommentService commentService)
        {
            _userManager = userManager;
            _mapper = maper;
            _commentService = commentService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateComment([FromBody] CommentViewModel commentViewModel)
        {
            CommentDTO commentDto = _mapper.Map<CommentDTO>(commentViewModel);
            commentDto.CommentDate = DateTime.Now;
            commentDto.PublicationId = commentViewModel.PublicationId;
            commentDto.UserName = User.Identity.Name;

            commentDto = _commentService.Create(commentDto);

            if (commentDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<CommentViewModel>(commentDto));
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetPublicationComments(int id)
        {
            var commentDtos = _commentService.GetAll().Where(x => x.PublicationId == id);

            List<UserCommentViewModel> comments = new List<UserCommentViewModel>();

            if (commentDtos == null)
            {
                return BadRequest();
            }

            foreach (var comment in commentDtos)
            {
                var user = _userManager.Users.Where(x => x.UserName == comment.UserName).FirstOrDefault();
                UserCommentViewModel userCommentViewModel = new UserCommentViewModel();
                userCommentViewModel.UserName = user.UserName;
                userCommentViewModel.CommentDate = comment.CommentDate;
                userCommentViewModel.CommentText = comment.CommentText;
                userCommentViewModel.CommentId = comment.CommentId;
                if (user.Avatar != null)
                {
                    string imageBase64Data = Convert.ToBase64String(user.Avatar);
                    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
                    userCommentViewModel.Avatar = imageDataURL;
                }
                comments.Add(userCommentViewModel);
            }

            return Ok(comments);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteCommentById(int id)
        {
            var commentDto = _commentService.Delete(id);

            if (commentDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<CommentViewModel>(commentDto));
        }
    }
}