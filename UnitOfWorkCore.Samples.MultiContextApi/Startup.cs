using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using UnitOfWorkCore.AspNetCore;
using UnitOfWorkCore.AspNetCore.Filters;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.IssuesDb;
using UnitOfWorkCore.Samples.MultiContextApi.DataAccess.ReleasesDb;
using UnitOfWorkCore.Samples.MultiContextApi.Services;

namespace UnitOfWorkCore.Samples.MultiContextApi
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
            services.AddDbContext<ReleasesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ReleasesDb")));
            services.AddDbContext<IssuesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IssuesDb")));

            services.AddUnitOfWorkPool(optionsBuilder =>
            {
                optionsBuilder.AddUnitOfWork<ReleasesDbContext>(ReleasesUoW.KEY);
                optionsBuilder.AddUnitOfWork<IssuesDbContext>(IssuesUow.KEY);
            });

            services.AddScoped<IReleasesUoW, ReleasesUoW>();
            services.AddScoped<IIssuesUoW, IssuesUow>();

            services.AddScoped<IIssuesService, IssuesService>();
            services.AddScoped<IReleasesService, ReleasesService>();

            services.AddAutoMapper();

            // Add framework services.
            services.AddMvcCore(options =>
            {
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));
                options.Filters.Add(typeof(UnitOfWorkPoolTransactionFilter));
            })
            .AddApiExplorer()
            .AddFormatterMappings()
            .AddJsonFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Multi Context Api", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "UnitOfWorkCore.Samples.MultiContextApi.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentName}/swagger.json";
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Single Context Api v1");
                c.ShowRequestHeaders();
                c.ShowJsonEditor();
            });
        }
    }
}
