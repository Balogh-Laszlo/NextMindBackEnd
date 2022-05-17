namespace NextMindBackEnd.Data.Models
{
    public class PageControl
    {
        public int ControlID { get; set; }
        public int PageID { get; set; } 
        public Control Control { get; set; }
        public Page Page { get; set; }
    }
}
