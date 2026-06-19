using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Entities
{
    public class Booking :BaseEntity
    {
 
        public DateTime BookingDate { get; set; }

        public bool IsAttended { get; set; } = false;
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
    }
}
