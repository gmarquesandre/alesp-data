using Alesp.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared
{
    public class Presence
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public CongressPerson CongressPerson { get; set; }  
        public int CongressPersonId { get; set; }
        public ELegislativeSessionType LegislativeSessionType { get; set; }
        public EPresenceStatus PresenceStatus { get; set; }
    }
}