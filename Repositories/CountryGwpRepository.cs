using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Galytix.Repositories
{
    public class CountryGwpRepository
    {
        // Hold the CSV data in this in memory collection.
        private List<CountryGwpEntity> gwpEntities = new List<CountryGwpEntity>();

        public CountryGwpRepository()
        {
        }

        public async Task<List<CountryGwpEntity>> GetCountryGwp(string country, IEnumerable<string> businesses)
        {
            await FillDataFromCSV();

            var res = gwpEntities.Where(_ =>
            _.Country.Equals(country, StringComparison.OrdinalIgnoreCase) &&
            businesses.Select(_ => _.ToLowerInvariant()).Contains(_.Business.ToLowerInvariant())).ToList();

            return res;
        }

        private async Task FillDataFromCSV()
        {
            if (gwpEntities.Count == 0)
            {
                var csv = await File.ReadAllTextAsync("./Data.csv");
                var data = csv.Split(Environment.NewLine);
                for (int i = 1; i <= data.Length - 2; i++)
                {
                    var columns = data[i].Split(",");
                    var row = new CountryGwpEntity();
                    row.Country = columns[0];
                    row.Year = int.Parse(columns[1]);
                    row.Gwp = double.Parse(columns[2]);
                    row.Business = columns[3];
                    gwpEntities.Add(row);
                }
            }
        }
    }
}
