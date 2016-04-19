using System.Threading.Tasks;

namespace CPE.Domain.Services
{
    public interface ITricornService
    {
        Task<string> GetNameByDrawingNumberAsync(string drawingNumber);
    }
}