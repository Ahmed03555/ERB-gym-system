using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using G_NET_02_MVC01.BLL.ViewModels.MembersViewModels;
namespace G_NET_02_MVC01.BLL.Services.Interface
{
    public interface IMemberServices
    {
        Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct=default);
        Task<MemberViewModel> GetMemberDetailsAsync(int memberId, CancellationToken ct=default);

        Task<MemberToUpdateViewModel> GetMemberToUpdateViewAsync(int memberId, CancellationToken ct = default);
        Task<HealthRecordViewModel> GetHealthRecordAsync(int memberId, CancellationToken ct = default);


        Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken ct = default);

        Task<bool> UpdateMemberAsync(int id,MemberToUpdateViewModel memberToUpdateViewModel, CancellationToken ct = default);

        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default);
    }
}
