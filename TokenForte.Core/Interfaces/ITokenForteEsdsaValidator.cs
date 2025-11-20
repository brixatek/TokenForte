using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TokenForte.Core.Models;

namespace TokenForte.Core.Interfaces
{
    public interface ITokenForteEsdsaValidator
    {
        Task<TokenForteValidationResult> ValidateEsdsaAsync(TokenForteValidationOptions options, string token);
    }
}
