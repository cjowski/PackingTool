namespace PackingTool.Core.Service.User
{
    public interface IUserService
    {
        Task<int> GetUserID(string username);
        Task<Output.AuthenticateResponse> Authenticate(Input.AuthenticateUser user);
        Task<Output.UserResponse> Register(Input.RegisterUser user);
        Task<Output.UserResponse> UpdatePassword(string userName, string password);
    }
}
