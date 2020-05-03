using System.Collections.Generic;

namespace SocialNetwork.BLL.Interfaces
{
    public interface ICRUDService<T>
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        T Create(T item);
        T Update(int id, T item);
        T Delete(int id);
    }
}
