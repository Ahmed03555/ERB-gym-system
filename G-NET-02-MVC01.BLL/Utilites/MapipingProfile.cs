using AutoMapper;
using G_NET_02_MVC01.BLL.Services.ViewModel.MemberShipViewModels;
using G_NET_02_MVC01.BLL.ViewModels.MembersViewModels;
using G_NET_02_MVC01.BLL.ViewModels.PlanViewModels;
using G_NET_02_MVC01.BLL.ViewModels.SessionViewModels;
using G_NET_02_MVC01.BLL.ViewModels.TrainerViewModels;
using G_NET_02_MVC01.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Utilites
{
    public class MapipingProfile : Profile
    {
        public MapipingProfile()
        {
            Configure();
        }
        public void Configure()
        {
            // Add your mapping configurations here
            CreateMap<Session, SessionViewModel>().ForMember(destinationMember => destinationMember.CategoryName, options => options.MapFrom(source => source.Category.categoryName))
                .ForMember(destinationMember => destinationMember.TrainerName, options => options.MapFrom(source => source.Trainer.Name))
                .ForMember(destinationMember => destinationMember.AvailableSlots, options => options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap<Trainer, TrainerViewModel>()
    .ForMember(
        dest => dest.Address,
        opt => opt.MapFrom(src =>
            $"{src.Address.Street}, {src.Address.City}, {src.Address.State}")
    );
            CreateMap<SessionViewModel, Session>();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>();
            CreateMap<Session,UpdateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerToUpdateViewModel>();


            CreateMap<Membership, MemberShipViewModels>()
            .ForMember(dest => dest.MembershipId,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MemberName,
                opt => opt.MapFrom(src => src.Member.Name))
            .ForMember(dest => dest.PlanName,
                opt => opt.MapFrom(src => src.Plan.Name));




            CreateMap<Plans, UpdatePlanViewModel>()
    .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address,
                    opt => opt.MapFrom(src => new GymUserAddress
                    {
                        BuildingNumber = src.BuildingNumber,
                        Street = src.Street,
                        City = src.City
                    })
                );
            CreateMap<Trainer, TrainerToUpdateViewModel>()
    .ForMember(d => d.BuildingNumber,
        o => o.MapFrom(s => s.Address.BuildingNumber))
    .ForMember(d => d.City,
        o => o.MapFrom(s => s.Address.City))
    .ForMember(d => d.Street,
        o => o.MapFrom(s => s.Address.Street))
    .ReverseMap()
    .ForMember(d => d.Address,
        o => o.MapFrom(s => new GymUserAddress
        {
            BuildingNumber = s.BuildingNumber,
            City = s.City,
            Street = s.Street
        }));
        
        }
    }
}
