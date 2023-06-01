using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Stadis.Intelligence.Web.External.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stadis.Intelligence.Service;
using Stadis.Intelligence.Service.Interface;
using Stadis.Intelligence.Service.Class;
using Stadis.Intelligence.Data.Repositories.Interface;
using Stadis.Intelligence.Data.Repositories.Class;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Stadis.Intelligence.Web.SDK;
using Reveal.Sdk;
using System.IO;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
//using Reveal.Sdk.AspNetCore;
using Stadis.Intelligence.Web.External.Logger;

namespace Stadis.Intelligence.Web
{
	public class Startup
	{
		private string _webRootPath;
		public IHttpContextAccessor _httpContextAccessor { get; set; }
		//public ILoggerManager _Logger { get; set; }

		public Startup(IConfiguration configuration, IWebHostEnvironment env)
		{
			Configuration = configuration;
			_webRootPath = env.WebRootPath;

			//_httpContextAccessor = httpContextAccessor;

		}

		public IConfiguration Configuration { get; }


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews().AddNewtonsoftJson();
			services.AddControllersWithViews();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			
			services.AddScoped<ICompanyDataSourceRepository, CompanyDataSourceRepository>();
			

			
			services.AddScoped<ICompanyDataSourceService, CompanyDataSourceService>();
			
			
			services.AddHttpContextAccessor();
			services.AddDistributedMemoryCache(); // Adds a default in-memory implementation of IDistributedCache
			services.AddSession();

			//JWT token authentication
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					//ValidIssuer = Configuration["Jwt:Issuer"],
					//ValidAudience = Configuration["Jwt:Issuer"],
					//IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
				};

			});
			//reveal configration....
			var embedSettings = new RevealEmbedSettings();
			embedSettings.LocalFileStoragePath = GetLocalFileStoragePath(_webRootPath);

			//You could configure the default disk locations used by RevealView to store cached data by uncommenting the following lines:
			var cacheFilePath = Configuration.GetSection("Caching")?["CacheFilePath"] ?? @"C:\Temp\RevealV1.5\Web\Cache";
			Directory.CreateDirectory(cacheFilePath);
			embedSettings.DataCachePath = cacheFilePath;
			embedSettings.CachePath = cacheFilePath;

			services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);
			services.AddResponseCompression(options =>
			{
				options.Providers.Add<GzipCompressionProvider>();
				options.EnableForHttps = false;
			});
			services.AddSingleton<IRVUserContextProvider, SampleUserContextProvider>();
			//services.AddRevealServices(embedSettings, CreateSdkContext());
			services.AddMvc()
					   .AddReveal(builder =>
					   {
						   builder
							  .AddDashboardProvider<MyDashboardProvider>()
							  .AddAuthenticationProvider<EmbedAuthenticationProvider>()
							  .AddSettings(s =>
							  {
								  s.CachePath = s.DataCachePath = cacheFilePath;
								  s.LocalFileStoragePath = GetLocalFileStoragePath(_webRootPath);
								  //s.MaxInMemoryCells = 50 * 1000 * 1000; //50M cells
								  s.MaxStorageCells = 50 * 1000 * 1000;
							  });
					   });

			services.AddMvc().AddRazorPagesOptions(options =>
			{
				options.Conventions.AddPageRoute("/Dashboards/DashboardCreate234", "");
			});
			//services.AddMvc();
		}
		/*protected virtual RevealSdkContextBase CreateSdkContext()
		{
			return new RevealSdkContext(Configuration, _httpContextAccessor);
		}*/
		protected virtual string GetLocalFileStoragePath(string webRootPath)
		{
			return Path.Combine(webRootPath, "App_Data", "RVLocalFiles");
		}
		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			_httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
			//_Logger = app.ApplicationServices.GetRequiredService<ILoggerManager>();
			//CreateSdkContext();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseAuthentication();

				//app.UseMvc();
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

			app.UseAuthorization();
			app.UseSession();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Dashboards}/{action=DashboardCreate}/{id?}");
			});
		}
	}
}
