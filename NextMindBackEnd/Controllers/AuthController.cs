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
        public async Task<ActionResult<RegisterResponse>> Register([FromForm]RegisterRequest request)
        {
            Console.WriteLine(request.UserName);
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
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromForm] LoginRequest request)
        {
            try
            {
                LoginResponse response = await authService.Login(request);
                return Ok(response);
            }
            catch (LoginException ex)
            {
                LoginResponse error = new LoginResponse();
                error.Message = ex.Message;
                error.Code = 400;
                return BadRequest(error);
            }
        }
        [HttpGet, Route("login/{token}")]
        public async Task<ActionResult<LoginWithTokenResponse>> LoginWithToken(string token)
        {
            try
            {
                LoginWithTokenResponse response = await authService.LoginWithToken(token);
                return Ok(response);
            }catch (LoginException ex)
            {
                LoginWithTokenResponse error = new LoginWithTokenResponse();
                error.Messag = ex.Message;
                error.Code = 400;
                return BadRequest(error);
            }
        }

    }
}
