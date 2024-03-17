using Microsoft.AspNetCore.Mvc;
using Moq;
using Product.Api.Controllers;
using Product.Application.Dtos.Requests;
using Product.Application.Dtos.Response;
using Product.Application.Helpers;
using Product.Application.Services.Interfaces;
using Product.Domain.Enums;
using System;
using FluentValidation;

namespace Product.Api.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productService = new Mock<IProductService>();
            _controller = new ProductsController(_productService.Object);
        }

        [Fact]
        public async Task GetProduct_ReturnsProductDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var product = new ProductDto { Id = id };
            _productService.Setup(service => service.GetProduct(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var id = Guid.NewGuid();
            _productService.Setup(service => service.GetProduct(It.IsAny<Guid>())).ThrowsAsync(new NotFoundException("Not found"));

            // Act
            Func<Task> act = async () =>await  _controller.GetProduct(id);

            // Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(act);
            Assert.Equal("Not found", exception.Message);
        }

        [Fact]
        public async Task CreateProduct_ReturnsProductDto()
        {
            // Arrange
            var productCreateDto = new ProductCreateDto("Test name", 10, "Test Description",ProductType.TypeFive );
            var productDto = new ProductDto
            {
                Name = "Test name",
                Price = 10,
                Description = "Test Description",
                Type = ProductType.TypeFive
            };
            _productService.Setup(service => service.CreateProduct(It.IsAny<ProductCreateDto>())).ReturnsAsync(productDto);

            // Act
            var result = await _controller.CreateProduct(productCreateDto);

            // Assert
            Assert.Equal(productDto, result);
        }

        [Fact]
        public async Task CreateProduct_ThrowsValidationError()
        {
            // Arrange
            var productCreateDto = new ProductCreateDto("Test name", 10, "Test Description", ProductType.TypeFive);
            _productService.Setup(service => service.CreateProduct(It.IsAny<ProductCreateDto>())).ThrowsAsync(new ValidationException("Validation error"));
            
            // Act
            Func<Task> act = async () => await _controller.CreateProduct(productCreateDto);

            // Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(act);
            Assert.Equal("Validation error", exception.Message);
        }
    }
}
