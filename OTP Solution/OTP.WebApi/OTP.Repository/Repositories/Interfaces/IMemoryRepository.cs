using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OTP.Repository.Repositories.Interfaces
{
    public interface IMemoryRepository<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> Search(
            Expression<Func<TEntity, bool>> filter = null);
        IQueryable<TEntity> SearchQueryable(
            Expression<Func<TEntity, bool>> filter = null);
        Task<TEntity> Get(Guid id);
        Task<TEntity> Create(TEntity entity);
        Task Delete(Guid id);
        Task<TEntity> Update(TEntity entity);
        IQueryable<TEntity> GetIQueryable();
    }
}
