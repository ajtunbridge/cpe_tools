using System.Threading.Tasks;
using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface IPhotoService
    {
        Task<byte[]> GetPhotoByPartAsync(IPart part);
    }
}