using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.DAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymContext _context;
        private readonly Dictionary<string, object> _repositories = [];
        public UnitOfWork(GymContext context)
        {
            _context = context;
            sessionRepositorie = new SessionRepositorie(_context);
        }

        public ISessionRepositorie sessionRepositorie { get; }

        public ITrainerRepositorie trainerRepositorie { get; }

        public async Task<int> CompleteAsync()
        {
           return await _context.SaveChangesAsync();
        }

        public IGenericRepositorie<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if(_repositories.TryGetValue(TypeName, out object OldRepository))
            {
                return (IGenericRepositorie<TEntity>)OldRepository;
            }
            var NewRepository = new GenericRepositorie<TEntity>(_context);
            _repositories[TypeName] = NewRepository;
            return NewRepository;
        }
    }
}
