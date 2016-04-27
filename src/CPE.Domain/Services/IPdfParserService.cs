using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CPE.Domain.Services
{
    public interface IPdfParserService
    {
        Task LoadPdfAsync(string fileName);

        bool CanFind(string expression, RegexOptions options);

        MatchCollection Find(string expression, RegexOptions options);
    }
}