using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using TokenForte.Core.Models;

namespace TokenForte.Core.Interfaces
{
    public interface ITokenForteHmacValidator
    {
        Task<TokenForteValidationResult> ValidateHmacAsync(TokenForteValidationOptions options, string token);
    }
}
