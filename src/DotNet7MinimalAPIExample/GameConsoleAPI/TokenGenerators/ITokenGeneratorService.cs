namespace GameConsoleAPI.TokenGenerators
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(string username, string fullName);
    }
}
