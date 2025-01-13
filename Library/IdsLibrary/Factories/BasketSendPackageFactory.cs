using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdsLibrary.Models;
using IdsLibrary.Serializing;
using FluentValidation;
using IdsLibrary.Converter;
using IdsLibrary.Models.PackageHeaders;
using IdsLibrary.Validators;

namespace IdsLibrary.Factories
{
    /// <summary>
    /// Factory to create a package to send a basket to the shop.
    /// </summary>
    public class BasketSendPackageFactory : IIdsPackageFactory<BasketSendPackageHeader>
    {
        /// <summary>
        /// Create a package to send a basket to the shop.
        /// </summary>
        /// <param name="packageHeader">Package header.</param>
        /// /// <param name="basketXml">Basket data as xml string.</param>
        /// <returns>Ids package data.</returns>
        /// <exception cref="ValidationException">Validation exception.</exception>
        public async Task<IIdsPackage> CreatePackage(BasketSendPackageHeader packageHeader, string basketXml)
        {
            var validator = new BasketSendPackageHeaderValidator();
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

            
            if (string.IsNullOrEmpty(packageHeader.HookUri?.ToString()) == false)
                content.Add(new StringContent(packageHeader.HookUri!.ToString()), PackageParameter.HookUri);

            if (packageHeader.Version != null)
                content.Add(new StringContent(packageHeader.Version), PackageParameter.Version);
            if (packageHeader.Target != null)
                content.Add(new StringContent(packageHeader.Target), PackageParameter.Target);

            var basket = Deserializer.DeserializeBasketSend(basketXml!);
            if (basket == null)
            {
                throw new ValidationException("Basket data is not valid");
            }

            content.Add(IdsConverter.ConvertToStringContent(basket), PackageParameter.Basket);

            // Command.
            content.Add(new StringContent(ActionCode.SendBasketToShop), PackageParameter.Action);

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
