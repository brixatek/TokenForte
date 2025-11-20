using System.Collections.Generic;

namespace TokenForte.Core.Models
{
    public class TokenForteValidationResult
    {
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
        public IDictionary<string, string>? Claims { get; set; }
    }
}
