namespace PackingTool.Core.Service.User
{
    public interface IUserService
    {
        Task<Output.AuthenticateResponse> Authenticate(Input.AuthenticateUser authenticateUser);
        Task<Output.UserResponse> Register(Input.RegisterUser registerUser);
        Task<Output.UserResponse> UpdatePassword(string userName, string password);
    }
}
