using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.ViewModels.PlanViewModels;
using G_NET_02_MVC01.BLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Interface
{
    public interface IPlanServices
    {
        
        Task<Result> UpdatePlanAsync(int Id, UpdatePlanViewModel model, CancellationToken ct = default);
        Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default);
    }
}
