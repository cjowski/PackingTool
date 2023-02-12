namespace PackingTool.Core.Repository
{
    public interface IUserRepository
    {
        Task AddUser(string username, string password);
    }
}
