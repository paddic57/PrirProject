using CryptoAppv3.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoAppv3.Service
{
    public class BinanceService : IBinanceService
    {
        public async Task<ChartDataBinance> getChartData(string symbol, string limit)
        {
            Uri uri = new Uri("https://www.binance.com/api/v3/depth?symbol=" + symbol + "&limit=" + limit);
            WebClient client = new WebClient();
            string dataFromApi = await client.DownloadStringTaskAsync(uri);
            var data = JsonSerializer.Deserialize<ChartDataBinance>(dataFromApi);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            double acumulateBids = 0.0;
            double acumulateAsks = 0.0;
            foreach (var stringList in data.asks)
            {
                data.asksPrices.Add(System.Convert.ToDouble(stringList[0], provider).ToString("C", new CultureInfo("en-US")));
                acumulateAsks += System.Convert.ToDouble(stringList[1], provider);
                data.asksQuantity.Add(Math.Round(acumulateAsks, 2));
            }
            foreach (var stringList in data.bids)
            {
                data.bidsPrices.Add(System.Convert.ToDouble(stringList[0], provider).ToString("C", new CultureInfo("en-US")));
                acumulateBids += System.Convert.ToDouble(stringList[1], provider);
                data.bidsQuantity.Add(Math.Round(acumulateBids, 2 ));
            }
           
            return data;
        }
    }
}
