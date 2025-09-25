using APIServiceLibrary.Services;
using DataLogicLibrary.DirectionStrategies.Interfaces;
using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.Factories;
using DataLogicLibrary.Services.Interfaces;
using DataLogicLibrary.Services;
using DataLogicLibrary.Infrastructure.Enums;
using CarSimulator.Server.Factories;
using Microsoft.Extensions.DependencyInjection;

namespace CarSimulator.Server
{
    // ... (using-satser och namespace som tidigare)

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<ISimulationLogicService, SimulationLogicService>();
            builder.Services.AddScoped<IDirectionContext, DirectionContext>();
            builder.Services.AddScoped<IAPIService, APIService>();
            builder.Services.AddScoped<ICarFactory, CarFactory>();
            builder.Services.AddScoped<IDriverFactory, DriverFactory>();
            builder.Services.AddScoped<IStatusFactory, StatusFactory>();
            builder.Services.AddScoped<IStatusMessageService, StatusMessageService>();
            builder.Services.AddScoped<TurnLeftStrategy>();
            builder.Services.AddScoped<TurnRightStrategy>();
            builder.Services.AddScoped<DriveForwardStrategy>();
            builder.Services.AddScoped<ReverseStrategy>();

            builder.Services.AddScoped<SimulationLogicService.DirectionStrategyResolver>(serviceProvider => movementAction =>
            {
                switch (movementAction)
                {
                    case MovementAction.Left:
                        return serviceProvider.GetRequiredService<TurnLeftStrategy>();
                    case MovementAction.Right:
                        return serviceProvider.GetRequiredService<TurnRightStrategy>();
                    case MovementAction.Forward:
                        return serviceProvider.GetRequiredService<DriveForwardStrategy>();
                    case MovementAction.Backward:
                        return serviceProvider.GetRequiredService<ReverseStrategy>();
                    default:
                        throw new KeyNotFoundException($"Unknown movement action: {movementAction}");
                }
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=CarSimulator}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
