using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace GameConsoleAPI.TokenGenerators
{
    /// <summary>
    /// Assymetric encryption is based on two keys, a public key and a private key.
    /// 
    /// Generate RSA public and private key (PEM): https://cryptotools.net/rsagen
    /// </summary>
    public class AsymmetricTokenGeneratorService : ITokenGeneratorService
    {
        private readonly JwtOptions _jwtOptions;

        public AsymmetricTokenGeneratorService(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateToken(string username, string fullName)
        {
            var privateRsa = RSA.Create();
            var privateKey = File.ReadAllText(_jwtOptions.PrivateKeyFileName);
            privateRsa.ImportFromPem(privateKey);
            var securityKey = new RsaSecurityKey(privateRsa);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Name, fullName)
            };

            var securityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}
