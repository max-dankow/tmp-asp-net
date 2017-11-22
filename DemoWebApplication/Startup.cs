﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DemoWebApplication
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
            // Add framework services.
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });
            services.AddMvc()
                .AddViewLocalization(options => options.ResourcesPath = "Resources")
                .AddDataAnnotationsLocalization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
//          Solution using default UseRequestLocalization
//            var supportedCultures = new[]
//            {
//                new CultureInfo("en-US"),
//                new CultureInfo("en"),
//                new CultureInfo("ru"),
//                new CultureInfo("fr"),
//            };
//
//            var requestCultureProviders = new List<IRequestCultureProvider>
//            {
//                new QueryStringRequestCultureProvider
//                {
//                    QueryStringKey = "lang",
//                    UIQueryStringKey = "lang"
//                }
//            };

//            app.UseRequestLocalization(new RequestLocalizationOptions
//            {
//                DefaultRequestCulture = new RequestCulture("ru"),
//                // Formatting numbers, dates, etc.
//                SupportedCultures = supportedCultures,
//                // UI strings that we have localized.
//                SupportedUICultures = supportedCultures,
//                RequestCultureProviders = requestCultureProviders
//                
//            });

            app.UseMiddleware<CultureMiddleware>("lang");
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
