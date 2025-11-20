# TokenForte

A comprehensive JWT validation library for .NET supporting multiple algorithms and key formats.

## Features

- **Multiple Algorithms**: HMAC (HS256/384/512), RSA (RS256/384/512), RSA-PSS (PS256/384/512), ECDSA (ES256/384/512)
- **Key Format Support**: PEM, Base64, raw keys from blob storage
- **Dependency Injection**: Built-in DI support
- **Async/Await**: Full async support

## Installation

```bash
dotnet add package TokenForte
```

## Quick Start

### 1. Register Services

```csharp
services.AddTokenForte();
```

### 2. Validate Tokens

```csharp
public class MyController : ControllerBase
{
    private readonly ValidationService _validationService;

    public MyController(ValidationService validationService)
    {
        _validationService = validationService;
    }

    public async Task<IActionResult> ValidateToken(string token)
    {
        var options = new TokenForteValidationOptions
        {
            Algorithm = TokenForteAlgorithm.HS256,
            SigningKey = "your-secret-key",
            ValidIssuer = "your-issuer",
            ValidAudience = "your-audience",
            ValidateLifetime = true
        };

        var result = await _validationService.ValidateAsync(token, options);
        
        if (result.IsValid)
        {
            // Access claims: result.Claims["sub"], result.Claims["iss"], etc.
            return Ok(result.Claims);
        }
        
        return BadRequest(result.ErrorMessage);
    }
}
```

## Supported Algorithms

| Algorithm | Key Type | Description |
|-----------|----------|-------------|
| HS256/384/512 | HMAC | Symmetric key |
| RS256/384/512 | RSA | RSA public key |
| PS256/384/512 | RSA-PSS | RSA public key with PSS padding |
| ES256/384/512 | ECDSA | Elliptic curve public key |

## Key Formats

```csharp
// PEM format
var pemKey = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA...
-----END PUBLIC KEY-----";

// Base64 format
var base64Key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA...";

// Both work with TokenForte
var options = new TokenForteValidationOptions
{
    SigningKey = pemKey, // or base64Key
    Algorithm = TokenForteAlgorithm.RS256
};
```