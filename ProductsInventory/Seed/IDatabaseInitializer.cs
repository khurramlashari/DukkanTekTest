using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ProductsInventory.Seed
{
    /// <summary>
    /// IDatabaseInitializer
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// SeedAsync
        /// </summary>
        /// <returns></returns>
        Task SeedAsync();
    }
    /// <summary>
    /// Database initializer and seeding class 
    /// </summary>
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> _userManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// DatabaseInitializer
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger, Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager, Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// SeedAsync
        /// </summary>
        /// <returns></returns>
        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);

            if (!await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");
                                  
                await EnsureRoleAsync("Admin" );                 
                await CreateUserAsync("khurramlashari", "abAB@12345", "khurramlashari.com", "+1 (123) 000-0000",  "Admin"  );

                _logger.LogInformation("Inbuilt account generation completed");


            }
            if (!await _context.ProductStatuses.AnyAsync())
            {
               await _context.ProductStatuses.AddAsync(new DukkanTek.Domain.Entities.ProductStatus()
               {
               Name = "InStock"
               });
                await _context.ProductStatuses.AddAsync(new DukkanTek.Domain.Entities.ProductStatus()
                {
                    Name = "Sold"
                });
                await _context.ProductStatuses.AddAsync(new DukkanTek.Domain.Entities.ProductStatus()
                {
                    Name = "Damaged"
                });
                await _context.SaveChangesAsync();  
            }
            if (!await _context.Categories.AnyAsync())
            {
                await _context.Categories.AddAsync(new DukkanTek.Domain.Entities.Category()
                {
                    Name = "Baby"
                });
                await _context.Categories.AddAsync(new DukkanTek.Domain.Entities.Category()
                {
                    Name = "Men"
                });
                await _context.Categories.AddAsync(new DukkanTek.Domain.Entities.Category()
                {
                    Name = "Women"
                });
                await _context.Categories.AddAsync(new DukkanTek.Domain.Entities.Category()
                {
                    Name = "Fashion"
                });
                await _context.SaveChangesAsync();
            }
            if (!await _context.Products.AnyAsync())
            {
                await _context.Products.AddAsync(new DukkanTek.Domain.Entities.Product()
                {
                    Name = "Baby Soap",
                    Description = "Baby Soap for infants",
                    Barcode = "232323",
                    ProductCategoryId = 1,
                    ProductStatusId = 1,
                    Weight = 2.5m
                });
                await _context.Products.AddAsync(new DukkanTek.Domain.Entities.Product()
                {
                    Name = "Men Soap",
                    Description = "Soap for males",
                    Barcode = "232324",
                    ProductCategoryId = 2,
                    ProductStatusId = 2,
                    Weight = 2.5m
                });
                await _context.Products.AddAsync(new DukkanTek.Domain.Entities.Product()
                {
                    Name = "Women Soap",
                    Description = "Soap for females",
                    Barcode = "232323",
                    ProductCategoryId = 3,
                    ProductStatusId = 3,
                    Weight = 2.5m
                });
                await _context.SaveChangesAsync();
            }

        }
         


        private async Task EnsureRoleAsync(string roleName )
        {
            if ((await _context.Roles.FindAsync(roleName)) == null)
            {
                var applicationRole = new IdentityRole(roleName);

                var result = await _roleManager.CreateAsync(applicationRole);

                if (!result.Succeeded)
                    throw new Exception($"Seeding \"{roleName}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            }
        }

        private async Task<IdentityUser> CreateUserAsync(string userName, string password,   string email, string phoneNumber, string role)
        {
            var applicationUser = new IdentityUser
            {
                UserName = userName,
                AccessFailedCount = 0,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true,                

            };

            var result = await _userManager.CreateAsync(applicationUser ,password);

            if (!result.Succeeded)
                throw new Exception($"Seeding \"{userName}\" user failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");
            else
            {
               result = await _userManager.AddToRoleAsync(applicationUser, role);
                if (!result.Succeeded)
                {
                    throw new Exception($"Seeding \"{userName}\" user and \"{role}\" role failed. Errors: {string.Join(Environment.NewLine, result.Errors)}");

                }
            }

            return applicationUser;
        }
    }
}
