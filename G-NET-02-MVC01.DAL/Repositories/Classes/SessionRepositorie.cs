using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class SessionRepositorie : GenericRepositorie<Session>, ISessionRepositorie
    {
        private readonly GymContext gymContext;

        public SessionRepositorie(GymContext gymContext) : base(gymContext)
        {
            this.gymContext=gymContext;
        }
        public async Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken cancellationToken = default)
        {
            var sessions = gymContext.Session.AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category);
            return await sessions.ToListAsync(cancellationToken);

        }

        public Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            return gymContext.Booking.AsNoTracking()
                .CountAsync(b => b.SessionId == sessionId, cancellationToken);
        }

        public async Task<Session> GetSessionWithTrainerAndCategoryByIdAsync(int sessionId, CancellationToken cancellationToken = default)
        { 
            var session = gymContext.Session.AsNoTracking()
                .Include(s => s.Trainer)
                .Include(s => s.Category)
                .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);
            return await session;
        }        
    }
}
