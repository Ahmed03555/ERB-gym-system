using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Interface
{
    public interface ISessionServices
    {
        public Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken cancellationToken = default);
        Task<Result> CreatSessionAsync(CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default);

        public Task<IEnumerable<CategorySelectViewModel>> GetCategorySelectListAsync(CancellationToken cancellationToken = default);

        public Task<IEnumerable<TrainerSelectViewModel>> GetTrainerSelectListAsync(CancellationToken cancellationToken = default);
        public Task<SessionViewModel?> GetSessionByIdAsync(int sessionId,CancellationToken cancellationToken);

        public Task<UpdateSessionViewModel> GetupdateSessionViewToAsync(int sessionId, CancellationToken cancellationToken);

        Task<Result> updateSessionAsync(int sessioni,UpdateSessionViewModel model,CancellationToken cancellationToken);

        public Task<SessionViewModel> GetSessionById(int sessionId, CancellationToken ct);
        Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct);

    }
}
