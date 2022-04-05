using System.ComponentModel.DataAnnotations;

namespace Alesp.Shared.Enums
{
    public enum ESpendingType
    {


        [Display(Name = "A - COMBUSTÍVEIS E LUBRIFICANTES")]
        Gas = 2,

        [Display(Name = "C - MATERIAIS E SERVIÇOS DE MANUT E CONSERV DE VEÍCULOS; PEDÁGIOS")]
        VehiculeSpending = 4,

        [Display(Name = "D - MATERIAIS E SERVIÇOS GRÁFICOS, DE CÓPIAS E REPRODUÇÃO DE DOCS")]
        DocumentsPrinting = 5,

        [Display(Name = "E - MATERIAIS DE ESCRITÓRIO E OUTROS MATERIAIS DE CONSUMO")]
        OfficeSpending = 6,

        [Display(Name = "F - SERVIÇOS TÉCNICOS PROFISSIONAIS (CONSULTORIA, PESQUISAS ETC)")]
        ConsultingSpending = 7,

        [Display(Name = "G - ASSINATURAS DE PERIÓDICOS, PUBLICAÇÕES, INTERNET E SOFTWARES")]
        JournalSubscription = 8,

        [Display(Name = "H - SERV.UTIL.PÚBLICA (TELEF.MÓVEL/FIXA, ENERGIA, ÁGUA, GÁS ETC)")]
        MonthlySpending = 9,

        [Display(Name = "I - HOSPEDAGEM, ALIMENTAÇÃO E DESPESAS DE LOCOMOÇÃO")]
        TravelingSpending = 11,

        [Display(Name = "J - SERVIÇOS DE COMUNICAÇÃO")]
        CommunicationSpending = 10,

        [Display(Name = "K - LOCAÇÃO DE BENS MÓVEIS")]
        OtherTypesOfRent = 12,

        [Display(Name = "L - LOCAÇÃO DE BENS IMÓVEIS")]
        RealStateRent = 13,

        [Display(Name = "M - MANUTENÇÃO DE BENS MÓVEIS, IMÓVEIS, CONDOMÍNIOS E OUTROS")]
        RealStateSpending = 14,

        [Display(Name = "N - MORADIA")]
        HouseSpending = 15,

        [Display(Name = "O - LOCAÇÃO DE VEÍCULO")]
        VehicleRent = 20,

        [Display(Name = "P - DIVULGAÇÃO DA ATIVIDADE PARLAMENTAR")]
        Marketing = 21,
    }
}