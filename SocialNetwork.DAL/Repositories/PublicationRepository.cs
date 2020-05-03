using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.DAL.Repositories
{
    public class PublicationRepository : IRepository<Publication>
    {
        private NetworkContext _context;

        public PublicationRepository(NetworkContext context)
        {
            _context = context;
        }

        public IQueryable<Publication> GetAll()
        {
            return _context.Publications;
        }

        public Publication Get(int id)
        {
            return _context.Publications.Find(id);
        }

        public Publication Create(Publication item)
        {
            return _context.Publications.Add(item).Entity;
        }

        public Publication Update(Publication item)
        {
            return _context.Update(item).Entity;
        }

        public Publication Delete(int id)
        {
            var item = _context.Publications.Find(id);
            if (item == null) return null;

            _context.Remove(item);
            return item;
        }
    }
}
