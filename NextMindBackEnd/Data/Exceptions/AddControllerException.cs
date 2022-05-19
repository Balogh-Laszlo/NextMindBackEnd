
namespace NextMindBackEnd.Data.Exceptions
{
    public class AddControllerException: Exception
    {
        public int Code { get; set; }
        public AddControllerException()
        {

        }
        public AddControllerException(string message, int code): base(message)
        {
            Code = code;
        }
    }
}
