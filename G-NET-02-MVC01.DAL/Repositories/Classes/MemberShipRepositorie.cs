using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class MemberShipRepositorie : GenericRepositorie<Membership>, IMemberShipRepositorie
    {
        private readonly GymContext _context;
        public MemberShipRepositorie(GymContext _context) : base(_context)
        {
            this._context = _context;
        }
    }
}
