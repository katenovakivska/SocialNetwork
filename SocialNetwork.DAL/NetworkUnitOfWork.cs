using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using SocialNetwork.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL
{
    public class NetworkUnitOfWork: INetworkUnitOfWork
    {
        private NetworkContext _context;

        private PublicationRepository _publicationRepository;

        private CommentRepository _commentRepository;

        private LikeRepository _likeRepository;

        private FriendshipRepository _friendshipRepository;

        private MessageRepository _messageRepository;

        private RequestRepository _requestRepository;

        public NetworkUnitOfWork(NetworkContext context)
        {
            _context = context;
        }

        public IRepository<Publication> PublicationRepository
        {
            get
            {
                if (_publicationRepository == null)
                {
                    _publicationRepository = new PublicationRepository(_context);
                }

                return _publicationRepository;
            }
        }

        public IRepository<Comment> CommentRepository
        {
            get
            {
                if (_commentRepository == null)
                {
                    _commentRepository = new CommentRepository(_context);
                }

                return _commentRepository;
            }
        }

        public IRepository<Like> LikeRepository
        {
            get
            {
                if (_likeRepository == null)
                {
                    _likeRepository = new LikeRepository(_context);
                }

                return _likeRepository;
            }
        }

        public IRepository<Friendship> FriendshipRepository
        {
            get
            {
                if (_friendshipRepository == null)
                {
                    _friendshipRepository = new FriendshipRepository(_context);
                }

                return _friendshipRepository;
            }
        }

        public IRepository<Message> MessageRepository
        {
            get
            {
                if (_messageRepository == null)
                {
                    _messageRepository = new MessageRepository(_context);
                }

                return _messageRepository;
            }
        }

        public IRepository<Request> RequestRepository
        {
            get
            {
                if (_requestRepository == null)
                {
                    _requestRepository = new RequestRepository(_context);
                }

                return _requestRepository;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }
    }
}
