using CoreService = PackingTool.Core.Service.User;
using CoreRepository = PackingTool.Core.Repository.User;

namespace PackingTool.Service.Service.User
{
    public class UserService : CoreService.IUserService
    {
        private CoreRepository.IUserRepository _repository { get; }

        public UserService(
            CoreRepository.IUserRepository repository
        )
        {
            _repository = repository;
        }

        public async Task<CoreService.Output.AuthenticateResponse> Authenticate(
            CoreService.Input.AuthenticateUser authenticateUser
        )
        {
            if (!await _repository.UserNameExists(authenticateUser.UserName))
            {
                return CoreService.Output.AuthenticateResponse
                    .Failed("Username or password is incorrect.");
            }

            var passwordHash = await _repository.GetPasswordHash(authenticateUser.UserName);
            if (!BCrypt.Net.BCrypt.Verify(authenticateUser.Password, passwordHash))
            {
                return CoreService.Output.AuthenticateResponse
                    .Failed("Username or password is incorrect.");
            }

            //TODO TOKEN
        }

        public async Task<CoreService.Output.UserResponse> Register(
            CoreService.Input.RegisterUser registerUser
        )
        {
            if (await _repository.UserNameExists(registerUser.UserName))
            {
                return CoreService.Output.UserResponse
                    .Failed($"User: '{registerUser.UserName}' already exists.");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            var registerUserDb = new CoreRepository.Input.RegisterUser(
                userName: registerUser.UserName,
                passwordHash: passwordHash,
                email: string.Empty //TODO
            );
            await _repository.AddUser(registerUserDb);

            return CoreService.Output.UserResponse.Succeed();
        }

        public async Task<CoreService.Output.UserResponse> UpdatePassword(
            string userName,
            string password
        )
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            await _repository.UpdatePassword(userName, passwordHash);

            return CoreService.Output.UserResponse.Succeed();
        }
    }
}
