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
                if (VerifyPasswordHash(request.Password, repoResponse.PasswordHash, repoResponse.PasswordSalt))
                {
                    response.Token = CreateToken(repoResponse);
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
            var tokenData = ValidateToken(token);
            if(tokenData != null)
            {
                response.UserName = tokenData.UserName;
                response.Messag = "Success";
                response.Code = 200;
                return response;
            }
            throw new LoginException("Invalid token");
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
                new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Constants.secretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private TokenData? ValidateToken(string token)
        {
            TokenData tokenData = new TokenData();
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Constants.secretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);
                var userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

                tokenData.UserName = userName;
                tokenData.Id = userId;
                // return user id from JWT token if validation successful
                return tokenData;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        } 



    }
}
