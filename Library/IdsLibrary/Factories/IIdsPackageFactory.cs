using IdsLibrary.Models.PackageHeaders;
using System.Threading.Tasks;

namespace IdsLibrary.Factories
{
    /// <summary>
    /// Factory to create a package to send data to the shop.
    /// </summary>
    /// <typeparam name="THeader">Package header type.</typeparam>
    public interface IIdsPackageFactory<in THeader> where THeader : IPackageHeader
    {
        /// <summary>
        /// Create a package to send data to the shop.
        /// </summary>
        /// <param name="packageHeader">Package header.</param>
        /// /// <param name="data">Data depending on the package type.</param>
        /// <returns>Ids package data.</returns>
        Task<IIdsPackage> CreatePackage(THeader packageHeader, string data);
    }
}
