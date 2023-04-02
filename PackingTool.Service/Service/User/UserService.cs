using CoreService = PackingTool.Core.Service.User;
using CoreRepository = PackingTool.Core.Repository.User;

namespace PackingTool.Service.Service.User
{
    public class UserService : CoreService.IUserService
    {
        private CoreRepository.IUserRepository _repository { get; }
        private CoreService.ITokenService _tokenService { get; }

        public UserService(
            CoreRepository.IUserRepository repository,
            CoreService.ITokenService tokenService
        )
        {
            _repository = repository;
            _tokenService = tokenService;
        }

        public async Task<int> GetUserID(
            string userName    
        )
        {
            return await _repository.GetUserID(userName);
        }

        public async Task<CoreService.Output.AuthenticateResponse> Authenticate(
            CoreService.Input.AuthenticateUser user
        )
        {
            var userID = await _repository.GetUserID(user.UserName);
            if (userID == 0)
            {
                return CoreService.Output.AuthenticateResponse
                    .Failed("Username or password is incorrect");
            }

            var passwordHash = await _repository.GetPasswordHash(userID);
            if (!BCrypt.Net.BCrypt.Verify(user.Password, passwordHash))
            {
                return CoreService.Output.AuthenticateResponse
                    .Failed("Username or password is incorrect");
            }

            var userRolesDb = await _repository.GetUserRoles(userID);
            var userRoles = userRolesDb
                .Select(ur =>
                    Enum.Parse<CoreService.Output.UserRole>(
                        $"{char.ToUpperInvariant(ur[0])}{ur[1..]}"
                    )
                )
                .ToArray();

            var token = _tokenService.GenerateToken(userID);
            return CoreService.Output.AuthenticateResponse.Succeed(userID, userRoles, token);
        }

        public async Task<CoreService.Output.UserResponse> Register(
            CoreService.Input.RegisterUser user
        )
        {
            if (await _repository.GetUserID(user.UserName) > 0)
            {
                return CoreService.Output.UserResponse
                    .Failed($"User '{user.UserName}' already exists");
            }

            if (await _repository.EmailExists(user.Email))
            {
                return CoreService.Output.UserResponse
                    .Failed($"Email address '{user.Email}' is already taken");
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var registerUserDb = new CoreRepository.Input.RegisterUser(
                userName: user.UserName,
                passwordHash: passwordHash,
                email: user.Email
            );
            var systemUserID = await _repository.GetUserID(CoreRepository.UserContants.SystemUserName);
            await _repository.AddUser(registerUserDb, systemUserID);

            return CoreService.Output.UserResponse.Succeed();
        }

        public async Task<CoreService.Output.UserResponse> ChangePassword(
            CoreService.Input.ChangePassword changePassword,
            int userID,
            int requestedUserID
        )
        {
            var currentPasswordHash = await _repository.GetPasswordHash(userID);
            if (!BCrypt.Net.BCrypt.Verify(changePassword.CurrentPassword, currentPasswordHash))
            {
                return CoreService.Output.UserResponse
                    .Failed("Current password is incorrect");
            }

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
            await _repository.UpdatePassword(userID, newPasswordHash, requestedUserID);

            return CoreService.Output.UserResponse.Succeed();
        }

        public async Task<CoreService.Output.UserDetails[]> SearchUsers(
            string searchingPhrase
        )
        {
            var userDetailsDb = await _repository.SearchUsers(searchingPhrase);
            return userDetailsDb
                .Select(u =>
                    new CoreService.Output.UserDetails(
                        userID: u.UserID,
                        userName: u.UserName,
                        authorized: u.Authorized
                    )
                )
                .OrderBy(u => u.UserName)
                .ToArray();
        }

        public async Task AuthorizeUser(
            int userID,
            int requestedUserID
        )
        {
            await _repository.AuthorizeUser(userID, requestedUserID);
        }

        public async Task SetTemporaryPassword(
            CoreService.Input.SetTemporaryPassword setTemporaryPassword,
            int requestedUserID
        )
        {
            var temporaryPasswordHash = BCrypt.Net.BCrypt.HashPassword(setTemporaryPassword.Password);
            await _repository.SetTemporaryPassword(setTemporaryPassword.UserID, temporaryPasswordHash, requestedUserID);
        }
    }
}
