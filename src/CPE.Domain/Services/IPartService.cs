using System.Threading.Tasks;
using CPE.Domain.Model;

namespace CPE.Domain.Services
{
    public interface IPartService : IServiceBase<IPart>
    {
        Task<IPart> GetWhereDrawingNumberEqualsAsync(string drawingNumber);
    }
}