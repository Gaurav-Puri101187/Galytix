using Galytix.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Galytix.Domain
{
    public class CountryGwpService
    {
        private readonly CountryGwpRepository countryGwpRepository;
        private readonly IMemoryCache memoryCache;

        public CountryGwpService(
            CountryGwpRepository countryGwpRepository,
            IMemoryCache memoryCache)
        {
            this.countryGwpRepository = countryGwpRepository;
            this.memoryCache = memoryCache;
        }

        /// <summary>
        /// This method will fetch the cache and decide upon the businesses
        /// and will fetch the required data for business which are not cached.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="businesses"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AvgGwpDTO>> GetAverageGwp(string country, IEnumerable<string> businesses)
        {
            var cache = memoryCache.Get<IEnumerable<AvgGwpDTO>>(country);
            List<AvgGwpDTO> avgGwps = new List<AvgGwpDTO>();
            if(cache != null)
            {
                avgGwps.AddRange(cache);
            }
            var nonCachedBusiness = cache != null ? businesses.Except(cache.Select(_ => _.Business)) : businesses;
            if (nonCachedBusiness.Any())
            {
                var data = await countryGwpRepository.GetCountryGwp(country, nonCachedBusiness);
                var nonCachedGwps = data.GroupBy(_ => _.Business).Select(_ => new AvgGwpDTO() { AvgGwp = _.Average(_ => _.Gwp), Business = _.Key });
                avgGwps.AddRange(nonCachedGwps);
                memoryCache.Set(country, nonCachedGwps);
            }
           
            return avgGwps;
        }

        /// <summary>
        /// This method is simpler and more usable and scalable with cavaet that
        /// it goes to db/repo layer if country not found. If businesses are added it won't fetch the latest business 
        /// results. Which ain't good.
        /// </summary>
        /// <param name="country"></param>
        /// <param name="businesses"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AvgGwpDTO>> GetAvgGwpCache(string country, IEnumerable<string> businesses)
        {
            return await memoryCache.GetOrCreateAsync(country, async _ =>
            {
                var data = await countryGwpRepository.GetCountryGwp(country, businesses);
                var avgGwps = data.GroupBy(_ => _.Business).Select(_ => new AvgGwpDTO() { AvgGwp = _.Average(_ => _.Gwp), Business = _.Key });
                return avgGwps;
            });
        }
    }
}
