using System;
using Xunit;
using K9Soft.Utils.TrafficRestriction;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace TrafficRestrictionTest
{
    public class UnitTest1
    {
        private async Task<string[]> GetNumbers(string cityName)
        {
            var baidu = new BaiduTrafficRestrictionService(null, 
                                    Options.Create(new TrafficRestrictionOption{ UseCache = false}));
            var numbers = await baidu.GetAsync(cityName);
            return numbers;
        }

        [Fact]
        public async Task TestCity()
        {
            // 今天是周三
            var map = new Dictionary<string, string[]>
            {
                { "成都",  new []{"3", "8"}},
                { "北京",  new []{"5", "0"}},
                { "天津",  new []{"5", "0"}},
                { "兰州",  new []{"2", "7"}},
                { "武汉",  new []{"单号"}},
                { "自贡",  new string[0]}
            };

            foreach (var item in map)
            {
                Assert.Equal(item.Value, await GetNumbers(item.Key));
            }
        }
    }
}
