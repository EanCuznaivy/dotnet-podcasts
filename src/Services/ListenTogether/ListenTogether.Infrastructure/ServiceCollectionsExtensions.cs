using ListenTogether.Application.Interfaces;
using ListenTogether.Infrastructure.Data;
using ListenTogether.Infrastructure.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ListenTogether.Infrastructure
{
    public static class ServiceCollectionsExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ListenTogetherDbContext>(options =>
            {
                options.UseMySql(
                    configuration.GetConnectionString("ListenTogetherDb"),
                    new MySqlServerVersion(new Version(8, 0, 28)),
                    b => b.MigrationsAssembly("ListenTogether.Hub")
                );
            });
            serviceCollection.AddScoped<IApplicationDbContext, ListenTogetherDbContext>();
            serviceCollection.AddHttpClient<IEpisodesClient, EpisodesHttpClient>(opt =>
            {
                opt.BaseAddress = new Uri(configuration["NetPodcastApi:BaseAddress"]);
            });

            return serviceCollection;
        }
    }
}
