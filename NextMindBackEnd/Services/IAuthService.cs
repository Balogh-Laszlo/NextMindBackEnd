using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Data.Responses;

namespace NextMindBackEnd.Services
{
    public interface IAuthService
    {
        public Task<RegisterResponse> Register(RegisterRequest request);
        public Task<LoginResponse> Login(LoginRequest request);
        public Task<LoginWithTokenResponse> LoginWithToken(string token);
    }
}
