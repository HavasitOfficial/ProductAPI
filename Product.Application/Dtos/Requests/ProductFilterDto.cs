using Product.Domain.Enums;

namespace Product.Application.Dtos.Requests
{
    public record ProductFilterDto(string? Name, decimal? Price, string? Description, ProductType? Type);
}
