using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.DAL.Repositories.Classes
{
    public class TrainerRepositorie : GenericRepositorie<Trainer> ,ITrainerRepositorie
    {
        private readonly GymContext _gymContext;
        public TrainerRepositorie(GymContext _gymContext) : base(_gymContext)
        {
            this._gymContext = _gymContext;
        }
    }
}
