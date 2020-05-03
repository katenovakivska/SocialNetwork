using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialNetwork.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        T Get(int id);
        T Create(T item);
        T Update(T item);
        T Delete(int id);
    }
}
