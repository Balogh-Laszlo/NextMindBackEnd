using NextMindBackEnd.Data.Requests;
using NextMindBackEnd.Data.Responses;

namespace NextMindBackEnd.Services
{
    public interface IRemoteControlService
    {
        public Task<AddControllerResponse> AddController(AddControllerRequest request);
    }
}
