namespace NextMindBackEnd.Data.Models
{
    public class ControlDataFromClient
    {
        // ids if control or iftttkey exists
        public int? ControlID { get; set; }
        public int? IFTTTKeyID { get; set; }
        //data if these does not exists
        public string? ControlName { get; set; }
        public string? URL { get; set; }
        public string? IFTTTKey { get; set; } 


    }
}
