using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TokenForte.Core.Interfaces;
using TokenForte.Core.Models;

namespace TokenForte.Infrastructure.Services
{
    public class TokenForteEsdsaValidator : ITokenForteEsdsaValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenForteEsdsaValidator(JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler ?? throw new ArgumentNullException(nameof(jwtSecurityTokenHandler));
        }

        public Task<TokenForteValidationResult> ValidateEsdsaAsync(TokenForteValidationOptions options, string token)
        {
            if (options == null)
                return Task.FromResult(CreateInvalidResult("Validation options cannot be null"));
            
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(CreateInvalidResult("Token cannot be null or empty"));
            
            if (string.IsNullOrWhiteSpace(options.SigningKey))
                return Task.FromResult(CreateInvalidResult("Signing key cannot be null or empty"));

            try
            {
                var securityKey = CreateEcdsaSecurityKey(options.SigningKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = !string.IsNullOrEmpty(options.ValidIssuer),
                    ValidIssuer = options.ValidIssuer,
                    ValidateAudience = !string.IsNullOrEmpty(options.ValidAudience),
                    ValidAudience = options.ValidAudience,
                    ValidateLifetime = options.ValidateLifetime,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = _jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out _);
                var claims = ExtractClaims(principal);

                return Task.FromResult(new TokenForteValidationResult
                {
                    IsValid = true,
                    Claims = claims
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(CreateInvalidResult(ex.Message));
            }
        }

        private static ECDsaSecurityKey CreateEcdsaSecurityKey(string signingKey)
        {
            var ecdsa = ECDsa.Create();
            
            if (signingKey.StartsWith("-----BEGIN PUBLIC KEY-----"))
            {
                var keyData = signingKey
                    .Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Trim();
                ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(keyData), out _);
            }
            else
                ecdsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(signingKey), out _);
            
            return new ECDsaSecurityKey(ecdsa);
        }

        private static Dictionary<string, string> ExtractClaims(ClaimsPrincipal principal)
        {
            return principal?.Claims?.ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>();
        }

        private static TokenForteValidationResult CreateInvalidResult(string errorMessage)
        {
            return new TokenForteValidationResult
            {
                IsValid = false,
                Claims = null
            };
        }
    }
}