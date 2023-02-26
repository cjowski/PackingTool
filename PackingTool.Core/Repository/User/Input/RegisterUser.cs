namespace PackingTool.Core.Repository.User.Input
{
    public class RegisterUser
    {
        public string UserName { get; }
        public string PasswordHash { get; }
        public string Email { get; }

        public RegisterUser(
            string userName,
            string passwordHash,
            string email
        )
        {
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
        }
    }
}
