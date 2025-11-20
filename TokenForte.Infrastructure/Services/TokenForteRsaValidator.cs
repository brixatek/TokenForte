using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TokenForte.Core.Interfaces;
using TokenForte.Core.Models;

namespace TokenForte.Infrastructure.Services
{
    public class TokenForteRsaValidator : ITokenForteRsaValidator
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenForteRsaValidator(JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler ?? throw new ArgumentNullException(nameof(jwtSecurityTokenHandler));
        }

        public Task<TokenForteValidationResult> ValidateRsaAsync(TokenForteValidationOptions options, string token)
        {
            if (options == null)
                return Task.FromResult(CreateInvalidResult("Validation options cannot be null"));
            
            if (string.IsNullOrWhiteSpace(token))
                return Task.FromResult(CreateInvalidResult("Token cannot be null or empty"));
            
            if (string.IsNullOrWhiteSpace(options.SigningKey))
                return Task.FromResult(CreateInvalidResult("Signing key cannot be null or empty"));

            try
            {
                var securityKey = CreateRsaSecurityKey(options.SigningKey);

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

        private static Dictionary<string, string> ExtractClaims(ClaimsPrincipal principal)
        {
            return principal?.Claims?.ToDictionary(c => c.Type, c => c.Value) ?? new Dictionary<string, string>();
        }

        private static RsaSecurityKey CreateRsaSecurityKey(string signingKey)
        {
            var rsa = RSA.Create();
            
            if (signingKey.StartsWith("-----BEGIN PUBLIC KEY-----"))
            {
                var keyData = signingKey
                    .Replace("-----BEGIN PUBLIC KEY-----", "")
                    .Replace("-----END PUBLIC KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Trim();
                rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(keyData), out _);
            }
            else if (signingKey.StartsWith("-----BEGIN RSA PUBLIC KEY-----"))
            {
                var keyData = signingKey
                    .Replace("-----BEGIN RSA PUBLIC KEY-----", "")
                    .Replace("-----END RSA PUBLIC KEY-----", "")
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Trim();
                rsa.ImportRSAPublicKey(Convert.FromBase64String(keyData), out _);
            }
            else
                rsa.ImportRSAPublicKey(Convert.FromBase64String(signingKey), out _);
            
            return new RsaSecurityKey(rsa);
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
