using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using IdsLibrary.Models;
using IdsLibrary.Models.PackageHeaders;
using IdsLibrary.Validators;

namespace IdsLibrary.Factories
{
    /// <summary>
    /// Factory to create a package to send a deep link to the shop.
    /// </summary>
    public class DeepLinkPackageFactory : IIdsPackageFactory<DeepLinkPackageHeader>
    {
        /// <summary>
        /// Create a package to send a deep link to the shop.
        /// Deep link is used to search for an article in the shop.
        /// </summary>
        /// <param name="packageHeader">Package header.</param>
        /// /// <param name="articleNumber">Article number.</param>
        /// <returns>Ids package data.</returns>
        /// <exception cref="ValidationException">Validation exception.</exception>
        public async Task<IIdsPackage> CreatePackage(DeepLinkPackageHeader packageHeader, string articleNumber)
        {
            var validator = new DeepLinkPackageHeaderValidator();
            var validationResult = await validator.ValidateAsync(packageHeader);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"PackageHeader is not valid: {errors}");
            }

            var content = new MultipartFormDataContent();

            if (string.IsNullOrEmpty(packageHeader!.CustomerNumber) == false)
                content.Add(new StringContent(packageHeader.CustomerNumber), PackageParameter.CustomNumber);
            if (string.IsNullOrEmpty(packageHeader.UserName) == false)
                content.Add(new StringContent(packageHeader.UserName), PackageParameter.UserName);
            if (string.IsNullOrEmpty(packageHeader.Password) == false)
                content.Add(new StringContent(packageHeader.Password), PackageParameter.Password);

            // Dummy url necessary.
            content.Add(new StringContent("http://localhost"), PackageParameter.HookUri);

            if (packageHeader.Version != null)
                content.Add(new StringContent(packageHeader.Version), PackageParameter.Version);
            if (packageHeader.Target != null)
                content.Add(new StringContent(packageHeader.Target), PackageParameter.Target);

            content.Add(new StringContent(articleNumber), PackageParameter.WholesaleArticleNumber);

            // Action code for article search.
            content.Add(new StringContent(ActionCode.ArticleDeeplink), PackageParameter.Action);

            var idsPackage = new IdsPackage()
            {
                ShopUri = packageHeader.ShopUri,
                Content = content,
                Headers = content.Headers
            };

            return idsPackage;
        }
    }
}