using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            return _entities.Set<T>().SingleOrDefault(e => e.Id == id);
        }

        protected async Task<T> GetByIdAsync(int id)
        {
            return await _entities.Set<T>().SingleOrDefaultAsync(e => e.Id == id);
        }

        protected IEnumerable<T> GetAll()
        {
            return _entities.Set<T>().ToList();
        }

        protected async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.Set<T>().ToListAsync();
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

        private void AttachInState(T entity, EntityState state)
        {
            _entities.Set<T>().Attach(entity);
            _entities.Entry(entity).State = state;
        }
    }
}