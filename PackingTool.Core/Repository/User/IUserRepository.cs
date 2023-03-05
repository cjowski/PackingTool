namespace PackingTool.Core.Repository.User
{
    public interface IUserRepository
    {
        Task<int> GetUserID(string userName);
        Task<bool> EmailExists(string email);
        Task AddUser(Input.RegisterUser registerUser);
        Task<string> GetPasswordHash(int userID);
        Task UpdatePassword(int userID, string password);
    }
}
