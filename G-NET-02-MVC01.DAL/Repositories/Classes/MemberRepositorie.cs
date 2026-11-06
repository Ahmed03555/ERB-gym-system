using System;
using System.Collections.Generic;
using System.Text;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.DAL;
using G_NET_02_MVC01.DAL.Context;
namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class MemberRepositorie :GenericRepositorie<Member>, IMemberRepositorie
    {
       private readonly GymContext gymContext;
        public MemberRepositorie(GymContext _gymContext) : base(_gymContext)
        {
            gymContext = _gymContext;
        }
    }
}
