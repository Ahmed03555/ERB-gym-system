using AutoMapper;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.Services.ViewModel.BookingViewModels;
using G_NET_02_MVC01.BLL.Services.ViewModel.MembersViewModels;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.Class
{
    public class BookingServices : IBookingServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingServices(IUnitOfWork _unitOfWork , IMapper _mapper)
        {
            this._unitOfWork = _unitOfWork;
            this._mapper = _mapper;
        }

        public async Task<Result> CancelBookingAsync(int memberId, int sessionId, CancellationToken cancellationToken = default)
        {
            var session = await _unitOfWork.sessionRepositorie.GetById(sessionId,cancellationToken);
            if(session == null)
            {
                return Result.NotFound("Session not found");
            }
            if(session.StartDate < DateTime.Now)
            {
                return Result.Fail("Cannot cancel booking for past sessions", ResultKind.Conflict);
            }
            var booking = await _unitOfWork.bookingRepositorie.FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId,true, cancellationToken);
            if(booking == null)
            {
                return Result.NotFound("Booking not found");
            }
            _unitOfWork.bookingRepositorie.Delete(booking.Id);
            var result =await _unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed to cancel booking", ResultKind.Conflict);

        }
        

        async Task<Result> IBookingServices.CreateBookingAsync(CreateBookingViewModel createBookingViewModel, CancellationToken cancellationToken)
        {
            var session = await _unitOfWork.sessionRepositorie.GetById(createBookingViewModel.SessionId, cancellationToken);
            if (session == null)
            {
                return Result.NotFound("The session was not found");
            }
            if (session.StartDate < DateTime.Now)
            {
                return Result.Fail("Cannot book a session that has already started", ResultKind.Conflict);
            }
            var member = await _unitOfWork.memberRepositorie.GetById(createBookingViewModel.MemberId, cancellationToken);
            if (member == null)
            {
                return Result.NotFound("The member was not found");
            }
            
            var BookingExists = await _unitOfWork.bookingRepositorie.AnyAsync(b => b.MemberId == createBookingViewModel.MemberId && b.SessionId == createBookingViewModel.SessionId, cancellationToken);
            if (BookingExists)
            {
                return Result.Fail("Member already booked this session.",ResultKind.Conflict);
            }

            var BookCount = await _unitOfWork.bookingRepositorie.CountAsync(b => b.SessionId == createBookingViewModel.SessionId, cancellationToken);
            if (BookCount >= session.Capacity)
            {
                return Result.Fail("Session is fully booked.", ResultKind.Conflict);
            }
            var booking = new Booking
            {
                MemberId = createBookingViewModel.MemberId,
                SessionId = createBookingViewModel.SessionId,
                IsAttended = false,
                CreatedAt = DateTime.Now
            };
             _unitOfWork.bookingRepositorie.Add(booking);
            var result = await _unitOfWork.CompleteAsync();

            return result > 0 ? Result.Ok() : Result.Fail("Failed to create booking", ResultKind.Conflict);
        }


        public async Task<IEnumerable<MemberSelectViewModel>> GetMmeberForDropdownBySessionId(int sessionId, CancellationToken cancellationToken = default)
        {
            var allMembers = await _unitOfWork.memberRepositorie.GetAll(false, cancellationToken);
            var allBookings = await _unitOfWork.bookingRepositorie.GetAll(false, cancellationToken);

            var bookedMemberIds = allBookings
                .Where(b => b.SessionId == sessionId)
                .Select(b => b.MemberId)
                .ToHashSet();

            var result = allMembers
                .Where(m => !bookedMemberIds.Contains(m.Id))
                .Select(m => new MemberSelectViewModel
                {
                    Id = m.Id,
                    SessionId = sessionId,
                    Name = m.Name
                });

            return result;
        }

        public async Task<IEnumerable<MemberForSession>> GetMmeberForOngoingBySessionId(int sessionId, CancellationToken cancellationToken = default)
        {
            var allBookings = await _unitOfWork.bookingRepositorie.GetAll(false, cancellationToken);
            var allMembers = await _unitOfWork.memberRepositorie.GetAll(false, cancellationToken);

            var result = allBookings
                .Where(b => b.SessionId == sessionId && b.IsAttended == true)
                .Join(allMembers,
                    b => b.MemberId,
                    m => m.Id,
                    (b, m) => new MemberForSession
                    {
                        MemberId = b.MemberId,
                        MemberName = m.Name,
                        SessionId = b.SessionId,
                        BookingDate = b.BookingDate.ToString("yyyy-MM-dd HH:mm"),
                        IsActived = b.IsAttended
                    });

            return result;
        }

        public async Task<IEnumerable<MemberForSession>> GetMmeberForUpcomingBySessionId(int sessionId, CancellationToken cancellationToken = default)
        {
            var allBookings = await _unitOfWork.bookingRepositorie.GetAll(false, cancellationToken);
            var allMembers = await _unitOfWork.memberRepositorie.GetAll(false, cancellationToken);

            var result = allBookings
                .Where(b => b.SessionId == sessionId && b.IsAttended == false)
                .Join(allMembers,
                    b => b.MemberId,
                    m => m.Id,
                    (b, m) => new MemberForSession
                    {
                        MemberId = b.MemberId,
                        MemberName = m.Name,
                        SessionId = b.SessionId,
                        BookingDate = b.BookingDate.ToString("yyyy-MM-dd HH:mm"),
                        IsActived = b.IsAttended
                    });

            return result;
        }
        public async Task<Result> MarkAsAttendedAsync(int memberId, int sessionId, CancellationToken cancellationToken = default)
        {
            var booking =await _unitOfWork.bookingRepositorie.FirstOrDefaultAsync(b => b.MemberId == memberId && b.SessionId == sessionId, true, cancellationToken);
            if(booking == null)
            {
                return Result.NotFound("Booking not found");
            }
            booking.IsAttended= true;
            booking.UpdatedAt = DateTime.Now;
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Result.Ok() : Result.Fail("Failed to Mark As AttendedAsync", ResultKind.Conflict);

        }
    }
}
