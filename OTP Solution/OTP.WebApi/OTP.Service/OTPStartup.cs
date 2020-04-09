using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OTP.Service.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace OTP.Service
{
    public class OTPStartup : StartupBase
    {
        public OTPStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public override void Configure(IApplicationBuilder app)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.Configure<OTPGeneratorConfiguration>(options => Configuration.GetSection("OTPGenerator").Bind(options));
        }
    }
}
