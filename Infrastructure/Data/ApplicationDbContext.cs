using DukkanTek.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
       
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        #region DbSets
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ProductStatus> ProductStatuses { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
             
        }
        
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {             
            return base.SaveChangesAsync(cancellationToken);
        }
         
    }
}
