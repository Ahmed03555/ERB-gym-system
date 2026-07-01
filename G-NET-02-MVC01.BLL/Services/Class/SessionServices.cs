using AutoMapper;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Class
{
    public class SessionServices : ISessionServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionServices(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreatSessionAsync(CreateSessionViewModel createSessionViewModel, CancellationToken cancellationToken = default)
        {
            if (createSessionViewModel.StartDate >= createSessionViewModel.EndDate) return Result.ValidationFailed("End date must be after start date");
            
            if (createSessionViewModel.StartDate < DateTime.Now)
            {
                return Result.ValidationFailed("Start Date Must Be in The Futher");
            }

            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();
            var CategoryRepo = _unitOfWork.GetRepository<Category>();
            var Trainer = await TrainerRepo.GetById(createSessionViewModel.TrainerId, cancellationToken);
            var Category = await CategoryRepo.GetById(createSessionViewModel.CategoryId, cancellationToken);
            if (Trainer == null || Category == null)
            {
                return Result.ValidationFailed("Trainer or Category not Founded"); ;
            }
            var session = _mapper.Map<CreateSessionViewModel, Session>(createSessionViewModel);
            var SessionRepo = _unitOfWork.GetRepository<Session>();
             SessionRepo.Add(session); 
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed to create session");
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetCategorySelectListAsync(CancellationToken cancellationToken = default)
        {
            var Categories = await _unitOfWork.GetRepository<Category>().GetAll(false, cancellationToken);
            return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);
        }

        public async Task<SessionViewModel> GetSessionById(int sessionId, CancellationToken ct)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetById(sessionId, ct);
            return _mapper.Map<Session, SessionViewModel>(session);
        }

        public async Task<SessionViewModel?> GetSessionByIdAsync(int sessionId, CancellationToken cancellationToken)
        {

           var Session = await _unitOfWork.sessionRepositorie.GetSessionWithTrainerAndCategoryByIdAsync(sessionId, cancellationToken);

            if (Session == null) return null;

            var mappedSession = _mapper.Map<Session, SessionViewModel>(Session);

            mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.sessionRepositorie.GetCountOfBookedSlotAsync(mappedSession.Id,cancellationToken);

            return mappedSession;
        }

        public async Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken cancellationToken = default)
        {
           var seessions = await _unitOfWork.sessionRepositorie.GetAllSessionsWithTrainerAndCategoryAsync(cancellationToken);

            if (!seessions.Any()) return null;

            seessions = seessions.OrderByDescending(s => s.StartDate);

            var SessionMaped = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(seessions);

            foreach (var session in SessionMaped)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.sessionRepositorie.GetCountOfBookedSlotAsync(session.Id, cancellationToken);
            }
            return SessionMaped;

        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetTrainerSelectListAsync(CancellationToken cancellationToken = default)
        {
           var trainers = await _unitOfWork.GetRepository<Trainer>().GetAll(false, cancellationToken);

              return _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerSelectViewModel>>(trainers);
        }

        public async Task<UpdateSessionViewModel> GetupdateSessionViewToAsync(int sessionId, CancellationToken cancellationToken)
        {
            var Session = await _unitOfWork.GetRepository<Session>().GetById(sessionId, cancellationToken);

            if (Session is null) return null;

            if (!await IsSessionValidForUpdate(Session, cancellationToken)) return null;

            return _mapper.Map<Session, UpdateSessionViewModel>(Session);


        }

        public async Task<Result> RemoveSessionAsync(int sessionId, CancellationToken ct)
        {
             var repo = _unitOfWork.GetRepository<Session>();

            var session = await repo.GetById(sessionId, ct);

            if (session is null) return Result.NotFound("session not found");

            if (session.EndDate <= session.StartDate) return Result.Fail("can not delete session that has not ended");

            var bookCount = await _unitOfWork.sessionRepositorie.GetCountOfBookedSlotAsync(sessionId,ct);

            if(bookCount >0)
            {

                return Result.Fail("can not delete session that has bookings");

            }
            repo.Delete(sessionId);

            var affectedRows = await _unitOfWork.CompleteAsync();

            return affectedRows> 0 ? Result.Ok() : Result.Fail("Failed to remove sessioon");


        }

        public async Task<Result> updateSessionAsync(int sessionid,UpdateSessionViewModel model ,CancellationToken cancellationToken)
        {
            var SessionRepo = _unitOfWork.GetRepository<Session>();

            var session = await SessionRepo.GetById(sessionid, cancellationToken);

            if (session is null) return Result.NotFound("Session is not founded");

            if (session.StartDate <= DateTime.Now) return Result.Fail("Can not Edit a session that has already started");

            var bookCount = await _unitOfWork.sessionRepositorie.GetCountOfBookedSlotAsync(sessionid,cancellationToken);

            if(bookCount > 0) {
                return Result.Fail("cannot Edit Asession That has been booked");
            }

            if (model.EndDate <= model.StartDate) return Result.ValidationFailed("End date must be afther start date");

            if (model.StartDate <= DateTime.Now) return Result.ValidationFailed("Start date must be in the futher");

            var TrainerRepo = _unitOfWork.GetRepository<Trainer>();

            var Trainer = await TrainerRepo.GetById(model.TrainerId, cancellationToken);

            if (Trainer is null) return Result.NotFound("Trainer is not found");

            session.UpdatedAt = DateTime.Now;

            _mapper.Map(model, session);

            SessionRepo.Update(session);

            var EffectedRow = await _unitOfWork.CompleteAsync();

            return EffectedRow > 0 ? Result.Ok() : Result.Fail("Failled to create Session");


        }

        private async Task<bool> IsSessionValidForUpdate(Session session,CancellationToken cancellationToken)
        {
            if (session.StartDate <= DateTime.Now) return false;
            var book = await _unitOfWork.sessionRepositorie.GetCountOfBookedSlotAsync(session.Id, cancellationToken);
            return book ==0; 
        }
    }
    }

