using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentValidation;
using IdsLibrary.Models;
using IdsLibrary.Models.PackageHeaders;
using IdsLibrary.Validators;

namespace IdsLibrary.Factories
{
    public class DeepLinkPackageFactory : IIdsPackageFactory<DeepLinkPackageHeader>
    {
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


            if (string.IsNullOrEmpty(packageHeader.HookUri?.ToString()) == false)
                content.Add(new StringContent(packageHeader.HookUri!.ToString()), PackageParameter.HookUri);

            if (packageHeader.Version != null)
                content.Add(new StringContent(packageHeader.Version), PackageParameter.Version);
            if (packageHeader.Target != null)
                content.Add(new StringContent(packageHeader.Target), PackageParameter.Target);

          
            content.Add(new StringContent(articleNumber), PackageParameter.SearchTerm);
            content.Add(new StringContent(ActionCode.ArticleSearch), PackageParameter.Action);

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
