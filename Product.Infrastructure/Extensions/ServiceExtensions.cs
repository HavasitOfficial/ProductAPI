using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddProductInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddProductRepositories();
        }

        private static void AddProductRepositories(this IServiceCollection services) =>
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(publicOnly: true)
                .AsMatchingInterface((service, filter) =>
                    filter.Where(implementation =>
                        implementation.Name.Equals($"I{service.Name}", StringComparison.OrdinalIgnoreCase)
                        && implementation.Name.EndsWith("Repository"))));
    }
}
