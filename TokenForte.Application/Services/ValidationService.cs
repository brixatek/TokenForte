using System.Threading.Tasks;
using TokenForte.Core.Interfaces;
using TokenForte.Core.Models;

namespace TokenForte.Application.Services
{
    public class ValidationService
    {
        private readonly ITokenForteValidator _validator;

        public ValidationService(ITokenForteValidator validator)
        {
            _validator = validator;
        }

        public Task<TokenForteValidationResult> ValidateAsync(string token, TokenForteValidationOptions options) => _validator.ValidateToken(token, options);
    }
}
