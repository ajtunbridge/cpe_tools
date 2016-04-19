using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface IPhotoService
    {
        byte[] GetPhotoByPart(IPart part);
    }
}