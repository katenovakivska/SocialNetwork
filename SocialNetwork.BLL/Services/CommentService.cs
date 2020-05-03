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
    public class CommentService : ICommentService
    {
        private INetworkUnitOfWork _uow;

        private IMapper _mapper;

        public CommentService(INetworkUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<CommentDTO> GetAll()
        {
            var comments = _uow.CommentRepository.GetAll().ToList();

            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public IEnumerable<CommentDTO> GetAllByCommentId(int commentId)
        {
            var comments = _uow.CommentRepository.GetAll()
                .Where(c => c.CommentId == commentId).ToList();

            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        public CommentDTO Get(int id)
        {
            var comment= _uow.CommentRepository.Get(id);

            return _mapper.Map<CommentDTO>(comment);
        }

        public CommentDTO Create(CommentDTO item)
        {
            if (!CommentValidator.IsCommentValid(item))
            {
                return null;
            }

            var comment = _uow.CommentRepository.Get((int)item.CommentId);
            if (comment == null)
            {
                return null;
            }

            var commentDto = _mapper.Map<Comment>(item);
            commentDto = _uow.CommentRepository.Create(comment);

            _uow.Save();

            return _mapper.Map<CommentDTO>(commentDto);
        }

        public CommentDTO Update(int id, CommentDTO item)
        {
            if (!CommentValidator.IsCommentValid(item))
            {
                return null;
            }

            if (_uow.CommentRepository.Get((int)item.CommentId) == null)
            {
                return null;
            }

            item.CommentId = id;
            var comment = _mapper.Map<Comment>(item);
            comment = _uow.CommentRepository.Update(comment);
            _uow.Save();

            return _mapper.Map<CommentDTO>(comment);
        }

        public CommentDTO Delete(int id)
        {
            var comment = _uow.CommentRepository.Delete(id);
            _uow.Save();

            return _mapper.Map<CommentDTO>(comment);
        }
    }
}
