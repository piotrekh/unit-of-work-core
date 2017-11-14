using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using UnitOfWorkCore.MultiContext;

namespace UnitOfWorkCore.AspNetCore
{
    public static class IServiceCollectionExtensions
    {
        public static void AddUnitOfWork<T>(this IServiceCollection services) where T : DbContext
        {
            services.AddScoped<IUnitOfWork<T>, UnitOfWork<T>>();
            services.AddScoped<IUnitOfWork>(provider => provider.GetService<IUnitOfWork<T>>());
        }

        public static void AddUnitOfWorkPool(this IServiceCollection services, Action<UnitOfWorkPoolOptionsBuilder> optionsAction)
        {            
            var optionsBuilder = new UnitOfWorkPoolOptionsBuilder();
            optionsAction.Invoke(optionsBuilder);

            services.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            services.AddSingleton(typeof(UnitOfWorkPoolOptions), optionsBuilder.Options);
            services.AddScoped<IUnitOfWorkPool, UnitOfWorkPool>();
        }
    }
}
