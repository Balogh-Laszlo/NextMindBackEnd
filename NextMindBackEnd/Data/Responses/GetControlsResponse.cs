using NextMindBackEnd.Data.Models;

namespace NextMindBackEnd.Data.Responses
{
    public class GetControlsResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public List<ControlToClient> controls { get; set; }
    }
}
