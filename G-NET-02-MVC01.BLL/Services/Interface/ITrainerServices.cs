using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.ViewModels.MembersViewModels;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using G_NET_02_MVC01.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Interface
{
    public interface ITrainerServices
    {
        public Task<IEnumerable<TrainerViewModel>> GetTrainerAsync(CancellationToken cancellationToken = default);
        Task<Result<TrainerViewModel>> GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default);

        Task<Result> CreateTrainerAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default);
        public Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken cancellationToken);

        Task<Result> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken cancellationToken);

        
        Task<Result> RemoveUpdateAsync(int trainerId, CancellationToken ct);
    }
}
