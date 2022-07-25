using CarbonaraWebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class ServiceStarter : IHostedService
    {

        public IServiceProvider Services { get;}
        public ServiceStarter(IServiceProvider services)
        {
            Services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            using (var scope = Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<AppDbContext>();
                context.Database.Migrate();
                if (!context.Employee.Any())
                {
                    DataSeeder.InitializeDb(context);
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
