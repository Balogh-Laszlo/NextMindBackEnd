using NextMindBackEnd.Data.Exceptions;
using NextMindBackEnd.Repositories;
using NextMindBackEnd.Utils;
using NextMindBackEnd.Data.Responses;

namespace NextMindBackEnd.Services
{
    public class ControlService : IControlService
    {
        private readonly IControlRepository controlRepository;
        public ControlService(IControlRepository controlRepository)
        {
            this.controlRepository = controlRepository;
        }
        public async Task<GetControlsResponse> GetControls(string Token)
        {
            var user = TokenMethods.Instance.ValidateToken(Token);
            if(user == null)
            {
                throw new GetControlsException("Invalid token!", 300);
            }
            var userId = user.Id;
            var controls = await controlRepository.GetControls(userId);
            var controlsResponse = new GetControlsResponse()
            {
                Code = 200, Message = "Success", controls = controls
            };
            return controlsResponse;
           
        }
    }
}
