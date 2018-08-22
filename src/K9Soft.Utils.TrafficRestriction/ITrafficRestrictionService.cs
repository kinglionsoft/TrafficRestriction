using System.Threading.Tasks;

namespace K9Soft.Utils.TrafficRestriction
{
    public interface ITrafficRestrictionService
    {
        /// <summary>
        /// Get today's restriction of the specific city
        /// </summary>
        /// <param name="cityName">Name of city</param>
        /// <returns>The last number of plates that are restricted today.</returns>
        Task<string[]> GetAsync(string cityName);
    }
}
