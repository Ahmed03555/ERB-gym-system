using System;
using System.Collections.Generic;
using System.Text;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.DAL;
using G_NET_02_MVC01.DAL.Context;
using Microsoft.EntityFrameworkCore;
namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class PlanRepositorie : IPlanRepositorie
    {
        private readonly GymContext gymContext;
        public void Add(Plans plans)
        {
            gymContext.plans.Add(plans);
        }

        public PlanRepositorie(GymContext _gymContext)
        {
            gymContext = _gymContext;
        }

        public Task<int> CompleteAsync()
        {
            return gymContext.SaveChangesAsync();
        }

        public void Delete(Plans plans)
        {
            var existingPlan = gymContext.plans.FirstOrDefaultAsync(p => p.Id == plans.Id);
            if(existingPlan != null)
            {
                gymContext.plans.Remove(plans);
            }
        }

        public async Task<IEnumerable<Plans>> GetAll(bool isTacked , CancellationToken ct = default )
        {
            var plans = isTacked ? gymContext.plans : gymContext.plans.AsNoTracking();
            return await gymContext.plans.ToListAsync();
        }

        public async Task<Plans?> GetById(int id, CancellationToken ct = default)
        {
            return await gymContext.plans.FirstOrDefaultAsync(P => P.Id==id);
        }

        public void Update(Plans plans)
        {
           gymContext.plans.Update(plans);
        }
    }
}
