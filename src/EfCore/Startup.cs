using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Domain.Entities;
using Domain.Entities.Base;
using Domain.Repositories;
using FclEx.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EfCore
{
    public class Startup<TDbContext> where TDbContext : DbContext
    {
        private static bool _configureServicesExcuted = false;
        private static bool _configureExcuted = false;

        public static void ConfigureServices(IServiceCollection services)
        {
            if (_configureServicesExcuted) return;
            AddDbServices(services);
            _configureServicesExcuted = true;
        }

        private static void AddDbServices(IServiceCollection services)
        {
            var entityType = typeof(IEntity<>);
            var entityTypes = entityType.GetTypeInfo().Assembly.GetTypes()
                .Where(m => m.IsInheritedFromGenericType(entityType) && !m.GetTypeInfo().IsGenericType && !m.GetTypeInfo().IsAbstract).ToArray();

            foreach (var type in entityTypes)
            {
                var interfaceType = type.GetGenericInterface(entityType);
                var keyType = interfaceType.GetTypeInfo().GetGenericArguments()[0];
                var repoInterfaceType = typeof(IRepository<,>).MakeGenericType(type, keyType);
                var repoType = typeof(EfCoreRepositoryBase<,>).MakeGenericType(typeof(TDbContext), type);
                services.AddTransient(repoInterfaceType, repoType);
                if (keyType == typeof(int))
                {
                    var otherRepoInterfaceType = typeof(IRepository<>).MakeGenericType(type);
                    services.AddTransient(otherRepoInterfaceType, repoType);
                }
            }
        }

        public static void Configure(IServiceProvider provider)
        {
            if (_configureExcuted) return;
            // When you use AddScope you shouldn't dispose the context yourself, 
            // because the next object that resolves your repository will have an disposed context.
            var context = provider.GetService<TDbContext>();
            if (context.Database.EnsureCreated()) context.Database.Migrate();

            _configureExcuted = true;
        }
    }
}
