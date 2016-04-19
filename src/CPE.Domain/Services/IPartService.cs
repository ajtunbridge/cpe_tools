using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface IPartService : IServiceBase<IPart>
    {
        IPart GetWhereDrawingNumberEquals(string drawingNumber);
    }
}