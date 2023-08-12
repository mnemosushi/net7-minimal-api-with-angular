namespace GameConsoleAPI
{
    public record class AuthenticateRequestDto(string UserName, string Password);
    public record class AuthenticateSuccessResponsedto(string AuthToken);
    public record class User(string UserName, string Password, string FirstName, string LastName)
    {
        public string Name => $"{FirstName} {LastName}";
    }
    public record class GameConsole(string Name, string Manufacturer, int ConstructionYear);
}
