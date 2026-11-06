using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.ViewModel.MemberShipViewModels
{
    public class CreateMembershipViewModel
    {
        [Required(ErrorMessage = "Please select a member.")]
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Please select a plan.")]
        public int PlanId { get; set; }
    }
}
