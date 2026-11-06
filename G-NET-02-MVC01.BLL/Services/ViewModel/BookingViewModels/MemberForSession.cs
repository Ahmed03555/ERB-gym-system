using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.ViewModel.BookingViewModels
{
    public class MemberForSession
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = default!;

        public int SessionId { get; set; }

        public String BookingDate { get; set; } = default!;

        public bool IsActived { get; set; } = false;

    }
}
