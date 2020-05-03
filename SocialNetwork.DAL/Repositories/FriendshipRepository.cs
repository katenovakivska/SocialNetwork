using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.DAL.Repositories
{
    public class FriendshipRepository : IRepository<Friendship>
    {
        private NetworkContext _context;

        public FriendshipRepository(NetworkContext context)
        {
            _context = context;
        }

        public IQueryable<Friendship> GetAll()
        {
            return _context.Friendships;
        }

        public Friendship Get(int id)
        {
            return _context.Friendships.Find(id);
        }

        public Friendship Create(Friendship item)
        {
            return _context.Friendships.Add(item).Entity;
        }

        public Friendship Update(Friendship item)
        {
            return _context.Update(item).Entity;
        }

        public Friendship Delete(int id)
        {
            var item = _context.Friendships.Find(id);
            if (item == null) return null;

            _context.Remove(item);
            return item;
        }
    }
}
