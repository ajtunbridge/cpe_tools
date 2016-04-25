using System.Threading.Tasks;
using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface IPartVersionService : IServiceBase<IPartVersion>
    {
        Task<IPartVersion> GetLatestVersionAsync(int partId);
    }
}