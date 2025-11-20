using System.Threading.Tasks;
using TokenForte.Core.Models;

namespace TokenForte.Core.Interfaces
{
    public interface ITokenFortePssValidator
    {
        Task<TokenForteValidationResult> ValidatePssAsync(TokenForteValidationOptions options, string token);
    }
}
