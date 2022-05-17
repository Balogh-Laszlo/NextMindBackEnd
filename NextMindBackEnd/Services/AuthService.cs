using Microsoft.IdentityModel.Tokens;
using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Models;
using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Data.Responses;
using NextMindBackEnd.Models;
using NextMindBackEnd.Repositories;
using NextMindBackEnd.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace NextMindBackEnd.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;
        public AuthService(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        public async Task<RegisterResponse> Register(RegisterRequest request)
        {
            if(request.Password != request.ConfirmPassword)
            {
                throw new RegisterException("The two passwords are not the same");
            }
            TokenMethods.Instance.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User()
            {
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            RegisterResponse response = new RegisterResponse();
            try
            {
                var repoResponse = await authRepository.register(user);
                if (repoResponse != null)
                {
                    response.Message = "Success";
                    response.Code = 200;
                }
                return response;
            }
            catch (RegisterRequestException ex)
            {
                throw new RegisterException(ex.Message);
            }
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                var repoResponse = await authRepository.login(request.UserName);
                if (TokenMethods.Instance.VerifyPasswordHash(request.Password, repoResponse.PasswordHash, repoResponse.PasswordSalt))
                {
                    response.Token = TokenMethods.Instance.CreateToken(repoResponse);
                    response.Code= 200;
                    response.Message = "Success";
                    response.UserName = repoResponse.UserName;
                    return response;
                }
                throw new LoginException("Incorrect Password");
            }
            catch(LoginRequestException ex)
            {
                throw new LoginException(ex.Message);
            }
            
        }

        public async Task<LoginWithTokenResponse> LoginWithToken(string token)
        {
            var response = new LoginWithTokenResponse();
            var tokenData = TokenMethods.Instance.ValidateToken(token);
            if(tokenData != null)
            {
                response.UserName = tokenData.UserName;
                response.Messag = "Success";
                response.Code = 200;
                return response;
            }
            throw new LoginException("Invalid token");
        }



        



    }
}
