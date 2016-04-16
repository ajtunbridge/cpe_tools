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
        private readonly CPEEntities _entities;

        protected RepositoryBase(CPEEntities entities)
        {
            _entities = entities;
        }

        public void Dispose()
        {
            _entities.Dispose();
        }

        public void Commit()
        {
            _entities.SaveChanges();
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
            return _entities.Set<T>().AsNoTracking();
        }

        private void AttachInState(T entity, EntityState state)
        {
            _entities.Set<T>().Attach(entity);
            _entities.Entry(entity).State = state;
        }
    }
}