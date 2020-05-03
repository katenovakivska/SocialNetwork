using AutoMapper;
using SocialNetwork.BLL.DTOs;
using SocialNetwork.BLL.Interfaces;
using SocialNetwork.BLL.Validators;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.BLL.Services
{
    public class FriendshipService : IFriendshipService
    {
        private INetworkUnitOfWork _uow;

        private IMapper _mapper;

        public FriendshipService(INetworkUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<FriendshipDTO> GetAll()
        {
            var friendships = _uow.FriendshipRepository.GetAll().ToList();

            return _mapper.Map<IEnumerable<FriendshipDTO>>(friendships);
        }

        public IEnumerable<FriendshipDTO> GetAllByFriendshipId(int friendshipId)
        {
            var friendships = _uow.FriendshipRepository.GetAll()
                .Where(f => f.FriendshipId == friendshipId)
                .ToList();

            return _mapper.Map<IEnumerable<FriendshipDTO>>(friendships);
        }

        public FriendshipDTO Get(int id)
        {
            var friendship = _uow.FriendshipRepository.Get(id);

            return _mapper.Map<FriendshipDTO>(friendship);
        }

        public FriendshipDTO Create(FriendshipDTO item)
        {
            if (!FriendshipValidator.IsFriendshipValid(item))
            {
                return null;
            }

            var friendship = _uow.FriendshipRepository.Get((int)item.FriendshipId);
            if (friendship == null)
            {
                return null;
            }

            var friendshipDto = _mapper.Map<Friendship>(item);
            friendshipDto = _uow.FriendshipRepository.Create(friendship);

            _uow.Save();

            return _mapper.Map<FriendshipDTO>(friendshipDto);
        }

        public FriendshipDTO Update(int id, FriendshipDTO item)
        {
            if (!FriendshipValidator.IsFriendshipValid(item))
            {
                return null;
            }

            if (_uow.FriendshipRepository.Get((int)item.FriendshipId) == null)
            {
                return null;
            }

            item.FriendshipId = id;
            var friendship = _mapper.Map<Friendship>(item);
            friendship = _uow.FriendshipRepository.Update(friendship);
            _uow.Save();

            return _mapper.Map<FriendshipDTO>(friendship);
        }

        public FriendshipDTO Delete(int id)
        {
            var friendship = _uow.FriendshipRepository.Delete(id);
            _uow.Save();

            return _mapper.Map<FriendshipDTO>(friendship);
        }
    }
}
