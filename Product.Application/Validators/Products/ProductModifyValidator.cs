using FluentValidation;
using Product.Application.Dtos.Requests;

namespace Product.Application.Validators.Products
{
    public class ProductModifyValidator: AbstractValidator<ProductModifyDto>
    {
        public ProductModifyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(256);
        }
    }
}
