using AutoMapper;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.MembersViewModels;
using G_NET_02_MVC01.BLL.ViewModels.TrainerViewModels;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Classes;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Class
{
    public class TrainerServices : ITrainerServices
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public TrainerServices(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork= unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetTrainerAsync(CancellationToken cancellationToken = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetAll(false,cancellationToken);
            if (!trainer.Any()) return [];
            return mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(trainer);
        }

        public async Task<Result<TrainerViewModel>>GetTrainerDetailsAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>() .GetById(trainerId, ct);

            if (trainer == null)
                return Result<TrainerViewModel>.NotFound("The trainer was not found.");

            var trainerViewModel = mapper.Map<Trainer,TrainerViewModel>(trainer);

            return Result<TrainerViewModel>.Ok(trainerViewModel);
        }

        public async Task<Result> CreateTrainerAsync(CreateTrainerViewModel createTrainerViewModel, CancellationToken cancellationToken = default)
        {
            var mailExist = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Email == createTrainerViewModel.Email, cancellationToken);
            var phoneExist = await unitOfWork.GetRepository<Trainer>().AnyAsync(m => m.Phone == createTrainerViewModel.Phone, cancellationToken);
            if (mailExist || phoneExist)
            {
                return Result.Fail("The Trainer already exist");
            }
                if (createTrainerViewModel == null)
            {
                return Result.NotFound("The trainer is not found");
            }
            var trainer = mapper.Map<CreateTrainerViewModel, Trainer>(createTrainerViewModel);
            var TrainerRepo =  unitOfWork.GetRepository<Trainer>();
            TrainerRepo.Add(trainer);
            var result = await unitOfWork.CompleteAsync();
            return result >0 ? Result.Ok() : Result.Fail("The operation is Fail");


        }



        private async Task<bool> IsTrainerValidForUpdate(Trainer trainer, CancellationToken cancellationToken)
        {
            
            var book = await unitOfWork.sessionRepositorie.GetCountOfBookedSlotAsync(trainer.Id, cancellationToken);
            return book ==0;
        }





        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int trainerId, CancellationToken ct = default)
        {
            var trainer = await unitOfWork.GetRepository<Trainer>().GetById(trainerId, ct);
            if (trainer is null) return null;
            var trainerViewModel = mapper.Map<TrainerToUpdateViewModel>(trainer);
            return trainerViewModel;
        }
        public async Task<Result> UpdateTrainerAsync(int trainerId, TrainerToUpdateViewModel model, CancellationToken ct = default)
        {
            var trainerRepo = unitOfWork.GetRepository<Trainer>();
            if (model is null) return Result.NotFound("Trainer Is Not Found");
            var emailExists = await trainerRepo.AnyAsync(t => t.Email == model.Email && t.Id != trainerId, ct);
            var phoneExists = await trainerRepo.AnyAsync(t => t.Phone == model.Phone && t.Id != trainerId, ct);

            if (phoneExists)
                return Result.ValidationFailed("This Phone Is Already Linked To A Trainer");
            if (emailExists)
                return Result.ValidationFailed("This Email Is Already Linked To A Trainer");

            var trainer = await trainerRepo.GetById(trainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer Is Not Found");
            trainer.Name = model.Name;
            trainer.Email = model.Email;
            trainer.Phone = model.Phone;
            trainer.Address.BuildingNumber = model.BuildingNumber;
            trainer.Address.Street = model.Street;
            trainer.Address.City = model.City;
            trainer.UpdatedAt = DateTime.Now;
            trainerRepo.Update(trainer);
            var result = await unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Trainer");
        }

        public async Task<Result> RemoveUpdateAsync(int trainerId, CancellationToken ct)
        {
            var TrainerRepo = unitOfWork.GetRepository<Trainer>();
            var trainer = await TrainerRepo.GetById(trainerId, ct);
            if (trainer is null) return Result.NotFound("Trainer Is Not Found");
            TrainerRepo.Delete(trainerId);

            var affectedRows = await unitOfWork.CompleteAsync();

            return affectedRows> 0 ? Result.Ok() : Result.Fail("Failed to remove sessioon");
        }
    }
}


