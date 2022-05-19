using NextMindBackEnd.Data.Models;

namespace NextMindBackEnd.Data.Requests
{
    public class AddControllerRequest
    {
        public string Token { get; set; }
        public string ControllerName { get; set; }
        public List<PageDataFromClient> Pages { get; set; }
    }
}
