namespace PackingTool.Core.Repository.User
{
    public interface IUserRepository
    {
        Task<bool> UserNameExists(string userName);
        Task AddUser(Input.RegisterUser registerUser);
        Task<string> GetPasswordHash(string userName);
        Task UpdatePassword(string userName, string password);
    }
}
