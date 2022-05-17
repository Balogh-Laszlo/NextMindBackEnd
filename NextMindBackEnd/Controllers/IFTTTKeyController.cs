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
    public class IFTTTKeyController : ControllerBase
    {
        private readonly IIFTTTKeyService service;
        public IFTTTKeyController(IIFTTTKeyService service)
        {
            this.service = service;
        }

        [HttpPost("getKeys")]
        public async Task<ActionResult<GetKeysResponse>> GetKeys([FromForm]GetKeysRequest request)
        {
            try
            {
                GetKeysResponse response = await service.GetKeys(request.Token);
                return Ok(response);

            }catch (GetKeysException ex)
            {
                GetKeysResponse error = new GetKeysResponse();
                error.Message = ex.Message;
                error.Code = 400;
                return BadRequest(error);
            }
        }
        [HttpPost("addKey")]
        public async Task<ActionResult<AddKeyResponse>> AddKey([FromForm] AddKeyRequest request)
        {
            try
            {
                AddKeyResponse response = await service.AddKey(request.Token,request.Key);
                return Ok(response);
            }catch (AddKeyException ex)
            {
                AddKeyResponse error = new AddKeyResponse();
                error.Code = ex.ErrorCode;
                error.Message = ex.Message;
                return BadRequest(error);
            }
        }
    }
}
