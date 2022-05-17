namespace NextMindBackEnd.Data.Exceptions
{
    public class AddKeyException: Exception
    {
        public int ErrorCode { get; set; }
        public AddKeyException()
        {

        }
        public AddKeyException(string message, int code): base(message)
        {
            ErrorCode = code;
        }
    }
}
