using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared
{
    public class Legislature
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<CongressPerson> CongressPeople { get; set; }
    }
}