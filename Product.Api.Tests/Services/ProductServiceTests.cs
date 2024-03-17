using FluentValidation;
using Moq;
using Product.Application.Dtos.Requests;
using Product.Application.Helpers;
using Product.Application.Services;
using Product.Application.Validators.Products;
using Product.Domain.Enums;
using Product.Domain.Models;
using System.Linq.Expressions;

namespace Product.Api.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly IValidator<ProductModifyDto> _modifyValidator;
        private readonly IValidator<ProductCreateDto> _createValidator;
        private readonly ProductService productService;

        public ProductServiceTests()
        {
            _productRepository = new Mock<IProductRepository>();
            _modifyValidator = new ProductModifyValidator();
            _createValidator = new ProductCreateValidator();
            productService = new ProductService(_productRepository.Object, _createValidator, _modifyValidator);

        }

        [Fact]
        public async Task UpdateProduct_UpdateProduct_WhenProductIsValid()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productModifyDto = new ProductModifyDto("TestName", 10, "TestDescription");
            var product = new Domain.Models.Product { Id = productId, Name = "Existing Product", Price = 10, Description = "Test", Type = ProductType.TypeFive };
            _productRepository.Setup(r => r.GetProductById(It.IsAny<Guid>())).ReturnsAsync(product);
            _productRepository.Setup(r => r.GetProductByFilter(It.IsAny<Expression<Func<Domain.Models.Product, bool>>>())).ReturnsAsync(() => null);
            _productRepository.Setup(r => r.UpdateProduct(It.IsAny<Domain.Models.Product>())).Returns(Task.CompletedTask);

            // Act
            var result = await productService.UpdateProduct(productId, productModifyDto);

            // Assert
            _productRepository.Verify(r => r.GetProductById(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(r => r.GetProductByFilter(It.IsAny<Expression<Func<Domain.Models.Product, bool>>>()), Times.Once);
            _productRepository.Verify(r => r.UpdateProduct(It.IsAny<Domain.Models.Product>()), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(result.Name, productModifyDto.Name);
            Assert.Equal(result.Description, productModifyDto.Description);
            Assert.Equal(result.Id, product.Id);
            Assert.Equal(result.Type, product.Type);
            Assert.Equal(result.Price, productModifyDto.Price);
        }

        [Fact]
        public async Task UpdateProduct_ThrowNotFoundException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productModifyDto = new ProductModifyDto ("TestName", 10, "TestDescription");
            _productRepository.Setup(r => r.GetProductById(It.IsAny<Guid>())).ReturnsAsync(()=> null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => productService.UpdateProduct(Guid.NewGuid(), productModifyDto));

            _productRepository.Verify(r => r.GetProductById(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(r => r.GetProductByFilter(It.IsAny<Expression<Func<Domain.Models.Product, bool>>>()), Times.Never);
            _productRepository.Verify(r => r.UpdateProduct(It.IsAny<Domain.Models.Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProduct_ThrowArgumentException_WhenProductWithSameNameExists()
        {
            var productModifyDto = new ProductModifyDto("TestName", 10, "Test");
            var product = new Domain.Models.Product { Id = Guid.NewGuid(),Name = "TestName", Description = "Test", Price = 10, Type = ProductType.TypeFive};
            _productRepository.Setup(r => r.GetProductById(It.IsAny<Guid>())).ReturnsAsync(product);
            _productRepository.Setup(r => r.GetProductByFilter(It.IsAny<Expression<Func<Domain.Models.Product, bool>>>())).ReturnsAsync(product);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => productService.UpdateProduct(Guid.NewGuid(), productModifyDto));

            _productRepository.Verify(r => r.GetProductById(It.IsAny<Guid>()), Times.Once);
            _productRepository.Verify(r => r.GetProductByFilter(It.IsAny<Expression<Func<Domain.Models.Product, bool>>>()), Times.Once);
            _productRepository.Verify(r => r.UpdateProduct(It.IsAny<Domain.Models.Product>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProduct_ThrowValidationException_WhenProductIsNotValid()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productModifyDto = new ProductModifyDto("", 10, "TestDescription");
            var product = new Domain.Models.Product { Id = productId, Name = "Existing Product", Price = 10, Description = "Test", Type = ProductType.TypeFive };
            _productRepository.Setup(r => r.GetProductById(It.IsAny<Guid>())).ReturnsAsync(product);
            _productRepository.Setup(r => r.GetProductByFilter(It.IsAny<Expression<Func<Domain.Models.Product, bool>>>())).ReturnsAsync(() => null);
            _productRepository.Setup(r => r.UpdateProduct(It.IsAny<Domain.Models.Product>())).Returns(Task.CompletedTask);

            // Act
            await Assert.ThrowsAsync<ValidationException>(() => productService.UpdateProduct(Guid.NewGuid(), productModifyDto));

            // Assert
            _productRepository.Verify(r => r.GetProductById(It.IsAny<Guid>()), Times.Never);
            _productRepository.Verify(r => r.GetProductByFilter(It.IsAny<Expression<Func<Domain.Models.Product, bool>>>()), Times.Never);
            _productRepository.Verify(r => r.UpdateProduct(It.IsAny<Domain.Models.Product>()), Times.Never);

        }
    }
}
