using Domain.Interfaces;
using System;

namespace Infrastructure
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return _serviceProvider.GetService<IUnitOfWork>();
        }
    }

    public static class ServiceProviderServiceExtensions
    {
        public static T GetService<T>(this IServiceProvider provider)
        {
            return (T)provider.GetService(typeof(T));
        }


    }
}
