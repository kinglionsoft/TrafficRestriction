using AngleSharp;
using AngleSharp.Network.Default;
#if NETSTANDARD2_0
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
#endif
using System;
using System.Net;
using System.Threading.Tasks;

namespace K9Soft.Utils.TrafficRestriction
{
    public class BaiduTrafficRestrictionService : TrafficRestrictionService
    {
        private IConfiguration _config;

        public IConfiguration Config
        {
            get
            {
                if (_config == null)
                {
                    var requester = new HttpRequester();
                    requester.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.106 Safari/537.36";
                    requester.Headers["Accept-Language"] = "zh-CN,zh;q=0.9";
                    _config = Configuration.Default.WithDefaultLoader(requesters: new[] { requester });
                }
                return _config;
            }
        }
#if NETSTANDARD2_0
        public BaiduTrafficRestrictionService(IMemoryCache cache, IOptions<TrafficRestrictionOption> optionAccessor) : base(cache, optionAccessor)
        {
        }
#endif

        protected override async Task<string[]> GetWithoutCacheAsync(string cityName)
        {
            var address = "https://www.baidu.com/s?wd=" + WebUtility.UrlEncode(cityName + " 车辆限行");
            var document = await BrowsingContext.New(Config).OpenAsync(address);
            var selector = "div[tpl='traffic'].result-op.c-container .op_traffic_time div:nth-child(1)";
            var element = document.QuerySelector(selector);
            if (element == null)
            {
                return new string[0];
            }
            var date = element.QuerySelector(".op_traffic_title").TextContent;
            if (date.StartsWith("今天"))
            {
                var textEle = element.QuerySelector(".op_traffic_txt") ?? element.QuerySelector(".op_traffic_off");
                if (textEle == null)
                {
                    return new string[0];
                }
                var numbers = textEle.TextContent;
                if (numbers == "单号" || numbers == "双号" || numbers == "不限行")
                {
                    return new[] { numbers };
                }
                return numbers.Split(new[] { '和', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }


    }
}