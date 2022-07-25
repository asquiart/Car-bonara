using CarbonaraWebAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarbonaraWebAPITest.Util
{
    public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
    {


        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<AppDbContext>));

                services.Remove(descriptor);

                string connectionString = $"Server=pma.asqui.xyz;User=carbonara;Password=bpx04sYqIlcVbJRC;Database=CarbonaraTest2";

                IServiceCollection serviceCollection = services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
                //options.UseInMemoryDatabase(Guid.NewGuid().ToString()));


                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dbContext = scopedServices.GetRequiredService<AppDbContext>();
                  
                    try
                    {
                        DataSeeder.InitializeDb(dbContext);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString(), "An error occurred seeding the " +
                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}
