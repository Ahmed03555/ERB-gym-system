using G_NET_02_MVC01.BLL.Services.Interface;
using G_NET_02_MVC01.DAL.Context;
using G_NET_02_MVC01.DAL.Entities;
using G_NET_02_MVC01.DAL.Repositories.Classes;
using G_NET_02_MVC01.DAL.Repositories.Interface;
using G_NET_02_MVC01.BLL.Services.Class;
using Microsoft.EntityFrameworkCore;
using G_NET_02_MVC01.BLL.Utilites;

namespace G_NET_02_MVC01
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            //builder.Services.AddScoped<IPlanRepositorie,PlanRepositorie>();
            //builder.Services.AddScoped(typeof(IGenericRepositorie<>), typeof(GenericRepositorie<>));
            builder.Services.AddScoped<IMemberServices, MemberServices>();
            builder.Services.AddScoped<ISessionServices, SessionServices>();
            builder.Services.AddScoped<ITrainerServices, TrainerServices>();
            builder.Services.AddScoped<IPlanServices, PlanServices>();
            builder.Services.AddScoped< IMemberShipServices, MemberShipServices>();

            builder.Services.AddScoped<IGenericRepositorie<Plans>, GenericRepositorie<Plans>>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();  
            builder.Services.AddAutoMapper(m => m.AddProfile<MapipingProfile>());
            builder.Services.AddScoped<IBookingRepositorie, BookingRepositorie>();
            builder.Services.AddScoped<IBookingServices, BookingServices>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
