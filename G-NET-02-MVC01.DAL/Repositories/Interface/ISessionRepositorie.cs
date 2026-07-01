using G_NET_02_MVC01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Interface
{
    public interface ISessionRepositorie : IGenericRepositorie<Session>
    {
        Task<IEnumerable<Session>> GetAllSessionsWithTrainerAndCategoryAsync(CancellationToken cancellationToken = default);
        Task<Session> GetSessionWithTrainerAndCategoryByIdAsync(int sessionId, CancellationToken cancellationToken = default);
        Task<int> GetCountOfBookedSlotAsync(int sessionId, CancellationToken cancellationToken = default);

    }
}
