namespace NextMindBackEnd.Data.Models
{
    public class ControlToClient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string  URL { get; set; }
        public string IFTTTKey { get; set; } = String.Empty;
    }
}
