using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using moore.Data;
using moore.Models;
using System;
using System.Data;

namespace LanguageFeatures
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.Configure<MvcOptions>(options => options.EnableEndpointRouting = false);

        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env is null)
            {
                throw new ArgumentNullException(nameof(env));
            }

            app.UseMvcWithDefaultRoute();
            app.UseAuthorization();

        }
  
    }
}