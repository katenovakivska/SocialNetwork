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
    public class LikeService : ILikeService
    {
        private INetworkUnitOfWork _uow;

        private IMapper _mapper;

        public LikeService(INetworkUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<LikeDTO> GetAll()
        {
            var likes = _uow.LikeRepository.GetAll().ToList();

            return _mapper.Map<IEnumerable<LikeDTO>>(likes);
        }

        public IEnumerable<LikeDTO> GetAllByLikeId(int likeId)
        {
            var likes = _uow.LikeRepository.GetAll()
                .Where(l => l.LikeId == likeId).ToList();

            return _mapper.Map<IEnumerable<LikeDTO>>(likes);
        }

        public LikeDTO Get(int id)
        {
            var like = _uow.LikeRepository.Get(id);

            return _mapper.Map<LikeDTO>(like);
        }

        public LikeDTO Create(LikeDTO item)
        {
            if (!LikeValidator.IsLikeValid(item))
            {
                return null;
            }

            var likeDto = _mapper.Map<Like>(item);
            likeDto = _uow.LikeRepository.Create(likeDto);

            _uow.Save();

            return _mapper.Map<LikeDTO>(likeDto);
        }

        public LikeDTO Update(int id, LikeDTO item)
        {
            if (!LikeValidator.IsLikeValid(item))
            {
                return null;
            }

            if (_uow.LikeRepository.Get((int)item.LikeId) == null)
            {
                return null;
            }

            item.LikeId = id;
            var like = _mapper.Map<Like>(item);
            like = _uow.LikeRepository.Update(like);
            _uow.Save();

            return _mapper.Map<LikeDTO>(like);
        }

        public LikeDTO Delete(int id)
        {
            var like = _uow.LikeRepository.Delete(id);
            _uow.Save();

            return _mapper.Map<LikeDTO>(like);
        }
    }
}
