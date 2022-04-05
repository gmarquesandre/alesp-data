using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared
{
    public class CongressPerson
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int AlespId { get; set; }
        public List<Legislature>? Legislatures { get; set; }
    }
}