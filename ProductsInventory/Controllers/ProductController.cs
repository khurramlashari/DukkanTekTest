using DukkanTek.Domain.DTOs;
using DukkanTek.Services.Product;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.AccessTokenValidation;

namespace ProductsInventory.Controllers
{
    /// <summary>
    /// Product controller
    /// </summary>
    /// 
    [ApiController]
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="productService"></param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Returns count of instock,damaged and sold products.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("count")]
        public async Task<ProductCountDTO> ProductCount()
        {
            return await _productService.ProductCount();
        }

        /// <summary>
        /// post an order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("order")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> Post([FromBody] OrderRequest order)
        {
            var result = await _productService.SellProduct(order);
            if (result.Item1)
            {
                return Ok(new { result.Item2 });
            }
            return BadRequest(new { result.Item2 });
        }

        /// <summary>
        /// modify product status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productStatusRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Put(int id, [FromBody] ProductStatusRequest productStatusRequest)
        {
            var result = await _productService.ProductStatusUpdate(id, productStatusRequest.Id);
            if (result.Item1)
            {
                return Ok(new { result.Item2 });
            }
            return BadRequest(new { result.Item2 });
        }


    }
}
