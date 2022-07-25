using CarbonaraWebAPI.Data;
using CarbonaraWebAPI.Infrastructure.Jwt;
using CarbonaraWebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarbonaraWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment{ get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            String connectionString;
            if (Environment.IsDevelopment())
                connectionString = Configuration.GetConnectionString("DevConnection");
            else
                connectionString = Configuration.GetConnectionString("DefaultConnection");
             
            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            services.AddSingleton(token);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = token.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(token.Secret)),
                    ValidAudience = token.Audience,
                    ValidateAudience = false,
                    RequireExpirationTime = true,
                    NameClaimType = "test"
                };
            });

            services.AddTransient<BookingService>();
            services.AddTransient<ExternalPartnerService>();
            services.AddTransient<AuthService>();
            services.AddTransient<AuthService>();          
            services.AddTransient<CarclassDatabaseService>();
            services.AddTransient<CarDatabaseService>();
            services.AddTransient<CartypeDatabaseService>();
            services.AddTransient<PlanDatabaseService>();
            services.AddTransient<StationDatabaseService>();
            services.AddTransient<AdminService>();
            services.AddTransient<CarService>();

            services.AddHostedService<ServiceStarter>();

            services.AddControllers();

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void OnShutdown() //Wird bei Herunterfahren der Anwendung aufgerufen
        {

        }
    }
}