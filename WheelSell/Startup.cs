
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WheelSell.Models.AppDbContext;
using AutoMapper;
using WheelSell.MappingProfiles;

namespace WheelSell
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
            services.AddControllersWithViews();
            services.AddDbContext<WheelSellDbContext>(options => options.UseSqlServer(Configuration
                .GetConnectionString("Default")));

            //services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<WheelSellDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<WheelSellDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddRazorPages();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddCloudscribePagination();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Bike}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                //Making conventional route for practice.
                //endpoints.MapControllerRoute(
                //    name: "ByYearMonth",
                //    pattern: "{controller=Make}/{action=ByYearMonth}/{year:int:length(4)}/{month:int:range(1,12)}");

            });
        }
    }
}
