using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Input = PackingTool.Core.Repository.User.Input;
using Output = PackingTool.Core.Repository.User.Output;

namespace PackingTool.Database.Repository
{
    public class UserRepository : Core.Repository.User.IUserRepository
    {
        private DbModels.PackingToolContext _context { get; }

        public UserRepository(
            DbModels.PackingToolContext context
        )
        {
            _context = context;
        }

        public async Task<int> GetUserID(
            string userName
        )
        {
            return await _context.Users
                .Where(u => u.UserName == userName)
                .Select(u => u.UserId)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> EmailExists(
            string email
        )
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<Output.UserAuthenticationDetailsDb> GetUserAuthenticationDetails(
            int userID
        )
        {
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userID)
                .Select(ur => ur.Role.RoleName)
                .ToArrayAsync();

            return await _context.UserAuthorizations
                .Where(ua => ua.UserId == userID)
                .Select(ua =>
                    new Output.UserAuthenticationDetailsDb(
                        ua.PasswordHash,
                        userRoles,
                        ua.Authorized,
                        ua.RequiredNewPassword
                    )
                )
                .SingleAsync();
        }

        public async Task SetLoginDate(
            int userID,
            DateTime lastLoginDate,
            bool onlyAttempt,
            int requestedUserID
        )
        {
            var userAuthorization = await _context.UserAuthorizations.SingleAsync(x => x.UserId == userID);
            userAuthorization.LastLoginAttemptDate = lastLoginDate;
            if (!onlyAttempt)
            {
                userAuthorization.LastLoginDate = lastLoginDate;
            }
            userAuthorization.ModifiedUserId = requestedUserID;
            userAuthorization.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task AddUser(
            Input.RegisterUser registerUser,
            int requestedUserID
        )
        {
            var user = new DbModels.User()
            {
                UserName = registerUser.UserName,
                Email = registerUser.Email,
                Deleted = false,
                CreatedDate = DateTime.Now,
                CreatedUserId = requestedUserID,
                ModifiedDate = DateTime.Now,
                ModifiedUserId = requestedUserID
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var userAuthorization = new DbModels.UserAuthorization()
            {
                UserId = user.UserId,
                PasswordHash = registerUser.PasswordHash,
                Authorized = false,
                RequiredNewPassword = false,
                LastLoginDate = DateTime.Now,
                LoginAttemptsLeft = 10,
                LastLoginAttemptDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedUserId = requestedUserID,
                ModifiedDate = DateTime.Now,
                ModifiedUserId = requestedUserID
            };

            _context.UserAuthorizations.Add(userAuthorization);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetPasswordHash(int userID)
        {
            return await _context.UserAuthorizations
                .Where(u => u.UserId == userID)
                .Select(u => u.PasswordHash)
                .SingleAsync();
        }

        public async Task UpdatePassword(
            int userID,
            string password,
            int requestedUserID
        )
        {
            var userAuthorization = await _context.UserAuthorizations.SingleAsync(x => x.UserId == userID);
            userAuthorization.PasswordHash = password;
            userAuthorization.RequiredNewPassword = false;
            userAuthorization.ModifiedUserId = requestedUserID;
            userAuthorization.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<Output.UserDetailsDb[]> SearchUsers(
            string searchingPhrase
        )
        {
            var query = searchingPhrase.Length < 3
                ? _context.Users.Where(u => u.UserName.StartsWith(searchingPhrase))
                : _context.Users.Where(u => u.UserName.Contains(searchingPhrase));

            return await query
                .SelectMany(u =>
                    _context.UserRoles
                        .Where(ur => ur.UserId == u.UserId)
                        .DefaultIfEmpty(),
                        (user, role) => new { user, role.Role.RoleName }
                )
                .Where(u =>
                    u.user.UserAuthorization != null
                    && u.RoleName != "admin"
                )
                .Take(10)
                .Select(u =>
                    new Output.UserDetailsDb(
                        u.user.UserId,
                        u.user.UserName,
                        u.user.UserAuthorization!.Authorized
                    )
                )
                .ToArrayAsync();
        }

        public async Task AuthorizeUser(
            int userID,
            bool authorized,
            int requestedUserID
        )
        {
            var userAuthorization = await _context.UserAuthorizations.SingleAsync(x => x.UserId == userID);
            userAuthorization.Authorized = authorized;
            userAuthorization.ModifiedUserId = requestedUserID;
            userAuthorization.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task SetTemporaryPassword(
            int userID,
            string password,
            int requestedUserID
        )
        {
            var userAuthorization = await _context.UserAuthorizations.SingleAsync(x => x.UserId == userID);
            userAuthorization.PasswordHash = password;
            userAuthorization.RequiredNewPassword = true;
            userAuthorization.ModifiedUserId = requestedUserID;
            userAuthorization.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
