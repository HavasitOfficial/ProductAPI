using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Product.Application.Validators.Products;

namespace Product.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddProductApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddServices();
            services.AddValidators();
        }

        private static void AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(publicOnly: true)
                .AsMatchingInterface((service, filter) =>
                    filter.Where(implementation =>
                        implementation.Name.Equals($"I{service.Name}", StringComparison.OrdinalIgnoreCase)))
                .WithScopedLifetime());
        }

        private static void AddValidators(this IServiceCollection services) =>
            services.AddValidatorsFromAssemblyContaining<ProductCreateValidator>();
    }
}
