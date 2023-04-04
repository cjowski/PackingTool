namespace PackingTool.Core.Repository.User
{
    public interface IUserRepository
    {
        Task<int> GetUserID(string userName);
        Task<bool> EmailExists(string email);
        Task<Output.UserAuthenticationDetailsDb> GetUserAuthenticationDetails(int userID);
        Task SetLoginDate(int userID, DateTime lastLoginDate, bool onlyAttempt, int requestedUserID);
        Task AddUser(Input.RegisterUser registerUser, int requestedUserID);
        Task<string> GetPasswordHash(int userID);
        Task UpdatePassword(int userID, string password, int requestedUserID);
        Task<Output.UserDetailsDb[]> SearchUsers(string searchingPhrase);
        Task AuthorizeUser(int userID, bool authorized, int requestedUserID);
        Task SetTemporaryPassword(int userID, string password, int requestedUserID);
    }
}
