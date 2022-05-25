namespace NextMindBackEnd.Data.Models
{
    public class ControlToClient
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string  URL { get; set; } = string.Empty;
        public Key? IFTTTKey { get; set; }
    }
}
