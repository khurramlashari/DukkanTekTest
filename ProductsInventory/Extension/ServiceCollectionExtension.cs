using AutoMapper.Configuration;
using Domain.Interfaces;
using DukkanTek.Domain.Interfaces;
using DukkanTek.Services.Product;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsInventory.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(BaseRepository<>))
                .AddScoped<IProductRepository, ProductRepository>();
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static IServiceCollection AddBusinessServices(this IServiceCollection services
           )
        {
            return services
                .AddScoped<IProductService, ProductService>();
        }
    }
}
