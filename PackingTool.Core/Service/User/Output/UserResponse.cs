namespace PackingTool.Core.Service.User.Output
{
    public class UserResponse
    {
        public bool Success { get; }
        public string Message { get; }

        private UserResponse(
            bool success,
            string message
        )
        {
            Success = success;
            Message = message;
        }

        public static UserResponse Succeed()
        {
            return new UserResponse(
                success: true,
                message: string.Empty
            );
        }

        public static UserResponse Failed(
            string message
        )
        {
            return new UserResponse(
                success: false,
                message: message
            );
        }
    }
}
