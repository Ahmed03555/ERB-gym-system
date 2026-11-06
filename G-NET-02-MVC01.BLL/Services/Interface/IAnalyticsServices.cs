using G_NET_02_MVC01.BLL.Services.ViewModel.AnalyticsViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Interface
{
    public interface IAnalyticsServices
    {
        Task<AnalyticsViewModel> GetAnalyticsAsync(CancellationToken cancellationToken = default);
    }
}
