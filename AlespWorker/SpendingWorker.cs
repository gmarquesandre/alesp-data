using Alesp.Shared;
using Alesp.Shared.Enums;
using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.RegularExpressions;

internal class SpendingWorker
{
    
    private HttpClient Client = new();
    private HtmlParser Parser = new();
    private CultureInfo _cultureInfo = new("pt-br");
    public SpendingWorker()
    {
    }

    public async Task GetSpendings()
    {

        using var _context = new AlespDbContext();
        
        var congressPeople = _context.CongressPeople
            .Include(a=> a.Legislatures.Where(a=> a.StartDate >= new DateTime(2015,1,1)))
            .Where(a=> a.Name == "Heni Ozi Cukier")
            .ToList();

        foreach(var congressPerson in congressPeople)
        {
            foreach(var legislature in congressPerson.Legislatures)
            {
                int i = 0;
                DateTime startDate = legislature.StartDate;
                DateTime endTime = legislature.EndDate;

                while(startDate.AddMonths(i) <= endTime)
                {
                    DateTime dateRef = startDate.AddMonths(i);
                    
                    var response = await Client.GetAsync($"https://www.al.sp.gov.br/deputado/contas/?matricula={congressPerson.AlespId}&mes={dateRef.Month}&ano={dateRef.Year}&cnpjOuCpf=&tipo=naturezas");
                    string responseString = await response.Content.ReadAsStringAsync();
                    var document = await Parser.ParseDocumentAsync(responseString);

                    var rowsSpending = document.QuerySelectorAll(".tabel,.sortable tbody tr");


                    foreach(var row in rowsSpending)
                    {
                        string url = row.QuerySelectorAll("td a")[1].GetAttribute("href")!.ToString();

                        response = await Client.GetAsync($"https://www.al.sp.gov.br{url}");
                        responseString = await response.Content.ReadAsStringAsync();
                        document = await Parser.ParseDocumentAsync(responseString);


                        var rowsSpendingCompanies = document.QuerySelectorAll(".tabel,.sortable tbody tr");
                        foreach(var rowCompany in rowsSpendingCompanies)
                        {
                            
                            string name = rowCompany.QuerySelectorAll("td a")[0].TextContent.Replace("\n","").Replace("\t", "").Trim();
                            string cnpj = rowCompany.QuerySelectorAll("td a")[1].TextContent.Replace("\n", "").Replace("\t", "").Replace("/","").Replace("-", "").Replace(".", "").Trim();
                            decimal value = Convert.ToDecimal(rowCompany.QuerySelectorAll("td span")[0].TextContent.Replace("\n", "").Replace("\t", "").Trim(), _cultureInfo);
                            
                            if (cnpj.Length < 14)
                                throw new Exception($"Cnpj {cnpj} é inválido");

                            int type = Convert.ToInt32(new Regex("(?<=idTipo=)(.*?)(?>&)").Match(url).ToString());

                            
                            var newSpending = new Spending()
                            {
                                Date = new DateTime(dateRef.Year, dateRef.Month, 1),
                                Type = (ESpendingType)type,
                                Value = value,
                                CongressPerson = congressPerson
                            };

                            var company = _context.Companies.FirstOrDefault(a => a.CNPJ == cnpj);
                            
                            
                            if(company != null)
                            {
                                newSpending.Company = company;

                                _context.Spendings.Add(newSpending);
                            }
                            else
                            {
                                Company newCompany = new()
                                {
                                    CNPJ = cnpj,
                                    CompanyName = name
                                };

                                _context.Companies.Add(newCompany);

                                newSpending.Company = newCompany;


                                _context.Spendings.Add(newSpending);
                            }

                            _context.SaveChanges();
                        }
                    }
                    i++;
                }
            }
        }
    }
}