using System;
using CPE.Data.EntityFramework.Model;

namespace CPE.Data.EntityFramework.Repositories
{
    public abstract class RepositoryBase : IDisposable
    {
        protected readonly CPEEntities _entities;

        protected RepositoryBase(CPEEntities entities)
        {
            _entities = entities;
        }

        public void Dispose()
        {
            _entities.Dispose();
        }
    }
}