using CoreService = PackingTool.Core.Service.User;

namespace PackingTool.Service.Service.User
{
    public class UserService : CoreService.IUserService
    {
        private Core.Repository.User.IUserRepository _repository { get; }

        public UserService(
            Core.Repository.User.IUserRepository repository
        )
        {
            _repository = repository;
        }

        public async Task AddUser(
            CoreService.Input.User user
        )
        {
            await _repository.AddUser(user.Name, user.Password);
        }
    }
}
