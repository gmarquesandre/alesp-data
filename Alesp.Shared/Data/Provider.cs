using Alesp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(14)]
        public string Identification { get; set; }
        public EProviderIdentificationType IdentificationType { get;set;}
        public string? Name { get; set; }
        public decimal ShareCapital { get; set; }
    }
}