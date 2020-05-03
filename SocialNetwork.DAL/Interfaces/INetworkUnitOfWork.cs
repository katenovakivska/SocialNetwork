using SocialNetwork.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialNetwork.DAL.Interfaces
{
    public interface INetworkUnitOfWork
    {
        public IRepository<Publication> PublicationRepository { get; }
        public IRepository<Comment> CommentRepository { get; }
        public IRepository<Like> LikeRepository { get; }
        public IRepository<Friendship> FriendshipRepository { get; }
        public IRepository<Message> MessageRepository { get; }
        public IRepository<Request> RequestRepository { get; }

        public int Save();
    }
}
