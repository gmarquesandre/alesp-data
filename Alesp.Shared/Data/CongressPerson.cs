using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared
{
    public class CongressPerson
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? RegionDescription { get; set; }
        public string? AreasOfWork { get; set; }
        public int AlespId { get; set; }
        public virtual ICollection<Legislature> Legislatures { get; set; }
    }
}