
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProductsInventory.Core
{
    public static class RegisterService
    {
        public static IServiceCollection _services;
        public static IConfiguration _configuration;

        public static void AddCustomServices(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            _configuration = configuration;

            //Scoped services
            #region Scoped services
           
             
            #endregion

            // Singleton services
            #region Singleton services

            _services.AddSingleton(_configuration);
            #endregion
        }
    }
}
