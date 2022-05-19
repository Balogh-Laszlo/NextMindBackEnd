using NextMindBackEnd.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextMindBackEnd.Data.Models
{
    public class Control
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string URL { get; set; } = string.Empty;
        public ICollection<PageControl> PageControls { get; set; }
        [Required]
        public IftttKey IftttKey { get; set; }
        public int IftttKeyId { get; set; }
    }
}
