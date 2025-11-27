using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Globalization;

using RealEstateApp.Data.Models;
using RealEstateApp.Data.DataServices.Contracts;
using RealEstateApp.Data.Repository.Contracts;
using RealEstateApp.Data.Repository;
using RealEstateApp.Data.DataServices;

namespace RealEstateApp.Web.Infrastructure.Extensions
{
    public static class ServiceApplicationExtensions
    {
        private const string SERVICE_INTERFACE_PREFIX = "I";
        private const string SERVICE_SUFFIX = "Service";

        public static void RegisterRepositories(this IServiceCollection services, Assembly repositoryAssembly)
        {
            Type[] typesToExclude = new Type[] { typeof(ApplicationUser) };

            Type[] repositoryClasses = repositoryAssembly.GetTypes()
                .Where(x => !x.IsAbstract && !x.IsInterface && !x.Name.ToLower().EndsWith("attribute"))
                .ToArray();

            foreach (Type type in repositoryClasses)
            {
                if (typesToExclude.Contains(type) == false)
                {
                    Type repositoryInterface = typeof(IRepository<,>);
                    Type repositoryInstanceType = typeof(BaseRepository<,>);

                    PropertyInfo idPropertyInfo = type.GetProperties()
                        .Where(x => x.Name.ToLower() == "id")
                        .SingleOrDefault()!;

                    if (idPropertyInfo == null)
                    {
                        throw new InvalidOperationException($"Entity {type.Name} does not have an Id property. All entities must expose a surrogate Id for repository registration.");
                    }

                    Type[] constructArgs = { type, idPropertyInfo.PropertyType };

                    repositoryInterface = repositoryInterface.MakeGenericType(constructArgs);
                    repositoryInstanceType = repositoryInstanceType.MakeGenericType(constructArgs);

                    services.AddScoped(repositoryInterface, repositoryInstanceType);
                }
            }
        }

        public static IServiceCollection RegisterUserServices(this IServiceCollection serviceCollection, Assembly serviceAssembly)
        {
            Type[] serviceClasses = serviceAssembly.GetTypes()
                .Where(x => !x.IsInterface &&
                x.Name.EndsWith(SERVICE_SUFFIX))
                .ToArray();

            foreach (Type serviceClass in serviceClasses)
            {
                Type? serviceInterface = serviceClass.GetInterfaces().FirstOrDefault(x => x.Name == $"{SERVICE_INTERFACE_PREFIX}{serviceClass.Name}");

                if (serviceInterface == null)
                {
                    throw new InvalidOperationException($"Service {serviceClass.Name} does not have a matching interface.");
                }

                serviceCollection.AddScoped(serviceInterface, serviceClass);
            }

            return serviceCollection;
        }

        public static IApplicationBuilder SeedDefaultIdentity(this IApplicationBuilder appBuilder)
        {
            using IServiceScope scope = appBuilder.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            ISeedService seedServices = serviceProvider.GetRequiredService<ISeedService>();
            seedServices.SeedIdentityAsync().GetAwaiter().GetResult();
            return appBuilder;
        }

        public static IApplicationBuilder SeedDefaultProperties(this IApplicationBuilder appBuilder)
        {
            using IServiceScope scope = appBuilder.ApplicationServices.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;

            ISeedService seedServices = serviceProvider.GetRequiredService<ISeedService>();
            seedServices.SeedDefaultProperties().GetAwaiter().GetResult();
            return appBuilder;
        }
    }
}
