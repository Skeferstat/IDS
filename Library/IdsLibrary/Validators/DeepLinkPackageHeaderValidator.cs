using FluentValidation;
using IdsLibrary.Models.PackageHeaders;

namespace IdsLibrary.Validators
{
    internal class DeepLinkPackageHeaderValidator : AbstractValidator<DeepLinkPackageHeader>
    {
        public DeepLinkPackageHeaderValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ShopUri).NotNull().NotEmpty();
            RuleFor(x => x.HookUri).NotNull().NotEmpty();
        }
    }
}
