using Product.Application.Dtos.Requests;
using Product.Application.Dtos.Response;
using Riok.Mapperly.Abstractions;

namespace Product.Application.Mappings
{
    [Mapper(AllowNullPropertyAssignment = false)]
    public static partial class ProductMapper
    {
        public static partial ProductDto MapToProductDto(Domain.Models.Product product);
        public static partial List<ProductDto> MapToProductDtos(List<Domain.Models.Product> products);
        public static partial Domain.Models.Product MapToProduct(ProductCreateDto dto);
        public static partial void ProductModifyDtoToExisting(ProductModifyDto modifyDto, Domain.Models.Product product);
    }
}
