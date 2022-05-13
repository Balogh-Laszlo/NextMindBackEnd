namespace NextMindBackEnd.Data.Responses
{
    public class LoginWithTokenResponse
    {
        public string? UserName { get; set; }
        public int Code { get; set; }
        public string Messag { get; set; } = string.Empty;
    }
}
