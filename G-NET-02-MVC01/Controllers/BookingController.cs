using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.BLL.Services.ViewModel.BookingViewModels;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace G_NET_02_MVC01.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingServices _bookingServices ;
        private readonly ISessionServices sessionServices;

        public BookingController(IBookingServices _bookingServices, ISessionServices sessionServices    )
        {
            this._bookingServices = _bookingServices;
            this.sessionServices=sessionServices;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            var Booking = await sessionServices.GetSessionsAsync(cancellationToken);
            return View(Booking);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int sessionId, CancellationToken cancellationToken = default)
        {
            var members = await _bookingServices.GetMmeberForDropdownBySessionId(sessionId, cancellationToken);

            var model = new CreateBookingViewModel
            {
                SessionId = sessionId
            };

            ViewBag.Members = new SelectList(members, "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
            {
                var members = await _bookingServices.GetMmeberForDropdownBySessionId(model.SessionId, cancellationToken);
                ViewBag.Members = new SelectList(members, "Id", "Name");
                return View(model);
            }

            var result = await _bookingServices.CreateBookingAsync(model, cancellationToken);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);

                var members = await _bookingServices.GetMmeberForDropdownBySessionId(model.SessionId, cancellationToken);
                ViewBag.Members = new SelectList(members, "Id", "Name");

                return View(model);
            }

            return RedirectToAction("Index", new { sessionId = model.SessionId });
        }

      
        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSession(int sessionId, CancellationToken cancellationToken = default)
        {
            var members = await _bookingServices.GetMmeberForOngoingBySessionId(sessionId, cancellationToken);
            return View(members);
        }

        public async Task<IActionResult> GetMembersForUpcomingSession(int sessionId, CancellationToken cancellationToken = default)
        {
            var members = await _bookingServices.GetMmeberForUpcomingBySessionId(sessionId, cancellationToken);
            return View(members);
        }

        [HttpPost]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken cancellationToken = default)
        {
            var result = await _bookingServices.MarkAsAttendedAsync(memberId, sessionId, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Member marked as attended successfully.";
            }

            return RedirectToAction("GetMembersForOngoingSession", new { sessionId = sessionId });
        }

        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken cancellationToken = default)
        {
            var result = await _bookingServices.CancelBookingAsync(memberId, sessionId, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["SuccessMessage"] = "Booking cancelled successfully.";
            }

            return RedirectToAction("GetMembersForOngoingSession", new { sessionId = sessionId });
        }

    }
}
