using G_NET_02_MVC01.BLL.Services.Class;
using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.ViewModels.MembersViewModels;
using Microsoft.AspNetCore.Mvc;
namespace G_NET_02_MVC01.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberServices _memberServices;
        public MemberController(IMemberServices memberServices)
        {
            _memberServices = memberServices;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberServices.GetAllMembersAsync(ct);
            return View(members);
        }
        [HttpGet]
        public IActionResult Create(CancellationToken ct)
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel CreateMember, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                return View(nameof(Create), CreateMember);
            }
            var result = await _memberServices.CreateMemberAsync(CreateMember, ct);
            if (result)

                TempData["SuccessMessage"] = "Member created successfully.";


            else

                TempData["ErrorMessage"] = "Failed to create member.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MemberDetails(int memberId, CancellationToken ct)
        {
            var memberDetails = await _memberServices.GetMemberDetailsAsync(memberId, ct);
            if (memberDetails is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(memberDetails);
        }

        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        { 
            var healthRecord = await _memberServices.GetHealthRecordAsync(id, ct);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health record not found.";
                return RedirectToAction(nameof(Index));
            } return View(healthRecord);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int memberId, CancellationToken ct)
        {
            var memberToUpdate = await _memberServices.GetMemberToUpdateViewAsync(memberId, ct);
            if (memberToUpdate is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(memberToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(MemberToUpdateViewModel memberToUpdateViewModel, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(memberToUpdateViewModel);

            var result = await _memberServices.UpdateMemberAsync(memberToUpdateViewModel.Id, memberToUpdateViewModel, ct);
            if (result)
                TempData["SuccessMessage"] = "Member updated successfully.";
            else
                TempData["ErrorMessage"] = "Failed to update member.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]

        public async Task<IActionResult> Delete(int memberId, CancellationToken ct)
        {
            var member = await _memberServices.GetMemberDetailsAsync(memberId, ct);

            if (member == null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
            
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int Id, CancellationToken ct)
        {
            var Result = await _memberServices.RemoveUpdateAsync(Id, ct);
            TempData[Result.IsSuccess ? "SuccessMessage" : "ErrorMessage"] = Result.IsSuccess
                ? "Member Deleted Successfully!"
                : Result.ErrorMessage; return RedirectToAction(nameof(Index));
        }
    }
}



