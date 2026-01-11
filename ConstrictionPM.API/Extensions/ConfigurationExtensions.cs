using ConstructionPM.Infrastructure.Configurations;

namespace ConstructionPM.API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddAppConfigurations(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Email settings
            services.Configure<EmailSettings>(
                configuration.GetSection("EmailSettings")
            );

            return services;
        }
    }
}
