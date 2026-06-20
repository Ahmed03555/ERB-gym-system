using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Numerics;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class GenericRepositorie<TEntity> : IGenericRepositorie<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymContext gymContext;

        public GenericRepositorie(GymContext _gymContext)
        {
            gymContext = _gymContext;
        }
        public void Add(TEntity entity)
        {
            gymContext.Set<TEntity>().Add(entity);
        }

        public async Task<bool?> AnyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken ct = default)
        {
            return await gymContext.Set<TEntity>().AnyAsync(expression, ct);
        }

        public Task<int> CompleteAsync()
        {
            return gymContext.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            var existingEntity = gymContext.Set<TEntity>().FirstOrDefault(e => e.Id == id);
            if (existingEntity != null)
            {
                gymContext.Set<TEntity>().Remove(existingEntity);
            }
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> expression,bool isTracked=false,CancellationToken ct = default)
        {
            var entity = isTracked ? gymContext.Set<TEntity>() : gymContext.Set<TEntity>().AsNoTracking();
            return await entity.FirstOrDefaultAsync(expression,ct);
        }

        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
            var entity = isTracked ? gymContext.Set<TEntity>() : gymContext.Set<TEntity>().AsNoTracking();
            return await entity.ToListAsync();
        }

        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
            return await gymContext.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public void Update(TEntity entity)
        {
            gymContext.Set<TEntity>().Update(entity);
        }
    }
}
