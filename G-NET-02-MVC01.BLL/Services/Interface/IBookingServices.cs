using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.ViewModel.BookingViewModels;
using G_NET_02_MVC01.BLL.Services.ViewModel.MembersViewModels;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using G_NET_02_MVC01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Interface
{
    public interface IBookingServices
    {
        Task<IEnumerable<MemberForSession>> GetMmeberForUpcomingBySessionId(int sessionId,CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberForSession>> GetMmeberForOngoingBySessionId(int sessionId, CancellationToken cancellationToken = default);
        Task<IEnumerable<MemberSelectViewModel>> GetMmeberForDropdownBySessionId(int sessionId, CancellationToken cancellationToken = default);

        Task<Result> CreateBookingAsync(CreateBookingViewModel createBookingViewModel, CancellationToken cancellationToken = default);
        Task<Result> CancelBookingAsync(int memberId,int sessionId, CancellationToken cancellationToken = default);

        Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken cancellationToken = default);

    }
}
