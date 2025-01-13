using FluentValidation;
using IdsLibrary.Models.PackageHeaders;

namespace IdsLibrary.Validators
{
    /// <summary>
    /// Validator for the search term package header.
    /// </summary>
    internal class SearchTermPackageHeaderValidator : AbstractValidator<SearchTermPackageHeader>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchTermPackageHeaderValidator"/> class.
        /// </summary>
        public SearchTermPackageHeaderValidator()
        {
            RuleFor(x => x.UserName).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x.ShopUri).NotNull().NotEmpty();
        }
    }
}
