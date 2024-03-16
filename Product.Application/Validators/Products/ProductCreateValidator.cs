using FluentValidation;
using Product.Application.Dtos.Requests;

namespace Product.Application.Validators.Products
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateDto>
    {
        public ProductCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(128);
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(256);
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
