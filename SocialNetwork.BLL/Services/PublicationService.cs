using AutoMapper;
using SocialNetwork.BLL.DTOs;
using SocialNetwork.BLL.Interfaces;
using SocialNetwork.BLL.Validators;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SocialNetwork.BLL.Services
{
    public class PublicationService: IPublicationService
    {
        private INetworkUnitOfWork _uow;

        private IMapper _mapper;

        public PublicationService(INetworkUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<PublicationDTO> GetAll()
        {
            var publications = _uow.PublicationRepository.GetAll().ToList();

            return _mapper.Map<IEnumerable<PublicationDTO>>(publications);
        }

        public IEnumerable<PublicationDTO> GetAllUserPublications(string userName)
        {
            if(userName == null)
            {
                return null;
            }
            var publications = _uow.PublicationRepository.GetAll().Where(x => x.UserName == userName).ToList();

            return _mapper.Map<IEnumerable<PublicationDTO>>(publications);
        }

        public IEnumerable<PublicationDTO> GetPage(int pageNumber, int pageElementCount)
        {
            var publications = _uow.PublicationRepository.GetAll()
                .Skip((pageNumber - 1) * pageElementCount)
                .Take(pageElementCount)
                .ToList();

            return _mapper.Map<IEnumerable<PublicationDTO>>(publications);
        }
        public IEnumerable<PublicationDTO> GetAllByUserId(int publicationId)
        {
            var publications = _uow.PublicationRepository.GetAll()
                .Where(p => p.PublicationId == publicationId).ToList();

            return _mapper.Map<IEnumerable<PublicationDTO>>(publications);
        }

        public PublicationDTO Get(int id)
        {
            var publication = _uow.PublicationRepository.Get(id);

            return _mapper.Map<PublicationDTO>(publication);
        }

        public PublicationDTO Create(PublicationDTO item)
        {
            if (!PublicationValidator.IsPublicationValid(item))
            {
                return null;
            }

            var publicationDto = _mapper.Map<Publication>(item);
            publicationDto = _uow.PublicationRepository.Create(publicationDto);

            _uow.Save();

            return _mapper.Map<PublicationDTO>(publicationDto);
        }

        public PublicationDTO Update(int id, PublicationDTO item)
        {
            if (!PublicationValidator.IsPublicationValid(item))
            {
                return null;
            }

            if (_uow.PublicationRepository.Get((int)item.PublicationId) == null)
            {
                return null;
            }

            item.PublicationId = id;
            var publication = _mapper.Map<Publication>(item);
            publication = _uow.PublicationRepository.Update(publication);
            _uow.Save();

            return _mapper.Map<PublicationDTO>(publication);
        }

        public PublicationDTO Delete(int id)
        {
            var publication = _uow.PublicationRepository.Delete(id);
            _uow.Save();

            return _mapper.Map<PublicationDTO>(publication);
        }

       
    }
}

