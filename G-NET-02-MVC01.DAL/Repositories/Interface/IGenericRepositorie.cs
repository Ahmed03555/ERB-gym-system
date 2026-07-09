using G_NET_02_MVC01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Interface
{
    public interface IGenericRepositorie<TEntity> where TEntity : BaseEntity, new()
    {
        Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default);
        Task<TEntity?> GetById(int id, CancellationToken ct = default);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression, bool isTracked, CancellationToken ct = default);

        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default);

        Task<int> CountAsync(Expression<Func<TEntity, bool>>? predicate =null,CancellationToken cancellationToken = default);

        void Update(TEntity entity);
        void Add(TEntity entity);
        void Delete(int id);
        Task<int> CompleteAsync();
    }

}
