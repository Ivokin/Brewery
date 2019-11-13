using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Brewery.Web.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Brewery.Infrastructure;
using SimpleInjector;
using Microsoft.Extensions.DependencyInjection;
using Brewery.BusinessLogic;
using Brewery.BusinessLogic.Service;

namespace Brewery.Web
{
    public class Startup
    {
        private Container container;

        private string connectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            container = new Container();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddMvc();
            services.AddSimpleInjector(container, options =>
            {
                options.AddAspNetCore()
                    .AddControllerActivation()
                    .AddViewComponentActivation()
                    .AddTagHelperActivation();
            });
            services.EnableSimpleInjectorCrossWiring(container);
            BusinessLogicIoCConfig businessLogicIoCConfig = new BusinessLogicIoCConfig(container, connectionString, services);
            businessLogicIoCConfig.RegisterDependencies();

            services.AddSingleton<IMapper, Mapper>();
            services.AddDbContext<ApplicationDbContext>(options => options.
            UseSqlServer(connectionString));

            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
            })
                .AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddClaimsPrincipalFactory<AppClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            DbSeed.Seed(app.ApplicationServices);
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseStatusCodePages();
            container.AutoCrossWireAspNetComponents(app);
            container.RegisterPageModels(app);
            container.Verify();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
