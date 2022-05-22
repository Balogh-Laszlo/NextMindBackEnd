namespace NextMindBackEnd.Data.Exceptions
{
    public class GetRemoteControllersException: Exception
    {
        public int Code;
        public GetRemoteControllersException()
        {

        }
        public GetRemoteControllersException(string message, int code): base(message)
        {
            this.Code = code;
        }
    }
}
