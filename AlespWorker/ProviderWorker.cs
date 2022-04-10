using Alesp.Shared;
using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Alesp.Worker
{
    public class ProviderWorker
    {

      
        private AlespDbContext _context = new AlespDbContext();
        private HttpClient Client = new();
        private HtmlParser Parser = new();
        private CultureInfo _cultureInfo = new("pt-br");


        public ProviderWorker()
        {

        }
        public async Task<Provider> InsertOrUpdateProviderAndReturn(Provider companyObj)
        {
            var company = _context.Providers.FirstOrDefault(a => a.Identification == companyObj.Identification);

            if (company != null)
            {
                return company;
            }
            await _context.Providers.AddAsync(companyObj);
            await _context.SaveChangesAsync();
            company = await _context.Providers.FirstOrDefaultAsync(a => a.Identification == companyObj.Identification);

            return company!;

        }
        
    }
}
