using Product.Domain.Enums;

namespace Product.Application.Dtos.Requests
{
    public record ProductCreateDto(string Name, decimal Price, string Description, ProductType Type);
}
