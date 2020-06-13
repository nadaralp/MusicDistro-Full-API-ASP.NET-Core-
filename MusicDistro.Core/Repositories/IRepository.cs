using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MusicDistro.Core.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicatExpression);
        IEnumerable<T> Where(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Update(T oldEntity, T newEntity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
