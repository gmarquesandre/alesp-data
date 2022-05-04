using Alesp.Shared;
using Alesp.Shared.Enums;
using Alesp.Shared.Validators;
using Alesp.Worker;
using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

internal class SpendingWorker
{

    private HttpClient Client = new();
    private HtmlParser Parser = new();
    private CultureInfo _cultureInfo = new("pt-br");
    private AlespDbContext _context = new AlespDbContext();
    private ProviderWorker _providerWorker = new ProviderWorker();

    public SpendingWorker()
    {
    }

    public async Task GetAllSpendings()
    {

        var congressPeople = _context.CongressPeople
            .Include(a => a.Legislatures.Where(a=> a.StartDate >= new DateTime(2002,1,1)))
            .Where(b => b.Legislatures.Any(a => a.StartDate >= new DateTime(2002, 1, 1)))
            .ToList();


        congressPeople = congressPeople.Where(a => a.Legislatures.Count > 0).ToList();

        await GetSpendingWithCongressPeopleList(congressPeople);

    }
    public async Task GetCongressPersonAllSpendingsWithId(int id)
    {
        var congressPerson = _context.CongressPeople.Include(a => a.Legislatures).FirstOrDefault(a => a.Id == id);

        if (congressPerson == null)
        {
            throw new Exception("Congress Person was not found");
        }
        await GetSpendingWithCongressPersonAndLegislatures(congressPerson);
    }
    public async Task GetCongressPersonAllSpendingsWithAlespId(int alespId)
    {
        var congressPerson = _context.CongressPeople.Include(a => a.Legislatures).FirstOrDefault(a => a.AlespId == alespId);

        if (congressPerson == null)
        {
            throw new Exception("Congress Person was not found");
        }
        await GetSpendingWithCongressPersonAndLegislatures(congressPerson);
    }
    public async Task GetCongressPersonLegislatureSpendings(int id, int legislatureNumber)
    {
        var congressPerson = await _context.CongressPeople.Include(a => a.Legislatures).FirstOrDefaultAsync(a => a.Id == id && a.Legislatures.Any(a => a.Number == legislatureNumber));

        if (congressPerson == null)
            throw new Exception($"Congress Person was not found on legislature {legislatureNumber}");

        await GetSpendingWithCongressPersonAndLegislatures(congressPerson);

    }
    private async Task GetSpendingWithCongressPeopleList(List<CongressPerson> congressPeople)
    {
        int count = 0;
        foreach (var congressPerson in congressPeople)
        {
            try
            {
                count++;
                Console.WriteLine($"{count}/{congressPeople.Count}");

                await GetSpendingWithCongressPersonAndLegislatures(congressPerson);
            }
            catch(HttpRequestException ex)
            {

                Console.WriteLine("Error " + ex.Message);
                Thread.Sleep(30000);
                await GetSpendingWithCongressPeopleList(congressPeople.Skip(count - 1).ToList());

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                throw new Exception($"Erro ao buscar dados do parlamentar Id Interno: {congressPerson.Id}, Nome {congressPerson.Name}");
            }

        }
    }
    private async Task GetSpendingWithCongressPersonAndLegislatures(CongressPerson congressPerson)
    {
        var lastMonth = congressPerson.Legislatures.Select(a => a.EndDate).Max();
        foreach (var legislature in congressPerson.Legislatures)
        {
          
            var listDatesSpending = _context.Spendings.Where(a => a.CongressPersonId == congressPerson.Id).Select(b => b.Date).Distinct().ToList();
            DateTime lastSpendingDate = listDatesSpending.DefaultIfEmpty(new DateTime(1900, 1, 1)).Max().AddMonths(-1);
            

            int i = 0;
            DateTime startDate = legislature.StartDate;
            DateTime endTime = legislature.EndDate;

            while (startDate.AddMonths(i) <= endTime && startDate.AddMonths(i) < DateTime.Now)
            {

                DateTime dateRef = startDate.AddMonths(i);
                                
                if (dateRef < lastSpendingDate)
                {
                    i++;
                    continue;
                }

                Console.WriteLine($"Month {dateRef}, Last {lastMonth}");

                var response = await Client.GetAsync($"https://www.al.sp.gov.br/deputado/contas/?matricula={congressPerson.AlespId}&mes={dateRef.Month}&ano={dateRef.Year}&cnpjOuCpf=&tipo=naturezas");
                string responseString = await response.Content.ReadAsStringAsync();
                var document = await Parser.ParseDocumentAsync(responseString);

                var rowsSpending = document.QuerySelectorAll(".tabel,.sortable tbody tr");


                foreach (var row in rowsSpending)
                {
                    string url = row.QuerySelectorAll("td a")[1].GetAttribute("href")!.ToString();

                    response = await Client.GetAsync($"https://www.al.sp.gov.br{url}");
                    responseString = await response.Content.ReadAsStringAsync();
                    document = await Parser.ParseDocumentAsync(responseString);

                    var rowsSpendingCompanies = document.QuerySelectorAll(".tabel,.sortable tbody tr");

                    foreach (var rowCompany in rowsSpendingCompanies)
                    {
                        int type = Convert.ToInt32(new Regex("(?<=idTipo=)(.*?)(?=&)").Match(url).ToString());
                        await GetInfoAndInsertOrUpdateInfo(rowCompany, type, dateRef, congressPerson);
                    }
                }
                i++;
            }
        }
    }
    private async Task GetInfoAndInsertOrUpdateInfo(AngleSharp.Dom.IElement rowCompany, int type, DateTime dateRef, CongressPerson congressPerson)
    {
        string name = rowCompany.QuerySelectorAll("td a")[0].TextContent.Replace("\n", "").Replace("\t", "").Trim();
        string identification = rowCompany.QuerySelectorAll("td a")[1].TextContent.Replace("\n", "").Replace("\t", "").Replace("/", "").Replace("-", "").Replace(".", "").Trim();
        decimal value = Convert.ToDecimal(rowCompany.QuerySelectorAll("td span")[0].TextContent.Replace("\n", "").Replace("\t", "").Trim(), _cultureInfo);       

        var newSpending = new Spending()
        {
            Date = new DateTime(dateRef.Year, dateRef.Month, 1),
            Type = (ESpendingType)type,
            Value = value,
            //CongressPerson = congressPerson,
            CongressPersonId = congressPerson.Id
        };


        var spending = _context.Spendings
            .Include(a => a.Provider)
            .FirstOrDefault(
                a => a.Type == newSpending.Type &&
                a.Date == newSpending.Date &&
                a.CongressPersonId == congressPerson.Id && a.Provider.Identification == identification
            );

        if (spending == null)
        {
            await InsertNewSpending(newSpending, identification, name);
        }

        
    }
    private async Task InsertNewSpending(Spending newSpending, string identification, string providerName)
    {
        Provider companyObj = new()
        {
            Identification = identification,
            Name = providerName,
            IdentificationType = GetIdentificationType(identification),            
        };

        var company = await _providerWorker.InsertOrUpdateProviderAndReturn(companyObj);

        newSpending.ProviderId = company.Id;
        await _context.Spendings.AddAsync(newSpending);

        await _context.SaveChangesAsync();
    }    
    private static EProviderIdentificationType GetIdentificationType(string identification)
    {
        IdentificationValidator validator = new IdentificationValidator();
        try
        {
            if (validator.IsCnpj(identification))
            {
                return EProviderIdentificationType.Cnpj;
            }
            else if (validator.IsCpf(identification))
            {
                return EProviderIdentificationType.Cpf;
            }

            Console.WriteLine($"{identification} não é CPF nem CNPJ");
            return EProviderIdentificationType.Unknown;
        }
        catch {
            return EProviderIdentificationType.Unknown;
        }
    }
}