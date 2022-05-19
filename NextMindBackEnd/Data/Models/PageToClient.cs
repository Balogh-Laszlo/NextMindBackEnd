namespace NextMindBackEnd.Data.Models
{
    public class PageToClient
    {
        public int Id { get; set; }
        public int Index { get; set; }
        public List<ControlToClient> Controls { get; set; }
    }
}
