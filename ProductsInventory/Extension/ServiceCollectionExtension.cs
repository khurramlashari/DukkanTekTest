using AutoMapper.Configuration;
using Domain.Interfaces;
using DukkanTek.Domain.Interfaces;
using DukkanTek.Services.Product;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsInventory.Extension
{
    /// <summary>
    /// ServiceCollectionExtensions
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// AddRepositories
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepository<>), typeof(BaseRepository<>))
                .AddScoped<IProductRepository, ProductRepository>();
        }
        /// <summary>
        /// AddUnitOfWork
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services
                .AddScoped<IUnitOfWork, UnitOfWork>();
        }
        /// <summary>
        /// AddBusinessServices
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBusinessServices(this IServiceCollection services
           )
        {
            return services
                .AddScoped<IProductService, ProductService>();
        }
    }
}
