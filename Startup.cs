using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectFlight.Data;
using ProjectFlight.Managers;
using System;
using System.Linq;

namespace ProjectFlight
{
	public class Startup
    {
        public Startup(IConfiguration configuration) 
	        => Configuration = configuration;

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

			// Add ApplicationDbContext to dependency injection (or in english: setup SQL)
	        services.AddDbContext<ApplicationDbContext>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Add session/cookies
	        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
			// Check database
			using (var context = new ApplicationDbContext())
				context.Database.EnsureCreated();

	        if (env.IsDevelopment())
		        app.UseDeveloperExceptionPage();
	        else
	        {
		        app.UseExceptionHandler("/Home/Error");
		        app.UseHsts();
			}

			//app.UseHttpsRedirection();
	        app.UseStaticFiles();
	        app.UseCookiePolicy();

	        app.UseAuthentication();

			app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

			// Setup WebSockets for notification delivery
	        app.UseWebSockets();
	        app.Use(async (context, next) => await WebSocketDeliverer.Handle(context, next));
	        
			// Start updating database
			var updater = new FlightInfoUpdater(TimeSpan.FromSeconds(5));

			// Subscribe to some testing events
	        // TODO: Instead of logging to console, use proper ASP.NET logger
	        updater.OnRefresh += updates =>
	        {
		        NotificationManager.Update(updates);
		        Console.WriteLine($"Refreshed {updates.Length} flight infos");
	        };

			// Create notification manager
			NotificationManager.Create();
        }
    }
}