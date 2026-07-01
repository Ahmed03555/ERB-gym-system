using AutoMapper;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.PlanViewModels;
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
    public class PlanServices : IPlanServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public PlanServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        //public async Task<Result<PlanViewModel>> GetPlanDetailsAsync(int planId, CancellationToken ct = default)
        //{
        //    var plan = await unitOfWork.GetRepository<Plans>().GetById(planId, ct);

        //    if (plan == null)
        //        return Result<PlanViewModel>.NotFound("The plan was not found.");

        //    var PlanViewModel = mapper.Map<Plans, PlanViewModel>(plan);

        //    return Result<PlanViewModel>.Ok(PlanViewModel);
        //}


        public async Task<UpdatePlanViewModel?> GetPlanToUpdateAsync(int planId, CancellationToken ct = default)
        {
            var plan = await unitOfWork.GetRepository<Plans>().GetById(planId, ct);
            if (plan is null) return null;
            return mapper.Map<UpdatePlanViewModel>(plan);
        }

        public async Task<Result> UpdatePlanAsync(int Id, UpdatePlanViewModel model, CancellationToken ct = default)
        {
            if (model is null) return Result.NotFound("Plan Is Not Found");

            var planRepo = unitOfWork.GetRepository<Plans>();

            var plan = await planRepo.GetById(Id, ct);
            if (plan is null) return Result.NotFound("Plan Is Not Found");

            // تحقق إن مفيش plan تانية بنفس الاسم
            var nameExists = await planRepo.AnyAsync(p => p.Name == model.PlanName && p.Id != Id, ct);
            if (nameExists)
                return Result.ValidationFailed("This Plan Name Already Exists");

            plan.Name = model.PlanName;
            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;
            plan.UpdatedAt = DateTime.Now;

            planRepo.Update(plan);

            var result = await unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Plan");
        }
    }
}
