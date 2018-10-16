using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectFlight.Data;
using System;
using System.Data.SqlClient;

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
	        ApplicationDbContext.ConnectionString = Configuration.GetConnectionString("DefaultConnection");
	        services.AddDbContext<ApplicationDbContext>();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

			// Refreshes the flight list
			// TODO: Use the data we have for now
			/*
	        Task.Run(() =>
	        {
				Console.WriteLine("Updating flight list...");

		        string response;
		        using (var client = new WebClient())
			        response = client.DownloadString("https://public-api.adsbexchange.com/VirtualRadar/AircraftList.json");

		        dynamic json = JsonConvert.DeserializeObject(response);
		        var flights = json.acList.ToObject<FlightInfoResponse[]>();

		        var infos = new List<FlightInfo>();
		        foreach (var flight in flights)
		        {
			        var info = new FlightInfo(flight);
					infos.Add(info);
				}
				
		        using (var context = new ApplicationDbContext())
		        {
			        foreach (var info in infos)
					{
						// Database type time can't store values higher than 24 hours
						if (info.Tracked.TotalHours >= 24)
							info.Tracked = TimeSpan.Parse("23:59:59.9999999");

						context.FlightInfos.Add(info);
					}

					context.SaveChanges();
				}

		        Console.WriteLine($"Saved {infos.Count} flights");
	        });
			*/
        }
    }
}