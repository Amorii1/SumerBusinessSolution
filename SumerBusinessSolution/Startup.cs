using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using SumerBusinessSolution.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SumerBusinessSolution.Transactions;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using SumerBusinessSolution.Services;
using System.Reflection;
using SumerBusinessSolution.Resources;
using SumerBusinessSolution.RouteModelConventions;
using Microsoft.AspNetCore.Localization.Routing;
using SumerBusinessSolution.Hubs;

namespace SumerBusinessSolution
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

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                //   .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                
                var supportedCultures = new[]
                 {
                    new CultureInfo("ar") {
                     DateTimeFormat =
                        {
                            ShortDatePattern = "dd/MM/yyyy"
                        }
                    },
                    new CultureInfo("en"),
        
            };
                options.DefaultRequestCulture = new RequestCulture(culture: "ar", uiCulture: "ar");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider { Options = options });
            });
            services.AddSingleton<CommonLocalizationService>();
            services.AddMvc().AddViewLocalization();

            services.AddRazorPages(options => {
                options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
            });
 

            services.AddMvcCore().AddViewLocalization();
            services.AddTransient<IInventoryTrans, InventoryTrans>();
            services.AddTransient(typeof(IInventoryTrans), typeof(InventoryTrans));

            services.AddTransient<ICustomerTrans, CustomerTrans>();
            services.AddTransient(typeof(ICustomerTrans), typeof(CustomerTrans));

            services.AddTransient<ISalesTrans, SalesTrans>();
            services.AddTransient(typeof(ISalesTrans), typeof(SalesTrans));

            //services.AddTransient<IReqNote, ReqNote>();
            //services.AddTransient(typeof(IReqNote), typeof(ReqNote));
            services.AddScoped<IDbInitializar, DbInitializar>();

            services.AddHttpContextAccessor();

            services.AddMvc().AddViewLocalization().AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                {
                    var assemblyName = new AssemblyName(typeof(CommonResources).GetTypeInfo().Assembly.FullName);
                    return factory.Create(nameof(CommonResources), assemblyName.Name);
                };
            });

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Identity/Account/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IInventoryTrans InvTrans, ICustomerTrans CustTrans, ISalesTrans SalesTrans, IDbInitializar dbIni)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            dbIni.Initializar();
            var localizationOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value;
            app.UseRequestLocalization(localizationOptions);



            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });

            //check this 

          
        }
    }
}
