using Microsoft.AspNetCore.Mvc;
using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Data.Responses;
using NextMindBackEnd.Models;
using NextMindBackEnd.Services;
using System.Security.Cryptography;

namespace NextMindBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody]RegisterRequest request)
        {
            try
            {
                RegisterResponse response = await authService.Register(request);
                return Ok(response);
            }
            catch (RegisterException ex)
            {
                RegisterResponse error = new RegisterResponse();
                error.Message = ex.Message;
                error.Code = 400;
                return BadRequest(error);
            }

            
        }

        //[HttpPost("login")]
        //public async Task<ActionResult<string>> Login(UserDto request)
        //{
        //    if(user.UserName != request.UserName)
        //    {
        //        return BadRequest("User not found");
        //    }

        //    if (VerifyPasswordHash(request.Password,user.PasswordHash,user.PasswordSalt))
        //    {
        //        return BadRequest("Wrong password");
        //    }

        //    string token = CreateToken(user);

        //    return Ok("Token");
        //}





    }
}
