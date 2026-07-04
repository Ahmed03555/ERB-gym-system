using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.ViewModel.MembersViewModels
{
    public class MemberSelectViewModel
    {
        public int Id { get; set; }

        public int SessionId { get; set; }
        public string Name { get; set; } = default!;
    }
}
