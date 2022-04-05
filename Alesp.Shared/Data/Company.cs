using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(14)]
        public int CNPJ { get; set; }
        public string? CompanyName { get; set; }
        public decimal ShareCapital { get; set; }
        public List<Legislature>? Legislatures { get; set; }
    }
}