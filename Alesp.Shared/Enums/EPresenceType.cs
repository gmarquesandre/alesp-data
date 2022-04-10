using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared.Enums
{
    public enum EPresenceStatus
    {


        [Display(Name = "P - Presente")]
        Present = 1,

        [Display(Name = "I - Indefinida")]
        Undefined = 2,
        
        [Display(Name = "J - Justificada nos termos do art. 90, §1º, \"2\", XIV RI")]
        JustifiedAbscence = 3,

        [Display(Name = "N - Não justificada")]
        NotJustifiedAbscence = 4,

        [Display(Name = "AE: Ausência em Sessão Extraordinária")]
        ExtraordinarySessionAbscence= 5,
        
        [Display(Name = "CQR: Com quórum regimental para abertura da sessão[2]")]
        WithRegimentalQuorum = 6,
        
        [Display(Name = "SP: A serviço do Poder Legislativo (art. 90, § 3º, XIV RI)[3]")]
        AbscenceOnLegislativeService = 7,
        
        [Display(Name = "C: Em Comissão de Representação ou CPI (art. 90, §1º, \"1\", XIV RI)[3]")]
        CommissionAttendance = 8,
        
        [Display(Name = "A: Em Audiência Pública")]
        PublicHearing = 9,
        
        [Display(Name = "LS: Licença Saúde (art. 84, II, XIV RI)[3]")]
        HealthLicense = 10,
        
        [Display(Name = "LIP: Licença de Interesse Particular (art. 84, III, XIV RI)[3]")]
        PrivateIssuesLicense = 11,
        
        [Display(Name = "LG: Licença Gestante (art. 84, § 1º, XIV RI)[3]")]
        PregnantLicense  = 12,
        
        [Display(Name = "LP: Licença Paternidade")]
        PaternityLeave = 13,
        
        [Display(Name = "LMC: Licença para Missão Diplomática ou Cultural (art. 84, I, XIV RI)[3]")]
        DiplomaticOrCulturalLicense = 14,
        
        [Display(Name = "D: Dispensado(a) nos termos do artigo 8º do Ato da Mesa nº 3, de 17 de março de 2020, e nos termos do artigo 7º, § 4º, do Ato do Presidente nº 52, de 30 de julho de 2020.")]
        Covid = 15,

    }
}