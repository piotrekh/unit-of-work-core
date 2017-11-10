using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using UnitOfWorkCore.AspNetCore;
using UnitOfWorkCore.AspNetCore.Filters;
using UnitOfWorkCore.Samples.SingleContextApi.DataAccess;
using UnitOfWorkCore.Samples.SingleContextApi.Services;

namespace UnitOfWorkCore.Samples.SingleContextApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ReleasesDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ReleasesDb")));
            services.AddUnitOfWork<ReleasesDbContext>();

            services.AddAutoMapper();            

            // Add framework services.
            services.AddMvcCore(options =>
            {                
                options.Filters.Add(new ProducesAttribute("application/json"));
                options.Filters.Add(new ConsumesAttribute("application/json"));
                options.Filters.Add(typeof(UnitOfWorkTransactionFilter));
            })
            .AddApiExplorer()
            .AddFormatterMappings()
            .AddJsonFormatters();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Single Context Api", Version = "v1" });
                var filePath = Path.Combine(AppContext.BaseDirectory, "UnitOfWorkCore.Samples.SingleContextApi.xml");
                c.IncludeXmlComments(filePath);                
            });

            services.AddScoped<IReleasesService, ReleasesService>();
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
