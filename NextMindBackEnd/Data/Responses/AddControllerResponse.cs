using NextMindBackEnd.Data.Models;

namespace NextMindBackEnd.Data.Responses
{
    public class AddControllerResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public ControllerDataToClient Controller {get; set;}
    }
}
