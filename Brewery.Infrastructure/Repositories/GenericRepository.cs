using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Brewery.Core.Interfaces.Repositories;

namespace Brewery.Infrastructure.Repositories
{
    public abstract class GenericRepository<T, A> : IGenericRepository<T> where T : class, new() where A : class
    {
        private ApplicationDbContext _context;
        private DbSet<A> table;

        public GenericRepository(ApplicationDbContext _context)
        {
            this._context = _context;
            table = _context.Set<A>();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            var a = await _context.Set<A>().ToListAsync();
            return a.Select(x => EntityToModel(x)).ToList();
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            var result = await table.FindAsync(id);
            return EntityToModel(result);
        }

        public virtual async Task<T> AddAsync(T obj)
        {
            var result = await table.AddAsync(ModelToEntity(obj));
            _context.SaveChanges();
            return EntityToModel(result.Entity);
        }

        public virtual async void InsertRangeAsync(IEnumerable<T> obj)
        {
            await table.AddRangeAsync(obj.Select(x => ModelToEntity(x)));
            _context.SaveChanges();
        }

        public virtual T Update(T obj)
        {
            _context.Entry(ModelToEntity(obj)).State = EntityState.Modified;
            _context.SaveChanges();
            return obj;
        }

        public virtual void Delete(Guid id)
        {
            A existing = table.Find(id);
            table.Remove(existing);
        }

        public virtual async void SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task<int> CountAsync()
        {
            return await table.CountAsync();
        }

        protected abstract T EntityToModel(A entity);

        protected abstract A ModelToEntity(T entity);
    }
}
