using Microsoft.AspNetCore.Mvc;
using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Data.Responses;
using NextMindBackEnd.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NextMindBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemoteControlController : ControllerBase
    {
        private readonly IRemoteControlService service;
        public RemoteControlController(IRemoteControlService service)
        {
            this.service = service;
        }
        [HttpPost("addController")]
        public async Task<ActionResult<AddControllerResponse>> AddController(AddControllerRequest request)
        {
            var response = new AddControllerResponse();
            try
            {
                response = await service.AddController(request);
                return Ok(response);
            }catch (AddControllerException ex)
            {
                response.Message=ex.Message;
                response.Code = ex.Code;
                return BadRequest(response);
            }
        } 
    }
}
