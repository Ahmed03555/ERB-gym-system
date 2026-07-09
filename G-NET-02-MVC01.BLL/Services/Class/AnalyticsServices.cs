using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.Services.ViewModel.AnalyticsViewModel;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Class
{
    public class AnalyticsServices : IAnalyticsServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AnalyticsViewModel> GetAnalyticsAsync(CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetAll(false);
            var totalMembers = await _unitOfWork.GetRepository<Member>().CountAsync(cancellationToken: cancellationToken);
            var TotalTrainers = await _unitOfWork.GetRepository<Trainer>().CountAsync(cancellationToken: cancellationToken);
            var activeMembers = await _unitOfWork.GetRepository<Membership>().CountAsync(m => m.EndDate > DateTime.UtcNow, cancellationToken: cancellationToken);
            return new AnalyticsViewModel
            {
                TotalMembers = totalMembers,
                ActiveMembers = activeMembers,
                TotalTrainers = TotalTrainers,
                UpcomingSessions = session.Count(s => s.StartDate > DateTime.UtcNow),
                OngoingSessions = session.Count(s => s.StartDate <= DateTime.UtcNow && s.EndDate >= DateTime.UtcNow),
                CompletedSessions = session.Count(s => s.EndDate < DateTime.UtcNow)
            };
        }
    }
}
