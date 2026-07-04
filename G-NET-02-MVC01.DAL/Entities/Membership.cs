using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace G_NET_02_MVC01.DAL.Entities
{
    public class Membership : BaseEntity
    {
        public Member Member { get; set; } = null!;
        public int MemberId { get; set; }

        public Plans Plan { get; set; } = null!;
        public int PlanId { get; set; }

        public DateTime StartDate { get; set; }   
        public DateTime EndDate { get; set; }

        [NotMapped]
        public string status => EndDate >= DateTime.Now ? "Active" : "Expired";

        [NotMapped]
        public bool isActive => EndDate >= DateTime.Now;
    }
}
