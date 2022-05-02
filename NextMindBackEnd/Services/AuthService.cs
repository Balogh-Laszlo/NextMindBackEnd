using Microsoft.IdentityModel.Tokens;
using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Data.Responses;
using NextMindBackEnd.Models;
using NextMindBackEnd.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

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
                throw new RegisterException("The two password are not the same");
            }
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
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
                    response.Token = CreateToken(user);
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



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("my top secret key"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
