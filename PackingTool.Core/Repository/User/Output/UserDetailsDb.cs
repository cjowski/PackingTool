namespace PackingTool.Core.Repository.User.Output
{
    public class UserDetailsDb
    {
        public int UserID { get; }
        public string UserName { get; }
        public bool Authorized { get; }

        public UserDetailsDb(
            int userID,
            string userName,
            bool authorized
        )
        {
            UserID = userID;
            UserName = userName;
            Authorized = authorized;
        }
    }
}
