using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.DAL.Repositories
{
    public class LikeRepository : IRepository<Like>
    {
        private NetworkContext _context;

        public LikeRepository(NetworkContext context)
        {
            _context = context;
        }

        public IQueryable<Like> GetAll()
        {
            return _context.Likes;
        }

        public Like Get(int id)
        {
            return _context.Likes.Find(id);
        }

        public Like Create(Like item)
        {
            return _context.Likes.Add(item).Entity;
        }

        public Like Update(Like item)
        {
            return _context.Update(item).Entity;
        }

        public Like Delete(int id)
        {
            var item = _context.Likes.Find(id);
            if (item == null) return null;

            _context.Remove(item);
            return item;
        }
    }
}
