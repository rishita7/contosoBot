using contosoBot.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace contosoBot.Sockservices
{
    public class Stock
    {
        public static async Task<StockItem> GetStockPrice(string symbol)
        {
            string uri =$"http://dev.markitondemand.com/Api/v2/Quote/json?symbol={symbol}";
            var StockItem = new StockItem();
            using (var client = new WebClient())
            {
                var rawData = await client.DownloadStringTaskAsync(new Uri(uri));
                // we need to convert this into a stock Item
                StockItem = JsonConvert.DeserializeObject<StockItem>(rawData);
                StockItem.Status = StockItem.Status ?? "FAIL"; // when I send a symbol that dose'nt exisit
            }
            return StockItem;
        }
    }
}