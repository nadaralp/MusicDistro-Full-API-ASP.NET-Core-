using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MusicDistro.Core.Repositories;

namespace TDistory.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            Context = context;
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return
                await Context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return
                await Context.Set<T>().ToListAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicatExpression)
        {
            return
                await Context.Set<T>().FirstOrDefaultAsync(predicatExpression);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> expression)
        {
            return Context.Set<T>().Where(expression);
        }

        public async Task AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await Context.Set<T>().AddRangeAsync(entities);
        }

        public void Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            Context.Set<T>().RemoveRange(entities);
        }

        public void Update(T oldEntity, T newEntity)
        {
            Context.Entry(oldEntity).CurrentValues.SetValues(newEntity);
        }
    }
}
