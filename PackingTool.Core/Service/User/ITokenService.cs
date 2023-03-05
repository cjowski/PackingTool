namespace PackingTool.Core.Service.User
{
    public interface ITokenService
    {
        string GenerateToken(int userID);
        int ValidateToken(string token);
    }
}
