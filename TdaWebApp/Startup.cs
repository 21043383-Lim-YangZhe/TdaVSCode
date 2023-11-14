using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using TdaWebApp.Models;
using TdaWebApp.Services;
using TdaWebApp.Settings;
using MongoDB.Driver;

namespace TdaWebApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //
            services.AddSingleton<IMongoClient>(sp => new MongoClient("mongodb://localhost:27017"));
            services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("mydb");
            });
            //

            // Add IConfiguration to the container
           services.AddSingleton<IConfiguration>(Configuration);

           services.AddScoped<BeersService>();
            

            //
            var mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
            services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>(
        mongoDbSettings.ConnectionString, mongoDbSettings.Name
    ).AddDefaultTokenProviders();

            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();


            services.AddControllersWithViews();
            foreach (var service in services)
            {
                Console.WriteLine(service.ServiceType);
            }
        }


        
        }
    }
