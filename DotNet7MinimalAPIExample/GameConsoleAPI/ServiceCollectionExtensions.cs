using GameConsoleAPI.TokenGenerators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;

namespace GameConsoleAPI
{
    public static class ServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddAsymmetricJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<ITokenGeneratorService, AsymmetricTokenGeneratorService>()
                .AddAuthentication()
                .AddJwtBearer(options =>
                {
                    string publicKeyFileName = configuration["JwtOptions:PublicKeyFileName"]!;
                    string issuer = configuration["JwtOptions:Issuer"]!;
                    string audience = configuration["JwtOptions:Audience"]!;

                    var publicKey = File.ReadAllText(publicKeyFileName);
                    var publicRsa = RSA.Create();
                    publicRsa.ImportFromPem(publicKey);
                    var issuerSigningKey = new RsaSecurityKey(publicRsa);

                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = audience, // Audience (who or what the token is intented for)
                        ValidIssuer = issuer, // Issuer (who created and signed this token) 
                        
                        ValidateIssuerSigningKey = true, // Important!
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        IssuerSigningKey = issuerSigningKey,
                    };
                });
        }

        public static AuthenticationBuilder AddSymmetricJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddSingleton<ITokenGeneratorService, SymmetricTokenGeneratorService>()
                .AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(configuration["JwtOptions:SecretKey"]!)),
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });
        }
    }
}
