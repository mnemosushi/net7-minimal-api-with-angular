namespace GameConsoleAPI.TokenGenerators
{
    /// <summary>
    /// JWT symmetric key with HMACSHA256
    /// </summary>
    public class JwtOptions
    {
        public required string SecretKey { get; set; }
        public required string PublicKeyFileName { get; set; }
        public required string PrivateKeyFileName { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public int TokenExpiry { get; set; }
    }
}
