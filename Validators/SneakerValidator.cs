public class SneakerValidator : AbstractValidator<Sneaker>
{
    public SneakerValidator()
    {
        RuleFor(sneaker => sneaker.Brand)
            .NotEmpty()
            .WithMessage("Brand is required");

        RuleFor(sneaker => sneaker.Price)
            .GreaterThan(0)
            .WithMessage("Price can't be negative");

        RuleFor(sneaker => sneaker.Name).NotEmpty().WithMessage("Name is required");

        RuleFor(sneaker => sneaker.Occasions)
            .NotEmpty()
            .WithMessage("Sneaker must have at least one occasion");
    }
}