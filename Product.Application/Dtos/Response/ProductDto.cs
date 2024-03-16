using Product.Domain.Enums;

namespace Product.Application.Dtos.Response
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ProductType Type { get; set; }
    }
}
