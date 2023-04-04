namespace PackingTool.Core.Repository.User.Output
{
    public class UserAuthenticationDetailsDb
    {
        public string PasswordHash { get; }
        public string[] Roles { get; }
        public bool IsAuthorized { get; }
        public bool RequiredNewPassword { get; }

        public UserAuthenticationDetailsDb(
            string passwordHash,
            string[] roles,
            bool isAuthorized,
            bool requiredNewPassword
        )
        {
            PasswordHash = passwordHash;
            Roles = roles;
            IsAuthorized = isAuthorized;
            RequiredNewPassword = requiredNewPassword;
        }
    }
}
