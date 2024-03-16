using Product.Domain.Abstracts;
using Product.Domain.Enums;

namespace Product.Domain.Models
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ProductType Type { get; set; }
    }
}
