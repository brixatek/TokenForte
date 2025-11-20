using System.Threading.Tasks;
using TokenForte.Core.Models;

namespace TokenForte.Core.Interfaces
{
    public interface ITokenForteRsaValidator
    {
        Task<TokenForteValidationResult> ValidateRsaAsync(TokenForteValidationOptions options, string token);
    }
}
