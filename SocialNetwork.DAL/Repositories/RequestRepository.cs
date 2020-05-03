using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.DAL.Repositories
{
    public class RequestRepository : IRepository<Request>
    {
        private NetworkContext _context;

        public RequestRepository(NetworkContext context)
        {
            _context = context;
        }

        public IQueryable<Request> GetAll()
        {
            return _context.Requests;
        }

        public Request Get(int id)
        {
            return _context.Requests.Find(id);
        }

        public Request Create(Request item)
        {
            return _context.Requests.Add(item).Entity;
        }

        public Request Update(Request item)
        {
            return _context.Update(item).Entity;
        }

        public Request Delete(int id)
        {
            var item = _context.Requests.Find(id);
            if (item == null) return null;

            _context.Remove(item);
            return item;
        }
    }
}
