namespace TokenForte.Core.Models
{
    public enum TokenForteAlgorithm
    {
        // HMAC
        HS256,
        HS384,
        HS512,

        // RSA
        RS256,
        RS384,
        RS512,

        // RSASSA-PSS
        PS256,
        PS384,
        PS512,

        // ECDSA
        ES256,
        ES384,
        ES512
    }
}
