using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Models;
using NextMindBackEnd.Data.Responses;
using NextMindBackEnd.Services;

namespace NextMindBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlController : ControllerBase
    {
        private readonly IControlService controlService;
        public ControlController(IControlService controlService)
        {
            this.controlService = controlService;
        }
        [HttpPost("getControls")]
        public async Task<ActionResult<GetControlsResponse>> GetControls(string Token)
        {
            try
            {
                var controls = await controlService.GetControls(Token);
                return Ok(controls);
            }catch(GetControlsException ex)
            {
                Data.Responses.GetControlsResponse error = new GetControlsResponse();
                error.Message = ex.Message;
                error.Code = ex.Code;
                return BadRequest(error);
            }
            
        }
    }
}
