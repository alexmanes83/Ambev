using Ambev.DeveloperEvaluation.Application.Services;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Ambev.DeveloperEvaluation.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string mongoConnectionString)
        {
            var mongoClient = new MongoClient(mongoConnectionString);
            var database = mongoClient.GetDatabase("DeveloperStore");

            services.AddSingleton<IMongoDatabase>(database);
            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ISaleService, SaleService>();

            return services;
        }
    }
} 