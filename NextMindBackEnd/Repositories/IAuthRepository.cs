using NextMindBackEnd.Models;

namespace NextMindBackEnd.Repositories
{
    public interface IAuthRepository
    {
        public Task<User> register(User user);
    }
}
