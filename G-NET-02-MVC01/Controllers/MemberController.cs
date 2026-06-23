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
    }
}



