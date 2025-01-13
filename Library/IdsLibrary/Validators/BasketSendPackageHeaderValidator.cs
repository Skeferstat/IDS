using FluentValidation;
using IdsLibrary.Models.PackageHeaders;

namespace IdsLibrary.Validators
{
    /// <summary>
    /// Validator for the basket send package header.
    /// </summary>
    internal class BasketSendPackageHeaderValidator : AbstractValidator<BasketSendPackageHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasketSendPackageHeaderValidator"/> class.
        /// </summary>
        public BasketSendPackageHeaderValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ShopUri).NotNull().NotEmpty();
            RuleFor(x => x.HookUri).NotNull().NotEmpty();
        }
    }
}
