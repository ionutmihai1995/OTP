using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using OTP.Repository.Repositories.Interfaces;
using OTP.Repository.Repositories.Implementations;
using OTP.Service.Interfaces;
using OTP.Service.Services;
using OTP.Repository.Context;
using OTP.Service;

namespace OTP.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            OTPStartup = new OTPStartup(Configuration);
        }

        public IConfiguration Configuration { get; }
        public OTPStartup OTPStartup { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped(typeof(IMemoryRepository<>), typeof(MemoryRepository<>));
            services.AddDbContext<OTPContext>(options => options.UseInMemoryDatabase(databaseName: "OTP"));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserSecretKeyService, UserSecretKeyService>();
            services.AddScoped<IGeneratedOTPService, GeneratedOTPService>();
            OTPStartup.ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
