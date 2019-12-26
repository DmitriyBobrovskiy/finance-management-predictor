using System;
using System.IO;
using finance_management_backend.Infrastructure;
using finance_management_backend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace finance_management_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = ReadSettings();
            Console.WriteLine(settings.ConnectionString);
            services.AddSingleton(settings);
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql(settings.ConnectionString);
            });
            // TODO: setup cors in a right way
            services.AddCors();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            // TODO: setup cors in a right way
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static Settings ReadSettings()
        {
            Console.WriteLine("Reading `.env` file and environment variables");
            try
            {
                DotNetEnv.Env.Load();
            }
            catch (FileNotFoundException exception)
            {
                Console.WriteLine($"Cannot find configuration file  '{exception.FileName}'");
                Console.WriteLine("Will use only environment variables");
            }
            return new Settings();
        }
    }
}
