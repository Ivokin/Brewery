using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Brewery.Core.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);

        Task<T> AddAsync(T obj);

        void InsertRangeAsync(IEnumerable<T> obj);

        T Update(T obj);

        void Delete(Guid id);

        void SaveAsync();

        Task<int> CountAsync();
    }
}
