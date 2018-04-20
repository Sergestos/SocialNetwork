using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Infastructure
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll { get; }
        IEnumerable<T> Find(Func<T, bool> predicate);
        T Get(int id);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
    }
}
