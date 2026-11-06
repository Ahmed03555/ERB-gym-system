using System;
using System.Collections.Generic;
using System.Text;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.DAL;
using G_NET_02_MVC01.DAL.Context;
using Microsoft.EntityFrameworkCore;
namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class PlanRepositorie :GenericRepositorie<Plans>, IPlanRepositorie
    {
        private readonly GymContext gymContext;
        public PlanRepositorie(GymContext _gymContext) : base(_gymContext)
        {
            gymContext = _gymContext;
        }
    }
}
