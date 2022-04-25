using DukkanTek.Domain.DTOs;
using System.Threading.Tasks;

namespace DukkanTek.Services.Product
{
    public interface IProductService
    {
        Task<ProductCountDTO> ProductCount();
        Task<(bool, string)> ProductStatusUpdate(int productId, int productStatusId);
        Task<(bool, string)> SellProduct(OrderRequest order);
    }
}
