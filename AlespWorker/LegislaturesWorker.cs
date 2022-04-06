using Alesp.Shared;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Alesp.Worker
{
    public class LegislaturesWorker
    {

        private HttpClient Client = new();
        private HtmlParser Parser = new();
        //private AlespDbContext _context;
        //public LegislaturesWorker(AlespDbContext context)
        //{
        //    _context = context;
        //}
        public async Task GetLesgilatures()
        {

            HttpResponseMessage response = await Client.GetAsync("https://www.al.sp.gov.br/deputado/legislaturas/");
            string responseString = await response.Content.ReadAsStringAsync();
            var document = await Parser.ParseDocumentAsync(responseString);
            var legislaturesDocuments = document.QuerySelectorAll("select[name = 'idLegislatura'] option");


            foreach(var legislature in legislaturesDocuments.Skip(1).Reverse())
            {

                var newLegislature = GetLegislature(legislature);

                TryToAddLegislatureAsync(newLegislature);

            }   

        }
        private void TryToAddLegislatureAsync(Legislature newLegislature)
        {
            using var _context = new AlespDbContext();

            var legislatureFind = _context.Legislatures.FirstOrDefault(a => a.Number == newLegislature.Number);

            if (legislatureFind == null)
                _context.Legislatures.Add(newLegislature);


            _context.SaveChanges();
        }
        private Legislature GetLegislature(IElement legislature)
        {
            string textLegislature = legislature.TextContent;

            int legislatureNumber = Convert.ToInt32(legislature.GetAttribute("value"));

            int yearStart = Convert.ToInt32(textLegislature.Substring(textLegislature.Length - 12, 4));
            int yearEnd = Convert.ToInt32(textLegislature.Substring(textLegislature.Length - 5, 4));

            int day = 15;
            int month = 3;

            //https://politica.estadao.com.br/noticias/geral,assembleia-de-sao-paulo-altera-data-de-posse-dos-deputados-para-1-de-fevereiro,70002756003
            if (yearStart >= 2027)
            {
                day = 1;
                month = 2;
            }

            DateTime startDate = new DateTime(yearStart, month, day);
            DateTime endDate = new DateTime(yearEnd, month, day);

            Legislature newLegislature = new()
            {
                Number = legislatureNumber,
                EndDate = endDate,
                StartDate = startDate,

            };

            return newLegislature;
            
        }
    }
}
