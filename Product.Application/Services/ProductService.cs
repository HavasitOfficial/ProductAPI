using FluentValidation;
using Product.Application.Dtos.Requests;
using Product.Application.Dtos.Response;
using Product.Application.Helpers;
using Product.Application.Mappings;
using Product.Application.Services.Interfaces;
using Product.Domain.Models;

namespace Product.Application.Services
{
    public class ProductService(
        IProductRepository productRepository,
        IValidator<ProductCreateDto> createValidator,
        IValidator<ProductModifyDto> modifyValidator) : IProductService
    {
        public async Task<List<ProductDto>> GetProducts(ProductFilterDto productFilterDto)
        {
            var filter = ExpressionBuilder.BuildWhereExpression(productFilterDto);
            var products = await productRepository.GetProducts(filter);

            return ProductMapper.MapToProductDtos(products);
        }

        public async Task<ProductDto> GetProduct(Guid id)
        {
            var product = await productRepository.GetProduct(id) ??
                          throw new NotFoundException($"Product with id {id} not found");

            return ProductMapper.MapToProductDto(product);
        }

        public async Task<ProductDto> CreateProduct(ProductCreateDto product)
        {
            await createValidator.ValidateAndThrowAsync(product);

            var newProduct = ProductMapper.MapToProduct(product);

            await productRepository.CreateProduct(newProduct);
            newProduct = await productRepository.GetProduct(newProduct.Id);

            return ProductMapper.MapToProductDto(newProduct);
        }

        public async Task<ProductDto> UpdateProduct(Guid id, ProductModifyDto product)
        {
            await modifyValidator.ValidateAndThrowAsync(product);
            var existingProduct = await productRepository.GetProduct(id) ??
                                  throw new NotFoundException($"Product not found with the given Id:[{id}]");

            ProductMapper.ProductModifyDtoToExisting(product, existingProduct);
            await productRepository.UpdateProduct(existingProduct);

            return ProductMapper.MapToProductDto(existingProduct);

        }

        public async Task DeleteProduct(Guid id)
        {
            var existingProduct = await productRepository.GetProduct(id) ??
                                  throw new NotFoundException($"Product not found with the given Id:[{id}]");

            await productRepository.DeleteProduct(existingProduct);
        }
    }
}
