using Alesp.Shared;
using AngleSharp.Html.Parser;

public class CongressPersonWorker
{


    private HttpClient Client = new();
    private HtmlParser Parser = new();
    public AlespDbContext _context = new AlespDbContext();

    public CongressPersonWorker()
    {

    }


    public async Task GetCongressPeople()
    {

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

        await GetCongressPersonInfo();



    }

    public async Task GetCongressPersonInfo()
    {
        var congressPeople = _context.CongressPeople.ToList();
        int count = 0;
        foreach(var congressPerson in congressPeople)
        {
            try
            {
                count++;
                Console.WriteLine($"{count} de {congressPeople.Count}");
                var response = await Client.GetAsync($"https://www.al.sp.gov.br/deputado/?matricula={congressPerson.AlespId}");
                var document = Parser.ParseDocument(await response.Content.ReadAsStringAsync());

                var imageElement = document!.QuerySelector(".img-deputado,.img-thumbnail")!;
                if (imageElement != null)
                {
                    string urlImage = imageElement.GetAttribute("src")!;

                    var responseImage = await Client.GetAsync(urlImage);
                    string imageBase64 = Convert.ToBase64String(await responseImage.Content.ReadAsByteArrayAsync());
                    congressPerson.PictureBase64 = imageBase64;

                }


                var queryInfos = document.QuerySelectorAll("#infoGeral div");
                if (queryInfos.Length > 0)
                {

                    foreach(var queryInfo in queryInfos)
                    {
                        if(queryInfo!.QuerySelector("label")!.InnerHtml.Contains("Base Eleitoral"))
                            congressPerson.RegionDescription = queryInfo!.QuerySelector("textArea")!.TextContent;
                        
                        else if(queryInfo!.QuerySelector("label")!.InnerHtml.Contains("Área de Atuação"))
                            congressPerson.AreasOfWork = queryInfo!.QuerySelector("textArea")!.TextContent;

                        else if (queryInfo!.QuerySelector("label")!.InnerHtml.Contains("E-mail"))
                        {
                            if (queryInfo!.QuerySelector("input") != null && queryInfo!.QuerySelector("input")!.GetAttribute("value").Contains("@"))
                                congressPerson.Email = queryInfo!.QuerySelector("input")!.GetAttribute("value").ToLower();
                            else
                                congressPerson.Email = "N/D";

                        }


                    }

                    congressPerson.Biography = document!.QuerySelector(".col-md-12")!.InnerHtml;




                    _context.CongressPeople.Update(congressPerson);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            


        }
    }
}