using Product.Application.Dtos.Requests;
using Product.Application.Dtos.Response;

namespace Product.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts(ProductFilterDto productFilterDto);
        Task<ProductDto> GetProduct(Guid id);
        Task<ProductDto> CreateProduct(ProductCreateDto productDto);
        Task<ProductDto> UpdateProduct(Guid id, ProductModifyDto productDto);
        Task DeleteProduct(Guid id);
    }
}
