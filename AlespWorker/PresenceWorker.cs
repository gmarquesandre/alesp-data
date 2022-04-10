using Alesp.Shared;
using Alesp.Shared.Enums;
using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Alesp.Worker
{
    public class PresenceWorker
    {


        private AlespDbContext _context = new AlespDbContext();
        private HttpClient _client = new();
        private HtmlParser _parser = new();
        private CultureInfo _cultureInfo = new("pt-br");


        public async Task GetAllCongressPeoplePresence()
        {

            var congressPeople = _context.CongressPeople
                .Include(a => a.Legislatures.Where(a => a.StartDate >= new DateTime(2011, 1, 1)))
                .Where(b => b.Legislatures.Any(a => a.StartDate >= new DateTime(2011, 1, 1)))
                .ToList();


            congressPeople = congressPeople.Where(a => a.Legislatures.Count > 0).ToList();

            await GetPresenceCongressPeopleList(congressPeople);



        }

        private async Task GetPresenceCongressPeopleList(List<CongressPerson> congressPeople)
        {
            var response = await _client.GetAsync("https://www.al.sp.gov.br/sigp-portal/default.xhtml");

            var document = await _parser.ParseDocumentAsync(await response.Content.ReadAsStringAsync());

            var listCongressPresence = document.QuerySelectorAll("#formFiltros\\:listaDeputados option");
            foreach (var congressPerson in congressPeople)
            {

                //await _client.GetAsync("https://www.al.sp.gov.br/alesp/presenca-plenario/");
                string valueSearch = "";
                foreach (var rowCongressPresence in listCongressPresence.Skip(1))
                {
                    if (rowCongressPresence.TextContent.ToLower() == congressPerson.Name.ToLower())
                    {
                        valueSearch = rowCongressPresence.GetAttribute("value")!;
                        break;
                    }

                }
                if (valueSearch == "")
                {
                    Console.WriteLine($"Deputado(a) {congressPerson.Name}, Id Interno {congressPerson.Id}, não foi encontrado na lista de presenças da Alesp");
                    continue;
                }

                Console.WriteLine($"Deputado(a) {congressPerson.Name} possui o código {valueSearch}");
                foreach (var legislature in congressPerson.Legislatures)
                {
                    DateTime startDate = legislature.StartDate;
                    DateTime endTime = legislature.EndDate;
                    int i = 0;



                    while (startDate.AddMonths(i) <= endTime && startDate.AddMonths(i) < DateTime.Now)
                    {

                        DateTime dateRef = startDate.AddMonths(i);
                        document = await _parser.ParseDocumentAsync(await response.Content.ReadAsStringAsync());

                        string viewState = document!.QuerySelector("input[name='javax.faces.ViewState']")!.GetAttribute("value")!;

                        var form = new List<KeyValuePair<string, string>>
                        {
                             new KeyValuePair<string, string>("formFiltros","formFiltros"),
                             new KeyValuePair<string, string>("formFiltros:listaDeputados",valueSearch),
                             new KeyValuePair<string, string>("formFiltros:mesAnoSessao",dateRef.ToString("M/yyyy")),
                             new KeyValuePair<string, string>("formFiltros:j_idt17","Filtrar"),
                             new KeyValuePair<string, string>("javax.faces.ViewState",viewState),
                        };
                        response = await _client.PostAsync("https://www.al.sp.gov.br/sigp-portal/default.xhtml", new FormUrlEncodedContent(form.ToArray()));
                        document = await _parser.ParseDocumentAsync(await response.Content.ReadAsStringAsync());

                        var presenceInfos = document.QuerySelectorAll("td[rowspan='3']");

                        foreach (var presenceInfo in presenceInfos)
                        {
                            var infos = presenceInfo.QuerySelectorAll("table tbody tr td label");
                            int day = Int32.Parse(infos[0].TextContent);
                            string sessionType = infos[1].TextContent;
                            string presence = infos[2].TextContent;

                            var newPresence = new Presence()
                            {
                                CongressPersonId = congressPerson.Id,
                                Date = new DateTime(startDate.Year, startDate.Month, day),
                                LegislativeSessionType = SessionDictionary[sessionType],
                                PresenceStatus = PresenceStatusDictionary[presence]
                            };


                            await InsertOrUpdatePresence(newPresence);

                        }
                    }

                }
            }
        }

        private async Task InsertOrUpdatePresence(Presence newPresence)
        {
            var presence = _context.Presences.FirstOrDefault( a=> a.Date == newPresence.Date && a.CongressPersonId == newPresence.CongressPersonId);

            if(presence == null)
                _context.Presences.Add(newPresence);

        }

        public Dictionary<string, ELegislativeSessionType> SessionDictionary = new Dictionary<string, ELegislativeSessionType>()
        {
            {"O", ELegislativeSessionType.Ordinary},
            {"E", ELegislativeSessionType.Extraordinary},
            {"R", ELegislativeSessionType.Meeting},
            {"I", ELegislativeSessionType.Inaugural},
            {"PI", ELegislativeSessionType.PreInaugural},
        };
        public Dictionary<string, EPresenceStatus> PresenceStatusDictionary = new Dictionary<string, EPresenceStatus>()
        {
            {"P", EPresenceStatus.Present},
            {"I", EPresenceStatus.Undefined},
            {"J", EPresenceStatus.JustifiedAbscence},
            {"N", EPresenceStatus.NotJustifiedAbscence},
            {"AE", EPresenceStatus.ExtraordinarySessionAbscence},
            {"CQR", EPresenceStatus.WithRegimentalQuorum},
            {"SP", EPresenceStatus.AbscenceOnLegislativeService},
            {"C", EPresenceStatus.CommissionAttendance},
            {"A", EPresenceStatus.PublicHearing},
            {"LS", EPresenceStatus.HealthLicense},
            {"LIP", EPresenceStatus.PrivateIssuesLicense},
            {"LG", EPresenceStatus.PregnantLicense},
            {"LP", EPresenceStatus.PaternityLeave},
            {"LMC", EPresenceStatus.DiplomaticOrCulturalLicense},
            {"D", EPresenceStatus.Covid},
        };

    }
}
