using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GeographyRepository;
using WebApi.Geography;
using VisitsRepository;

namespace WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IGeographyRepository>((s) =>
            {
                return GeographyRepositoryFactory.GetInstance();
            });
            services.AddTransient<IVisitsRepository>((s) =>
            {
                return VisitsRepositoryFactory.GetInstance();
            });

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            VisitsRepositoryFactory.ConnectionString = Configuration.GetSection("Visits").GetValue<string>("ConnectionString");

            //((FileGeographyRepository.FileGeographyRepository)app.ApplicationServices.GetService<IGeographyRepository>()).CitiesCsvFilePath = @"C:\Users\micha\Documents\GitHub\RestApiProjectSpivey\data\City.csv";
            //((FileGeographyRepository.FileGeographyRepository)app.ApplicationServices.GetService<IGeographyRepository>()).StatesCsvFilePath = @"C:\Users\micha\Documents\GitHub\RestApiProjectSpivey\data\State.csv";

            app.UseMvc();
        }
    }
}
