#if NETSTANDARD2_0

using System;
using K9Soft.Utils.TrafficRestriction;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TrafficExtensions
    {
        public static IServiceCollection AddTrafficRestrictionService(this IServiceCollection services,
            Action<TrafficRestrictionOption> optionBuilder)
        {
            services.Configure<TrafficRestrictionOption>(optionBuilder);
            services.AddSingleton<ITrafficRestrictionService, BaiduTrafficRestrictionService>();
            return services;
        }
    }
}

#endif