using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared.Enums
{
    public enum ELegislativeSessionType
    {
        [Display(Name = "O - Sessão Ordinária")]
        Ordinary = 0,
        [Display(Name = "E - Sessão Extraordinária")]
        Extraordinary = 1,        
        [Display(Name = "R - Reunião")]
        Meeting = 2,
        [Display(Name = "I - Inaugural")]
        Inaugural = 3,
        [Display(Name = "PI - Preparatória Inaugural")]
        PreInaugural = 4,
        [Display(Name = "PI - Preparatória Inaugural")]
        Unknown = 99
    }
}