using System;
using GuestiaCodingTask.Data;
using GuestiaCodingTask.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GuestiaCodingTask.Services
{
    public static class ServiceContainer
    {
        public static IServiceProvider ConfigureServices()
        {
            var service = new ServiceCollection();
            
            // register DbContext
            service.AddDbContext<GuestiaContext>();
            
            // register services
            service.AddTransient<IGuestRepository, GuestRepository>();
            service.AddTransient<IGuestService, GuestService>();
            service.AddTransient<IAdoGuestRepository, AdoGuestRepository>();
            
            return service.BuildServiceProvider();
        }
    }
}