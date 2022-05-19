namespace NextMindBackEnd.Data.Exceptions
{
    public class GetControlsException: Exception
    {
        public int Code;
        public GetControlsException()
        {

        }
        public GetControlsException(string message, int code): base(message)
        {
            Code = code;
        }
    }
}
