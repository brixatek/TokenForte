namespace TokenForte.Core.Models
{
    public class TokenForteValidationOptions
    {
        public string SigningKey { get; set; } = string.Empty;
        public TokenForteAlgorithm Algorithm { get; set; } = TokenForteAlgorithm.HS256;
        public string ValidIssuer { get; set; } = string.Empty;
        public string ValidAudience { get; set; } = string.Empty;
        public bool ValidateLifetime { get; set; } = true;
    }
}
