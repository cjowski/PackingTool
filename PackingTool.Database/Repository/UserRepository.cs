namespace PackingTool.Database.Repository
{
    public class UserRepository : Core.Repository.IUserRepository
    {
        private DbModels.PackingContext _context { get; }

        public UserRepository(
            DbModels.PackingContext context            
        )
        {
            _context = context;
        }

        public async Task AddUser(
            string username,
            string password
        )
        {
            var user = new DbModels.User()
            {
                Name = username,
                Password = password,
                CreatedDate = DateTime.Now,
                CreatedUserId = Constants.Constants.SystemUserID,
                ModifiedDate = DateTime.Now,
                ModifiedUserId = Constants.Constants.SystemUserID
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
