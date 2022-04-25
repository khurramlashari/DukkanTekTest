using Domain.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using DukkanTek.Domain.DTOs;
using DukkanTek.Domain.Shared;
using System;

namespace DukkanTek.Services.Product
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ProductCountDTO> ProductCount()
        {
            var unit = UnitOfWork.BaseRepositoryAsync<Domain.Entities.Product>();
            return new ProductCountDTO()
            {
                InStock = await unit.CountAsync(x => x.ProductStatusId == (int)ProductStatusEnum.InStock),
                Sold = await unit.CountAsync(x => x.ProductStatusId == (int)ProductStatusEnum.Damged),
                Damaged = await unit.CountAsync(x => x.ProductStatusId == (int)ProductStatusEnum.Sold)
            };
        }

        public async Task<(bool,string)> ProductStatusUpdate(int productId,int productStatusId)
        {
            var productObj = await UnitOfWork.BaseRepositoryAsync<Domain.Entities.Product>().FindAsync(x => x.Id == productId,true);
            if (productObj == null)
            {
                return (false,"No such product exists");
            }
            productObj.ProductStatusId = productStatusId;
            UnitOfWork.BaseRepositoryAsync<Domain.Entities.Product>().UpdateAsync(productObj);
            await UnitOfWork.SaveChangesAsync();
            return (true, "Product updated sucessfully");
        }

        public async Task<(bool,string)> SellProduct(OrderRequest order)
        {
            var productUnit =  UnitOfWork.BaseRepositoryAsync<Domain.Entities.Product>();
            var product =    await productUnit.FirstOrDefaultAsync(x => x.Id == order.ProductId);
            if (product  is null)
            {
                return (false,"No such product exists");
            }
            if (product.Stock - order.Quantity < 0)
            {
                return (false,"system is unable to place your order because of insuffient stock");
            }
            var unit = UnitOfWork.BaseRepositoryAsync<Domain.Entities.Order>();

            await unit.AddAsync(new Domain.Entities.Order()
            {
                CustomerName = order.CustomerName,
                Price = product.Price,
                Quantity = order.Quantity,
                OrderDate = System.DateTime.Now,
                ProductId = order.ProductId
            });
            product.Stock -= order.Quantity;
            if (product.Stock  == 0)
            {
                product.ProductStatusId = (int)ProductStatusEnum.Sold;
            }
            await UnitOfWork.SaveChangesAsync();
            return (true, "Order placed successfully");
        }
    }
}
