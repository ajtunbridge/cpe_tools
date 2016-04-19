using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using CPE.Data.EntityFramework.Model;
using CPE.Domain.Model;
using CPE.Domain.Services;

namespace CPE.Data.EntityFramework.Repositories
{
    public abstract class RepositoryBase<T> : IDisposable where T : class, IEntity
    {
        protected readonly CPEEntities Entities;

        protected RepositoryBase(CPEEntities entities)
        {
            Entities = entities;
        }

        public void Dispose()
        {
            Entities.Dispose();
        }

        public void Commit()
        {
            Entities.SaveChanges();
        }

        protected T GetById(int id)
        {
            return GetSet().SingleOrDefault(e => e.Id == id);
        }

        protected async Task<T> GetByIdAsync(int id)
        {
            return await GetSet().SingleOrDefaultAsync(e => e.Id == id);
        }

        protected IEnumerable<T> GetAll()
        {
            return GetSet().ToList();
        }

        protected async Task<IEnumerable<T>> GetAllAsync()
        {
            return await GetSet().ToListAsync();
        }

        protected void Insert(T entity)
        {
            AttachInState(entity, EntityState.Added);
        }

        protected void Delete(T entity)
        {
            AttachInState(entity, EntityState.Deleted);
        }

        protected void Update(T entity)
        {
            AttachInState(entity, EntityState.Modified);
        }

        protected DbQuery<T> GetSet()
        {
            return Entities.Set<T>().AsNoTracking();
        }

        private void AttachInState(T entity, EntityState state)
        {
            Entities.Set<T>().Attach(entity);
            Entities.Entry(entity).State = state;
        }
    }
}