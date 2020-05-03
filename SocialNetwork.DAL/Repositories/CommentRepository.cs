using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.DAL.Repositories
{
    public class CommentRepository : IRepository<Comment>
    {
        private NetworkContext _context;

        public CommentRepository(NetworkContext context)
        {
            _context = context;
        }

        public IQueryable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public Comment Get(int id)
        {
            return _context.Comments.Find(id);
        }

        public Comment Create(Comment item)
        {
            return _context.Comments.Add(item).Entity;
        }

        public Comment Update(Comment item)
        {
            return _context.Update(item).Entity;
        }

        public Comment Delete(int id)
        {
            var item = _context.Comments.Find(id);
            if (item == null) return null;

            _context.Remove(item);
            return item;
        }
    }
}
