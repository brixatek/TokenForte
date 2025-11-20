using System;
using System.Threading.Tasks;
using TokenForte.Core.Interfaces;
using TokenForte.Core.Models;

namespace TokenForte.Infrastructure.Services
{
    public class TokenForteValidator : ITokenForteValidator
    {
        private readonly ITokenForteHmacValidator _hmacValidator;
        private readonly ITokenForteRsaValidator _rsaValidator;
        private readonly ITokenFortePssValidator _pssValidator;
        private readonly ITokenForteEsdsaValidator _ecdsaValidator;

        public TokenForteValidator(ITokenForteHmacValidator hmacValidator, ITokenForteRsaValidator rsaValidator, ITokenFortePssValidator pssValidator, ITokenForteEsdsaValidator ecdsaValidator)
        {
            _hmacValidator = hmacValidator;
            _rsaValidator = rsaValidator;
            _pssValidator = pssValidator;
            _ecdsaValidator = ecdsaValidator;
        }

        public Task<TokenForteValidationResult> ValidateToken(string token, TokenForteValidationOptions options)
        {
            return options.Algorithm switch
            {
                TokenForteAlgorithm.HS256 => _hmacValidator.ValidateHmacAsync(options, token),
                TokenForteAlgorithm.HS384 => _hmacValidator.ValidateHmacAsync(options, token),
                TokenForteAlgorithm.HS512 => _hmacValidator.ValidateHmacAsync(options, token),
                TokenForteAlgorithm.RS256 => _rsaValidator.ValidateRsaAsync(options, token),
                TokenForteAlgorithm.RS384 => _rsaValidator.ValidateRsaAsync(options, token),
                TokenForteAlgorithm.RS512 => _rsaValidator.ValidateRsaAsync(options, token),
                TokenForteAlgorithm.PS256 => _pssValidator.ValidatePssAsync(options, token),
                TokenForteAlgorithm.PS384 => _pssValidator.ValidatePssAsync(options, token),
                TokenForteAlgorithm.PS512 => _pssValidator.ValidatePssAsync(options, token),
                TokenForteAlgorithm.ES256 => _ecdsaValidator.ValidateEsdsaAsync(options, token),
                TokenForteAlgorithm.ES384 => _ecdsaValidator.ValidateEsdsaAsync(options, token),
                TokenForteAlgorithm.ES512 => _ecdsaValidator.ValidateEsdsaAsync(options, token),
                _ => Task.FromResult(new TokenForteValidationResult { IsValid = false, Claims = null })
            };
        }
    }
}
