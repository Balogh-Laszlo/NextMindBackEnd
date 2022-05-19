using NextMindBackEnd.Data.Responses;

namespace NextMindBackEnd.Services
{
    public interface IControlService
    {
        public Task<GetControlsResponse> GetControls(string Token);
    }
}
