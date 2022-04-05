using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;

internal class SpendingWorker
{
    
    private HttpClient Client = new();
    private HtmlParser Parser = new();
    public SpendingWorker()
    {
    }

    public async Task GetSpendings()
    {

        using var _context = new AlespDbContext();

        var congressPeople = _context.CongressPeople.Include(a => a.Legislatures).ToList();

        foreach(var congressPerson in congressPeople)
        {
            foreach(var legislature in congressPerson.Legislatures)
            {
                int i = 0;
                DateTime startDate = legislature.StartDate;
                DateTime endTime = legislature.EndDate;

                while(startDate.AddMonths(i) >= endTime)
                {
                    DateTime dateRef = startDate.AddMonths(i);
                    
                    var response = await Client.GetAsync($"https://www.al.sp.gov.br/deputado/contas/?matricula={congressPerson.AlespId}&mes={dateRef.Month}&ano={dateRef.Year}&cnpjOuCpf=&tipo=naturezas");
                    string responseString = await response.Content.ReadAsStringAsync();
                    var document = await Parser.ParseDocumentAsync(responseString);
                 
                    

                    i++;
                }
            }
        }
    }
}