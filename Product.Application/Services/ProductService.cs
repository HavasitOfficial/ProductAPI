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
            var product = await productRepository.GetProductById(id) ??
                          throw new NotFoundException($"Product with id {id} not found");

            return ProductMapper.MapToProductDto(product);
        }

        public async Task<ProductDto> CreateProduct(ProductCreateDto productDto)
        {
            await createValidator.ValidateAndThrowAsync(productDto);

            var existingProduct = await productRepository.GetProductByFilter(x => x.Name.Equals(productDto.Name));

            if (existingProduct != null)
            {
                throw new ArgumentException($"Product with name {productDto.Name} already exists");
            }

            var newProduct = ProductMapper.MapToProduct(productDto);

            await productRepository.CreateProduct(newProduct);
            newProduct = await productRepository.GetProductById(newProduct.Id);

            return ProductMapper.MapToProductDto(newProduct);
        }

        public async Task<ProductDto> UpdateProduct(Guid id, ProductModifyDto productDto)
        {
            await modifyValidator.ValidateAndThrowAsync(productDto);
            var product = await productRepository.GetProductById(id) ??
                                  throw new NotFoundException($"Product not found with the given Id:[{id}]");

            var existingProduct = await productRepository.GetProductByFilter(x => x.Name.Equals(product.Name) && x.Id != product.Id);

            if (existingProduct != null)
            {
                throw new ArgumentException($"Product with name {productDto.Name} already exists");
            }

            ProductMapper.ProductModifyDtoToExisting(productDto, product);
            await productRepository.UpdateProduct(product);

            return ProductMapper.MapToProductDto(product);

        }

        public async Task DeleteProduct(Guid id)
        {
            var existingProduct = await productRepository.GetProductById(id) ??
                                  throw new NotFoundException($"Product not found with the given Id:[{id}]");

            await productRepository.DeleteProduct(existingProduct);
        }
    }
}
