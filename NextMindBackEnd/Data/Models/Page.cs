using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextMindBackEnd.Data.Models
{
    public class Page
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        [Required]
        public int Index { get; set; }
        public ICollection<PageControl> PageControls { get; set; } 
        public RemoteController RemoteController { get; set; }

    }
}
