using System;
using System.Collections.Generic;
using System.IO;
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
        [HttpPost("{id}")]
        public IActionResult AddPublicationPhoto(int id)
        {
            var photo = Request.Form.Files[0];
            // Person person = new Person { Name = pvm.Name };
            
            byte[] imageData = null;
            // считываем переданный файл в массив байтов
            using (var binaryReader = new BinaryReader(photo.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)photo.Length);
            }
            // установка массива байтов

            var publicationDto = _publicationService.AddPhoto(id,imageData);

            return Ok(_mapper.Map<PublicationViewModel>(publicationDto));
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreatePublication([FromBody] PublicationViewModel publicationViewModel)
        {
            var publicationDto = _mapper.Map<PublicationDTO>(publicationViewModel);
            publicationDto.UserName = User.Identity.Name;
            publicationDto.PublicationDate = DateTime.Now;
            publicationDto.PublicationText = publicationViewModel.PublicationText;

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
            //if (!IsUserOwnLot(id, User.Identity.Name))
            //{
            //    return Forbid();
            //}

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