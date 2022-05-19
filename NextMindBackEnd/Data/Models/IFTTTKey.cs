using NextMindBackEnd.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextMindBackEnd.Data.Models
{
    public class IftttKey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Key { get; set; }
        public ICollection<Control> Controls { get; set; }

    }
}
