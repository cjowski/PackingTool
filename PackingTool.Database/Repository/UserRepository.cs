using Microsoft.EntityFrameworkCore;

namespace PackingTool.Database.Repository
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

        public async Task<bool> UserNameExists(
            string userName
        )
        {
            return await _context.User.AnyAsync(u => u.UserName == userName);
        }

        public async Task AddUser(
            Core.Repository.User.Input.RegisterUser registerUser
        )
        {
            var user = new DbModels.User()
            {
                UserName = registerUser.UserName,
                PasswordHash = registerUser.PasswordHash,
                Email = registerUser.Email,
                CreatedDate = DateTime.Now,
                CreatedUserId = Constants.Constants.SystemUserID,
                ModifiedDate = DateTime.Now,
                ModifiedUserId = Constants.Constants.SystemUserID
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetPasswordHash(string userName)
        {
            return await _context.User
                .Where(u => u.UserName == userName)
                .Select(u => u.PasswordHash)
                .SingleAsync();
        }

        public async Task UpdatePassword(
            string userName,
            string password
        )
        {
            var user = await _context.User.SingleAsync(x => x.UserName == userName);
            user.PasswordHash = password;
            user.ModifiedUserId = user.UserId;
            user.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}
