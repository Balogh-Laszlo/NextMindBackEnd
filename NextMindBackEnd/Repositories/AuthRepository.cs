using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Models;

namespace NextMindBackEnd.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        public AuthRepository(DataContext context)
        {
            this.context = context;
        }

        public async Task<User> login(string userName)
        {
            try
            {
                var user = await (from u in context.Users where u.UserName == userName
                           select u).FirstOrDefaultAsync<User>();
                if( user!= null)
                {
                    return user;
                }
                throw new LoginRequestException("User not found");

            }
            catch (Exception ex)
            {
                throw new LoginRequestException(ex.Message);
            }
        }

        public async Task<User> register(User user)
        {
            try
            {
                if(context.Users.Any( u => u.UserName == user.UserName)) { throw new RegisterRequestException("Username already taken!"); }
                var registerResponse = context.Users.Add(user);
                await context.SaveChangesAsync();
                return registerResponse.Entity;
            }
            catch (System.Exception ex)
            {
                throw new RegisterRequestException(ex.Message);
            }
        }
    }
}
