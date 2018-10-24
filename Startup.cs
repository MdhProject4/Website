using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectFlight.Data;
using System;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.Cookies;

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
	        try
	        {
		        using (var context = new ApplicationDbContext())
			        context.Database.EnsureCreated();
	        }
	        catch (SqlException e)
	        {
				Console.WriteLine($"SQL Error: {e.Message}");
	        }

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
	        
			// Start updating database
			// TODO: Instead of logging to console, use proper ASP.NET logger
			var updater = new FlightInfoUpdater(TimeSpan.FromSeconds(5));

			// Subscribe to some testing events
	        updater.OnAdd     += amount => Console.WriteLine($"Added {amount} flight infos");
	        updater.OnRefresh += amount => Console.WriteLine($"Refreshed {amount} flight infos");
        }
    }
}