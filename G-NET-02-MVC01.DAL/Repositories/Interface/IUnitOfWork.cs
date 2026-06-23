using G_NET_02_MVC01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Interface
{
    public interface IUnitOfWork
    {
        public IGenericRepositorie<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new();
        public Task<int> CompleteAsync();
    }
}
