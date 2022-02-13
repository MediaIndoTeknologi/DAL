using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WonderKid.DAL;
using WonderKid.DAL.Interface;
using WonderKid.Data;

namespace WonderKid.Sample
{
    public class Startup
    {
		public Startup(IConfiguration configuration)
		{
			System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }


		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(Configuration.GetConnectionString("NorthwindDatabase")));

			services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Wonderkid Sample Api", Version = "v1" });
				c.TagActionsBy(api =>
				{
					var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
					if (controllerActionDescriptor != null)
					{
						return new[] { $"WonderKid{controllerActionDescriptor.ControllerName}" };
					}

					throw new InvalidOperationException("Unable to determine tag for endpoint.");
				});
				c.DocInclusionPredicate((name, api) => true);
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Wonderkid Service v1"));

			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
