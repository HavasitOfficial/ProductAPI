using Microsoft.AspNetCore.Mvc;
using Product.Application.Dtos.Requests;
using Product.Application.Dtos.Response;
using Product.Application.Services.Interfaces;
using Product.Domain.Enums;

namespace Product.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> GetProducts(
            [FromQuery] string? name,
            [FromQuery] decimal? price,
            [FromQuery] string? description,
            [FromQuery] ProductType? type)
        {
            return await productService.GetProducts(new ProductFilterDto(name, price, description, type));
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> GetProduct(Guid id) => await productService.GetProduct(id);

        [HttpPost]
        public async Task<ProductDto> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            return await productService.CreateProduct(productCreateDto);
        }

        [HttpPut("{id}")]
        public async Task<ProductDto> UpdateProduct(Guid id, [FromBody] ProductModifyDto productModifyDto) => await productService.UpdateProduct(id, productModifyDto);

        [HttpDelete("{id}")]
        public async Task DeleteProduct(Guid id) => await productService.DeleteProduct(id);
    }
}
