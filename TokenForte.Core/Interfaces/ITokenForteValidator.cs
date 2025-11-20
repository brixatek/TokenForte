using System.Threading.Tasks;
using TokenForte.Core.Models;

namespace TokenForte.Core.Interfaces
{
    public interface ITokenForteValidator
    {
        Task<TokenForteValidationResult> ValidateToken(string token, TokenForteValidationOptions options);
    }
}
