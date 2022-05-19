namespace NextMindBackEnd.Data.Models
{
    public class ControllerDataToClient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PageToClient> Pages { get; set; }
    }
}
