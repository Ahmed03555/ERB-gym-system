using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.ViewModel.MemberShipViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Interface
{
    public interface IMemberShipServices
    {
        Task<IEnumerable<MemberShipViewModels>> GetAllMembershipsAsync(CancellationToken ct = default);

        Task<Result> CreateMembershipAsync(CreateMembershipViewModel request, CancellationToken cancellationToken);
        
        
}
}
