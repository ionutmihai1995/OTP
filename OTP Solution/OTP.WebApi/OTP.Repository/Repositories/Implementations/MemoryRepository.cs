using Microsoft.EntityFrameworkCore;
using OTP.Repository.Context;
using OTP.Repository.Entities;
using OTP.Repository.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OTP.Repository.Repositories.Implementations
{
    public class MemoryRepository<TEntity> : IMemoryRepository<TEntity>
        where TEntity : IdentifiableEntity
    {

        protected OTPContext context;

        protected DbSet<TEntity> dbSet;

        public MemoryRepository(OTPContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
        public virtual async Task<IEnumerable<TEntity>> Search(
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public virtual IQueryable<TEntity> SearchQueryable(
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }

        public async virtual Task<TEntity> Get(Guid id)
        {
            IQueryable<TEntity> query = dbSet;
            return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            var updatedEntity = await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();

            return updatedEntity.Entity;
        }

        public async virtual Task Delete(Guid id)
        {
            TEntity entityToDelete = await Get(id);
            dbSet.Remove(entityToDelete);
            await context.SaveChangesAsync();
        }

        public virtual async Task<TEntity> Update(TEntity entity)
        {
            var updatedEntity = dbSet.Update(entity);
            await context.SaveChangesAsync();

            return updatedEntity.Entity;
        }

        public virtual IQueryable<TEntity> GetIQueryable()
        {
            return this.dbSet.AsQueryable<TEntity>();
        }
    }
}
