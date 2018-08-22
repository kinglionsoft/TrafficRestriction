#if NETSTANDARD2_0
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
#endif
using System;
using System.Threading.Tasks;

namespace K9Soft.Utils.TrafficRestriction
{
    public abstract class TrafficRestrictionService : ITrafficRestrictionService
    {
#if NETSTANDARD2_0
        private readonly IMemoryCache _cache;
        private readonly TrafficRestrictionOption _option;

        protected TrafficRestrictionService(IMemoryCache cache, IOptions<TrafficRestrictionOption> optionAccessor)
        {
            _cache = cache;
            _option = optionAccessor.Value;
        }
#endif
        public virtual async Task<string[]> GetAsync(string cityName)
        {
#if NETSTANDARD2_0
            if (_option.UseCache)
            {
                var key = "__traffic_" + cityName;
                var result = _cache.Get<string[]>(key);
                if (result == null)
                {
                    result = await GetWithoutCacheAsync(cityName);

                    _cache.Set(key, result, new DateTimeOffset(DateTime.Today.AddDays(1)));
                }

                return result;
            }
#endif
            return await GetWithoutCacheAsync(cityName);
        }

        protected abstract Task<string[]> GetWithoutCacheAsync(string cityName);
    }

}