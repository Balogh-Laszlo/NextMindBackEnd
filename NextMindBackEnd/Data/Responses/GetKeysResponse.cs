using NextMindBackEnd.Data.Models;

namespace NextMindBackEnd.Data.Responses
{
    public class GetKeysResponse
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<Key>? Keys { get; set; }
    }
}
