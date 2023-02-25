namespace PackingTool.Core.Repository.User
{
    public interface IUserRepository
    {
        Task AddUser(string username, string password);
    }
}
