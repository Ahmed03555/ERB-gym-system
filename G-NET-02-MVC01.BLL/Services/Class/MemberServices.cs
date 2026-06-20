using System;
using System.Collections.Generic;
using System.Text;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.MembersViewModels;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.DAL.Entities;
namespace G_NET_02_MVC01.BLL.Services.Class
{
    public class MemberServices : IMemberServices
    {
        private readonly IGenericRepositorie<Member> _memberRepositorie;
        private readonly IGenericRepositorie<Membership> _membershipRepositorie;
        private readonly IGenericRepositorie<Plans> _planRepositorie;
        private readonly IGenericRepositorie<HealthRecord> _healthRecordRepositorie;
        private readonly IGenericRepositorie<Booking> _bookingRepositorie;
        public MemberServices(IGenericRepositorie<Member> _memberRepositorie, IGenericRepositorie<Membership> _membershipRepositorie, IGenericRepositorie<Plans> _planRepositorie, IGenericRepositorie<HealthRecord> _healthRecordRepositorie, IGenericRepositorie<Booking> _bookingRepositorie) {
            this._memberRepositorie = _memberRepositorie;
            this._membershipRepositorie = _membershipRepositorie;
            this._planRepositorie = _planRepositorie;
            this._healthRecordRepositorie = _healthRecordRepositorie;
            this._bookingRepositorie = _bookingRepositorie;
        }
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel createMemberViewModel, CancellationToken ct = default)
        {
            var mailExist = await _memberRepositorie.AnyAsync(m => m.Email == createMemberViewModel.Email, ct);
            var phoneExist = await _memberRepositorie.AnyAsync(m => m.Phone == createMemberViewModel.Phone, ct);
            
            if(mailExist || phoneExist)
            {
                return false;
            }
            var member = new Member()
            {
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
             _memberRepositorie.Add(member);
            var result = await _memberRepositorie.CompleteAsync();
            return result > 0;
        }

        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct = default)
        {
            var memberFuture =await _bookingRepositorie.AnyAsync(b => b.MemberId == memberId && b.Session.EndDate > DateTime.Now, ct);
            if (memberFuture)
            {
                return false;
            }
            _memberRepositorie.Delete(memberId);
            var result = await _memberRepositorie.CompleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<MemberViewModel>> GetAllMembersAsync(CancellationToken ct = default)
        {
            var members = await _memberRepositorie.GetAll(false, ct);
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
           var healthRecord = await _healthRecordRepositorie.FirstOrDefaultAsync(hr => hr.MemberId == memberId, false, ct);
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
           var member = await _memberRepositorie.GetById(memberId, ct);
            if (member == null) return null;
            var MemberShipVm=  new MemberViewModel()
           {
              
               Name = member.Name,
               Email = member.Email,
               Phone = member.Phone,
               DateOfBirth = member.DateOfBirth.ToShortDateString(),
               Gender = member.Gender.ToString(),
               Address=$"{member.Address.BuildingNumber},{member.Address.State}, {member.Address.ZipCode}",
           };
           var ActiveMembership = await _membershipRepositorie.FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, false, ct);
            if (ActiveMembership != null)
            {
                var plan = await _planRepositorie.GetById(ActiveMembership.PlanId, ct);
                MemberShipVm.PlanName = plan.Name;
                MemberShipVm.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                MemberShipVm.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();
            }
            return MemberShipVm;
        }

        public async Task<MemberToUpdateViewModel> GetMemberToUpdateViewAsync(int memberId, CancellationToken ct = default)
        {
            var member = await _memberRepositorie.GetById(memberId, ct);
            if (member == null) return null;
            return new MemberToUpdateViewModel()
            {
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
            var member = await _memberRepositorie.GetById(id, ct);
            if (member == null) return false;
            if(await _memberRepositorie.AnyAsync(m => m.Email == memberToUpdateViewModel.Email && m.Id != id, ct)) return false;
            if (await _memberRepositorie.AnyAsync(m => m.Phone == memberToUpdateViewModel.Phone && m.Id != id, ct)) return false;
            member.Email = memberToUpdateViewModel.Email;
            member.Phone = memberToUpdateViewModel.Phone;
           member.Address.BuildingNumber = memberToUpdateViewModel.BuildingNumber;
            member.Address.City = memberToUpdateViewModel.City;
            member.Address.Street = memberToUpdateViewModel.Street;
            member.UpdatedAt = DateTime.Now;
            _memberRepositorie.Update(member);
            var result = await _memberRepositorie.CompleteAsync();
            return result > 0;

        }
    }
}
