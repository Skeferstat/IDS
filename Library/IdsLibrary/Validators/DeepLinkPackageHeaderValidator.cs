using FluentValidation;
using IdsLibrary.Models.PackageHeaders;

namespace IdsLibrary.Validators
{
    /// <summary>
    /// Validator for the deep link package header.
    /// </summary>
    internal class DeepLinkPackageHeaderValidator : AbstractValidator<DeepLinkPackageHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeepLinkPackageHeaderValidator"/> class.
        /// </summary>
        public DeepLinkPackageHeaderValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ShopUri).NotNull().NotEmpty();
        }
    }
}
