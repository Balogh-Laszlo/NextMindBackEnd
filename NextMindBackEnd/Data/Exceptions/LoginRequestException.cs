namespace NextMindBackEnd.Data.Exceptions
{
    public class LoginRequestException: Exception
    {
        public LoginRequestException()
        {

        }
        public LoginRequestException(string message): base(message)
        {

        }
    }
}
