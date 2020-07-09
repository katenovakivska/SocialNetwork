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
    public class MessageService : IMessageService
    {
        private INetworkUnitOfWork _uow;

        private IMapper _mapper;

        public MessageService(INetworkUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<MessageDTO> GetAll()
        {
            var messages = _uow.MessageRepository.GetAll().ToList();

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public IEnumerable<MessageDTO> GetChatMessages(string userName, string friendName)
        {
            var messages = _uow.MessageRepository.GetAll()
                .Where(x => (x.UserName == userName && x.ReceiverName == friendName)
                || (x.UserName == friendName && x.ReceiverName == userName)).ToList();

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public IEnumerable<MessageDTO> GetAllByMessageId(int messageId)
        {
            var messages = _uow.MessageRepository.GetAll()
                .Where(m => m.MessageId == messageId).ToList();

            return _mapper.Map<IEnumerable<MessageDTO>>(messages);
        }

        public MessageDTO Get(int id)
        {
            var message = _uow.MessageRepository.Get(id);

            return _mapper.Map<MessageDTO>(message);
        }

        public MessageDTO Create(MessageDTO item)
        {
            if (!MessageValidator.IsMessageValid(item))
            {
                return null;
            }

            var messageDto = _mapper.Map<Message>(item);
            messageDto = _uow.MessageRepository.Create(messageDto);

            _uow.Save();

            return _mapper.Map<MessageDTO>(messageDto);
        }

        public MessageDTO Update(int id, MessageDTO item)
        {
            if (!MessageValidator.IsMessageValid(item))
            {
                return null;
            }

            if (_uow.MessageRepository.Get((int)item.MessageId) == null)
            {
                return null;
            }

            item.MessageId = id;
            var message = _mapper.Map<Message>(item);
            message = _uow.MessageRepository.Update(message);
            _uow.Save();

            return _mapper.Map<MessageDTO>(message);
        }

        public MessageDTO Delete(int id)
        {
            var message = _uow.MessageRepository.Delete(id);
            _uow.Save();

            return _mapper.Map<MessageDTO>(message);
        }
    }
}
