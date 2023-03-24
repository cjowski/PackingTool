using Microsoft.EntityFrameworkCore;

namespace PackingTool.MsSqlDatabase.Repository
{
    public class UserRepository : Core.Repository.User.IUserRepository
    {
        private DbModels.PackingContext _context { get; }

        public UserRepository(
            DbModels.PackingContext context            
        )
        {
            _context = context;
        }

        public async Task<int> GetUserID(
            string userName
        )
        {
            return await _context.User
                .Where(u => u.UserName == userName)
                .Select(u => u.UserId)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> EmailExists(
            string email
        )
        {
            return await _context.User.AnyAsync(u => u.Email == email);
        }

        public async Task AddUser(
            Core.Repository.User.Input.RegisterUser registerUser,
            int requestedUserID
        )
        {
            var user = new DbModels.User()
            {
                UserName = registerUser.UserName,
                PasswordHash = registerUser.PasswordHash,
                Email = registerUser.Email,
                CreatedDate = DateTime.Now,
                CreatedUserId = requestedUserID,
                ModifiedDate = DateTime.Now,
                ModifiedUserId = requestedUserID
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetPasswordHash(int userID)
        {
            return await _context.User
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
            var user = await _context.User.SingleAsync(x => x.UserId == userID);
            user.PasswordHash = password;
            user.ModifiedUserId = requestedUserID;
            user.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
