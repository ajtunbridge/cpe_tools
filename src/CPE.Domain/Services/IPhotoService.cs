using System.Threading.Tasks;
using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface IPhotoService
    {
        Task<byte[]> GetPhotoByDrawingNumber(string drawingNumber);
        Task<byte[]> GetPhotoByPartAsync(IPart part);
    }
}