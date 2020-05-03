﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialNetwork.BLL.DTOs;
using SocialNetwork.BLL.Interfaces;
using SocialNetwork.PL.ViewModels;

namespace SocialNetwork.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private IPublicationService _publicationService;
        private IMapper _mapper;

        public PublicationsController(IPublicationService publicationService, IMapper mapper)
        {
            _publicationService = publicationService;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUserPublications()
        {
            IEnumerable<PublicationDTO> publicationDtos;

            publicationDtos = _publicationService.GetAllUserPublications(User.Identity.Name);

            return Ok(_mapper.Map<IEnumerable<PublicationViewModel>>(publicationDtos));
        }
        //public IActionResult GetAllPublications(int? pageNumber, int? pageElementCount)
        //{
        //    IEnumerable<PublicationDTO> publicationDtos;

        //    if (pageNumber.HasValue && pageElementCount.HasValue)
        //    {
        //        publicationDtos = _publicationService.GetPage(pageNumber.Value, pageElementCount.Value);
        //    }
        //    else
        //    {
        //        publicationDtos = _publicationService.GetAll();
        //    }

        //    return Ok(_mapper.Map<IEnumerable<PublicationViewModel>>(publicationDtos));
        //}

        [HttpGet("{id}")]
        public IActionResult GetPublicationById(int id)
        {
            var publicationDto = _publicationService.Get(id);

            if (publicationDto == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePublication([FromBody] PublicationViewModel publicationViewModel)
        {
            var publicationDto = _mapper.Map<PublicationDTO>(publicationViewModel);
            publicationDto.UserName = User.Identity.Name;
            publicationDto.PublicationDate = DateTime.Now;
            publicationDto = _publicationService.Create(publicationDto);

            if (publicationDto == null)
            {
                return BadRequest();
            }

            return Created(Request.Path + "/" + publicationDto.PublicationId, _mapper.Map<PublicationViewModel>(publicationDto));
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdatePublication(int id, [FromBody] PublicationViewModel publicationViewModel)
        {
            //if (!IsUserOwnLot(id, User.Identity.Name))
            //{
            //    return Forbid();
            //}

            var publicationDto = _mapper.Map<PublicationDTO>(publicationViewModel);
            publicationDto = _publicationService.Update(id, publicationDto);

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }

       // [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeletePublicationById(int id)
        {
            //if (!IsUserOwnLot(id, User.Identity.Name))
            //{
            //    return Forbid();
            //}

            var publicationDto = _publicationService.Delete(id);

            if (publicationDto == null)
            {
                return BadRequest();
            }

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }

        //private bool IsUserOwnLot(int lotId, string userName)
        //{
        //    var lotDto = _lotService.Get(lotId);

        //    if (lotDto == null)
        //    {
        //        return false;
        //    }

        //    return lotDto.UserName == userName;
        //}
    }
}