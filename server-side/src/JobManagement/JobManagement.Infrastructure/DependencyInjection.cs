using JobManagement.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using JobManagement.Domain.Repositories;

namespace JobManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped(typeof(IJobRepository), typeof(JobRepository));
            services.AddScoped(typeof(IProfessionRepository), typeof(ProfessionRepository));

            return services;
        }
    }
}
