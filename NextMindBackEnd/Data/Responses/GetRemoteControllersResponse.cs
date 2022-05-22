using NextMindBackEnd.Data.Models;

namespace NextMindBackEnd.Data.Responses
{
    public class GetRemoteControllersResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<ControllerDataToClient> RemoteControllers { get; set; } = new List<ControllerDataToClient>();
    }
}
