using AutoMapper;
using G_NET_02_MVC01.BLL.Services.AttachmentServices;
using G_NET_02_MVC01.BLL.Services.Common;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.MembersViewModels;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;
namespace G_NET_02_MVC01.BLL.Services.Class
{
    public class MemberServices : IMemberServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentServices attachmentServices;
        private readonly IMapper mapper;
        public MemberServices(IUnitOfWork unitOfWork, IAttachmentServices attachmentServices, IMapper mapper) {
            this.unitOfWork=unitOfWork;
            this.attachmentServices=attachmentServices;
            this.mapper=mapper;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken ct = default)
        {
            var mailExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == createMemberViewModel.Email, ct);
            var phoneExist = await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == createMemberViewModel.Phone, ct);
            
            if(mailExist || phoneExist)
            {
                return false;
            }
            var photoPath = await attachmentServices.UpdateAsync(createMemberViewModel.PhotoFile.OpenReadStream(), createMemberViewModel.PhotoFile.FileName, "MemberPhotos", ct);

            if (string.IsNullOrEmpty(photoPath))
            {
                return false;
            }

            var member = new Member()
            {
                Photos = photoPath,
                Name = createMemberViewModel.Name,
                Email = createMemberViewModel.Email,
                Phone = createMemberViewModel.Phone,
                DateOfBirth = createMemberViewModel.DateOfBirth,
                Gender = createMemberViewModel.Gender,
                Address = new GymUserAddress()
                {
                    BuildingNumber = createMemberViewModel.BuildingNumber,
                    City = createMemberViewModel.City,
                    Street = createMemberViewModel.Street
                },
                HealthRecord = new HealthRecord()
                {
                    Height = createMemberViewModel.HealthRecordViewModel.Height,
                    Weight = createMemberViewModel.HealthRecordViewModel.Weight,
                    BloodType = createMemberViewModel.HealthRecordViewModel.BloodType,
                    Note = createMemberViewModel.HealthRecordViewModel.Note
                }

            };
             unitOfWork.GetRepository<Member>().Add(member);
            var result = await unitOfWork.CompleteAsync();
            if(result > 0)
            {
                return true;
            }
            else
            {
                 attachmentServices.Delete( member.Photos, "MemberPhotos");
                return false;
            }
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var memberFuture =await unitOfWork.GetRepository<Booking>().AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, ct);
            if (memberFuture)
            {
                return false;
            }
            unitOfWork.GetRepository<Member>().Delete(memberId);
            var result = await unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await unitOfWork.GetRepository<Member>().GetAll(false, ct);
            if (!members.Any()) return [];
            var membersViewModel = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Photo = m.Photos,
                Gender =m.Gender.ToString(),

            });
            return membersViewModel;

        }

        public async Task<HealthRecordViewModel> GetHealthRecordAsync(int memberId, CancellationToken ct = default)
        {
           var healthRecord = await unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(hr => hr.MemberId == memberId, false, ct);
            if (healthRecord == null) return null;
            return new HealthRecordViewModel()
            {
                Height = healthRecord.Height,
                Weight = healthRecord.Weight,
                BloodType = healthRecord.BloodType,
                Note = healthRecord.Note
            };


        }

        public async Task<MemberViewModel> GetMemberDetailsAsync(int memberId, CancellationToken ct = default)
        {
           var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member == null) return null;
            var MemberShipVm=  new MemberViewModel()
           {

                Id = member.Id,
                Name = member.Name,
               Email = member.Email,
               Phone = member.Phone,
               DateOfBirth = member.DateOfBirth.ToShortDateString(),
               Gender = member.Gender.ToString(),
               Address=$"{member.Address.BuildingNumber},{member.Address.State}, {member.Address.ZipCode}",
           };
           var ActiveMembership = await unitOfWork.GetRepository<Membership>().FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, false, ct);
            if (ActiveMembership != null)
            {
                var plan = await unitOfWork.GetRepository<Plans>().GetById(ActiveMembership.PlanId, ct);
                MemberShipVm.PlanName = plan.Name;
                MemberShipVm.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                MemberShipVm.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
            }
            return MemberShipVm;
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateViewAsync(int memberId, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(memberId, ct);
            if (member == null) return null;
            return new MemberToUpdateViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                City = member.Address.City,
                Street = member.Address.Street,
                Photo = member.Photos,
            };
           
        }

        public async Task<bool> UpdateMemberAsync(int id, MemberToUpdateViewModel memberToUpdateViewModel, CancellationToken ct = default)
        {
            var member = await unitOfWork.GetRepository<Member>().GetById(id, ct);
            if (member == null) return false;
            if(await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Email == memberToUpdateViewModel.Email && m.Id != id, ct)) return false;
            if (await unitOfWork.GetRepository<Member>().AnyAsync(m => m.Phone == memberToUpdateViewModel.Phone && m.Id != id, ct)) return false;
            member.Email = memberToUpdateViewModel.Email;
            member.Phone = memberToUpdateViewModel.Phone;
           member.Address.BuildingNumber = memberToUpdateViewModel.BuildingNumber;
            member.Address.City = memberToUpdateViewModel.City;
            member.Address.Street = memberToUpdateViewModel.Street;
            member.UpdatedAt = DateTime.Now;
            unitOfWork.GetRepository<Member>().Update(member);
            var result = await unitOfWork.CompleteAsync();
            return result > 0;

        }
        public async Task<Result> RemoveUpdateAsync(int id, CancellationToken ct)
        {
            var MemberRepo = unitOfWork.GetRepository<Member>();
            var member = await MemberRepo.GetById(id, ct);
            if (member is null) return Result.NotFound("Member Is Not Found");
            MemberRepo.Delete(id);

            var affectedRows = await unitOfWork.CompleteAsync();

            if(affectedRows > 0)
            {
                if (!string.IsNullOrEmpty(member.Photos))
                
                    attachmentServices.Delete(member.Photos, "MemberPhotos");
                
                return Result.Ok();
            }
            return Result.Fail("Failed to remove member");
        }
    }
}
