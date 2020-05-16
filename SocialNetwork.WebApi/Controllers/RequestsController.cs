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
        private IMapper _mapper;

        public RequestsController(UserManager<ApplicationUser> userManager, IRequestService requestService, IMapper mapper)
        {
            _requestService = requestService;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [Authorize]
        [HttpPost]
        public IActionResult CreateRequest([FromBody] RequestViewModel requestViewModel)
        {
            var requestDto = _mapper.Map<RequestDTO>(requestViewModel);
            requestDto.UserName = User.Identity.Name;
            requestDto.ReceiverName = requestViewModel.ReceiverName;
            requestDto.Status = requestViewModel.Status;

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
        [HttpGet("send")]
        public IActionResult GetSendRequests()
        {
            IEnumerable<RequestDTO> requestDtos;

            requestDtos = _requestService.GetAll().Where(x => x.UserName == User.Identity.Name && x.Status == "not confirmed");

            return Ok(_mapper.Map<IEnumerable<RequestViewModel>>(requestDtos));
        }

        [Authorize]
        [HttpGet("received")]
        public IActionResult GetReceivedRequests()
        {
            IEnumerable<RequestDTO> requestDtos;

            requestDtos = _requestService.GetAll().Where(x => x.ReceiverName == User.Identity.Name && x.Status == "not confirmed");

            return Ok(_mapper.Map<IEnumerable<RequestViewModel>>(requestDtos));
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

            return Ok(_mapper.Map<PublicationViewModel>(requestDto));
        }
    }
}