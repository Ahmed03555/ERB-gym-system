using AutoMapper;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.Services.ViewModel.MemberShipViewModels;
using G_NET_02_MVC01.BLL.ViewModels.TrainerViewModels;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Classes;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Class
{
    public class MemberShipServices : IMemberShipServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public MemberShipServices(IUnitOfWork _unitOfWork, IMapper _mapper)
        {
            this._unitOfWork = _unitOfWork;
            this._mapper = _mapper;
        }

        public async Task<Result> CreateMembershipAsync(CreateMembershipViewModel request, CancellationToken cancellationToken)
        {
            var member = await _unitOfWork
                .GetRepository<Member>()
                .GetById(request.MemberId, cancellationToken);

            if (member == null)
                return Result.Fail("Member does not exist.");

            var plan = await _unitOfWork
                .GetRepository<Plans>()
                .GetById(request.PlanId, cancellationToken);

            if (plan == null)
                return Result.Fail("Plan does not exist.");

            if (!plan.IsActive)
                return Result.Fail("Plan is inactive.");

            var hasActiveMembership = await _unitOfWork
                .GetRepository<Membership>()
                .AnyAsync(x =>
                    x.MemberId == request.MemberId &&
                    x.EndDate >= DateTime.Now,
                    cancellationToken);

            if (hasActiveMembership)
                return Result.Fail("Member already has an active membership.");

            // 5. Calculate dates
            var startDate = DateTime.Now;
            var endDate = startDate.AddDays(plan.DurationDays);

            var membership = new Membership
            {
                MemberId = request.MemberId,
                PlanId = request.PlanId,
                StartDate = startDate,
                EndDate = endDate
                
            };

            _unitOfWork.GetRepository<Membership>().Add(membership);

            await _unitOfWork.CompleteAsync();

            return Result.Ok();
        }
        public async Task<IEnumerable<MemberShipViewModels>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.GetRepository<Membership>().GetAll(false, ct);
            if (!memberships.Any()) return [];
            return _mapper.Map<IEnumerable<Membership>, IEnumerable<MemberShipViewModels>>(memberships);
        }

    }
} 

