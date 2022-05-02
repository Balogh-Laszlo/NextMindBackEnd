namespace NextMindBackEnd.Data.Exceptions
{
    public class RegisterRequestException: Exception
    {
        public RegisterRequestException()
        {

        }
        public RegisterRequestException(string message): base(message)
        {

        }
    }
}
