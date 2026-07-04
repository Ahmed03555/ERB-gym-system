using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class BookingRepositorie : GenericRepositorie<Booking>, IBookingRepositorie
    {
        private readonly GymContext gymContext;
        public BookingRepositorie(GymContext gymContext) :base(gymContext)
        {
            this.gymContext = gymContext;
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default)
        {
            var bookings = await gymContext.Booking.AsNoTracking()
                .Where(b => b.SessionId == sessionId)
                .Include(x => x.Member)
                .ToListAsync(cancellationToken);
            return bookings;
        }
    }
}
