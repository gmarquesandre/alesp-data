using Alesp.Shared;
using AngleSharp.Html.Parser;

public class CongressPersonWorker
{


    private HttpClient Client = new();
    private HtmlParser Parser = new();

    public CongressPersonWorker()
    {

    }


    public async Task GetCongressPeople()
    {
        using var _context = new AlespDbContext();

        var legislatures = _context.Legislatures.ToList();

        foreach (var legislature in legislatures)
        {
            var response = await Client.GetAsync($"https://www.al.sp.gov.br/deputado/legislaturas/?nomeDeputado=&idLegislatura={legislature.Number}");
            string responseString = await response.Content.ReadAsStringAsync();
            var document = await Parser.ParseDocumentAsync(responseString);

            var listCongressPeople = document.QuerySelectorAll("#deputadoslegislaturas tbody tr");

            foreach (var congressPersonReg in listCongressPeople)
            {
                var congressPersonInfo = congressPersonReg.QuerySelector("td a");
                var congressPersonName = congressPersonInfo!.TextContent;
                var congressPersonAlespId = Convert.ToInt32(congressPersonInfo.GetAttribute("href")!.Split("=")[1].ToString());


                var congressPersonRow = _context.CongressPeople.FirstOrDefault(a => a.AlespId == congressPersonAlespId);

                if (congressPersonRow != null)
                {
                    
                    _context.CongressPeople.Update(congressPersonRow);
                    _context.SaveChanges();
                }
                else
                {


                    var Legislatures = new List<Legislature>() { };

                    Legislatures.Add(legislature);

                    CongressPerson newCongressPerson = new()
                    {

                        AlespId = congressPersonAlespId,
                        Name = congressPersonName,
                        Legislatures = Legislatures

                    };

                    _context.CongressPeople.Add(newCongressPerson);
                    _context.SaveChanges();

                }


            }
        }
    }
}