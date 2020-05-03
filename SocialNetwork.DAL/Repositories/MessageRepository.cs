using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.DAL.Repositories
{
    public class MessageRepository : IRepository<Message>
    {
        private NetworkContext _context;

        public MessageRepository(NetworkContext context)
        {
            _context = context;
        }

        public IQueryable<Message> GetAll()
        {
            return _context.Messages;
        }

        public Message Get(int id)
        {
            return _context.Messages.Find(id);
        }

        public Message Create(Message item)
        {
            return _context.Messages.Add(item).Entity;
        }

        public Message Update(Message item)
        {
            return _context.Update(item).Entity;
        }

        public Message Delete(int id)
        {
            var item = _context.Messages.Find(id);
            if (item == null) return null;

            _context.Remove(item);
            return item;
        }
    }
}
