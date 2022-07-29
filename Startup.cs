using System;
using System.Collections.Generic;
using System.Linq;
using DevTrust_Task.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DevTrust_Task.Services;

namespace DevTrust_Task
{
    public class Startup
    {
        static public long ADDRESS_LAST_ID = 0, PERSON_LAST_ID = 0;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            services.AddDbContext<DevTrustContext>(options =>
                options.UseNpgsql("User ID=pvntynwfvnquov;Password=9b58a2a4a7f6dc0ef76c7de87737a90d55eaee9072b6b1396011f7ff60aa683f;Host=ec2-54-77-40-202.eu-west-1.compute.amazonaws.com;Port=5432;Database=d3j2qeqcon7842;Pooling=true;SSL Mode=Require;TrustServerCertificate=True;"));

            services.AddScoped<IPersonRepo, PersonRepo>();
            services.AddScoped<ISerializer, Serializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
