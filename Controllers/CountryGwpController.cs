using Galytix.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galytix.Controllers
{
    [Route("api/countrygwp")]
    [ApiController]
    public class CountryGwpController : ControllerBase
    {
        private readonly CountryGwpService countryGwpService;

        public CountryGwpController(CountryGwpService countryGwpService)
        {
            this.countryGwpService = countryGwpService;
        }


        [HttpPost("avg/country/{country}")]
        public async Task<IActionResult> GetAverageGwp([FromRoute]string country, [FromBody]IEnumerable<string> businesses)
        {
            var avgGwp = await countryGwpService.GetAverageGwp(country, businesses);
            Dictionary<string, double> businessGwpMap = new Dictionary<string, double>();
            foreach(var item in avgGwp)
            {
                businessGwpMap.Add(item.Business, item.AvgGwp);
            }

            if(avgGwp.Count() == 0)
            {
                return NotFound($"No results found for country: {country}");
            }

            return Ok(businessGwpMap);
        }
    }
}
