using FluentValidation;

namespace Catalog.API.DTO
{
    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string ImgFileName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
    }
    public class ProductValidator : AbstractValidator<ProductDto>
    {
        public ProductValidator() {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Product must have a name");

            RuleFor(p => p.Category)
             .NotEmpty().WithMessage("Product must have a category");

            RuleFor(p=>p.Price)
                .NotEmpty().WithMessage("Price Must not be Empty")
                .NotNull().WithMessage("You Must add price ")
                .ExclusiveBetween(0,50000).WithMessage("Price Must be between 0 and 50000");
        }   
    }
}
