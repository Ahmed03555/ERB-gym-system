using G_NET_02_MVC01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Interface
{
    public interface IBookingRepositorie : IGenericRepositorie<Booking>
    {
        public Task<IEnumerable<Booking>> GetAllBookingsBySessionIdAsync(int sessionId, CancellationToken cancellationToken = default);
    }
}
