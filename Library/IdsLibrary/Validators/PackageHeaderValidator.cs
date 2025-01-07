using FluentValidation;
using IdsLibrary.Models;

namespace IdsLibrary.Validators
{
    internal class PackageHeaderValidator : AbstractValidator<PackageHeader>
    {
        public PackageHeaderValidator()
        {
            RuleFor(x => x.ShopUri)
                .NotNull()
                .WithMessage("ShopUri must not be null.")
                .NotEmpty()
                .WithMessage("ShopUri must not be empty.");

            RuleFor(x => x.ActionCode)
                .Must(x => x != ActionCode.Unknown);
        }
    }
}
