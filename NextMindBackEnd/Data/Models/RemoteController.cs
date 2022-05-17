
using NextMindBackEnd.Models;

namespace NextMindBackEnd.Data.Models
{
    public class RemoteController
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Page> Pages { get; set; }
        public User User { get; set; }  
    }
}
