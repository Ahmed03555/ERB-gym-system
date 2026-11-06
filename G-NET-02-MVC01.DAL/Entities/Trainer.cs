using G_NET_02_MVC01.DAL.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace G_NET_02_MVC01.DAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialization Specialization { get; set; }
        public DateTime HireDate { get; set; }

        public ICollection<Session> Sessions { get; set; }
        = new HashSet<Session>();
    }
}
