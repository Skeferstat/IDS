using IdsLibrary.Models.PackageHeaders;
using System.Threading.Tasks;

namespace IdsLibrary.Factories
{
    public interface IIdsPackageFactory<in THeader> where THeader : IPackageHeader
    {
        Task<IIdsPackage> CreatePackage(THeader packageHeader, string data);
    }
}
