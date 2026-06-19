using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
