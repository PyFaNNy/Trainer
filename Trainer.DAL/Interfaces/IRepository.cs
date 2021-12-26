using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Trainer.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(Guid id);
        Task<IEnumerable<T>> Range(IEnumerable<T> list);
        Task<T> Create(T item);
        Task<T> Update(T item);
        Task Delete(Guid id);
    }
}
