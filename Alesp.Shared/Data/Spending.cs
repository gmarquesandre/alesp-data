using Alesp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared
{
    public class Spending
    {
        [Key]
        public int Id { get; set; }
        public Company Company { get; set; }
        public decimal Value { get; set; }
        public DateTime Date { get; set; }
        public CongressPerson CongressPerson { get; set; }
        public ESpendingType Type { get; set; }
    }
}
