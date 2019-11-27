﻿using GeolocationAPI.Converters;
using GeolocationAPI.Converters.Interfaces;
using GeolocationAPI.Persistence;
using GeolocationAPI.Persistence.Repositories;
using GeolocationAPI.Persistence.Repositories.Interfaces;
using GeolocationAPI.Services;
using GeolocationAPI.Services.Interfaces;
using GeolocationAPI.Validators;
using GeolocationAPI.Validators.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace GeolocationAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddTransient<IGeolocationDataService, GeolocationDataService>();
            services.AddTransient<IGeolocationService, GeolocationService>();
            services.AddTransient<IGeolocationDataRepository, GeolocationDataRepository>();
            services.AddTransient<IGeolocationDataConverter, GeolocationDataConverter>();
            services.AddTransient<IIpAddressValidator, IpAddressValidator>();
            services.AddDbContext<ApplicationDbContext>(x =>
                x.UseSqlite(Configuration.GetConnectionString("GeolocationDataDb")));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GeolocationAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GeolocationAPI V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
