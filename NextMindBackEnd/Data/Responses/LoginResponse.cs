namespace NextMindBackEnd.Data.Responses
{
    public class LoginResponse
    {
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
