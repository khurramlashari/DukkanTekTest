using DukkanTek.Domain.Entities;
using DukkanTek.Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) :base(context)         
        {

        }
    }
}
