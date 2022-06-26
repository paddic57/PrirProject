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
        public async Task<ChartDataBinance> getChartData(string symbol = "BTCUSDT", string limit = "20")
        {
            Uri uri = new Uri("https://www.binance.com/api/v3/depth?symbol=" + symbol + "&limit=" + limit);
            WebClient client = new WebClient();
            string dataFromApi = await client.DownloadStringTaskAsync(uri);
            var data = JsonSerializer.Deserialize<ChartDataBinance>(dataFromApi);

            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            foreach (var stringList in data.asks)
            {
                data.asksPrices.Add(System.Convert.ToDouble(stringList[0], provider).ToString("C", new CultureInfo("en-US")));
                data.asksQuantity.Add(Math.Round(System.Convert.ToDouble(stringList[1], provider), 2));
            }
            foreach (var stringList in data.bids)
            {
                data.bidsPrices.Add(System.Convert.ToDouble(stringList[0], provider).ToString("C", new CultureInfo("en-US")));
                data.bidsQuantity.Add(Math.Round(System.Convert.ToDouble(stringList[1], provider),2 ));
            }

            return data;
        }
    }
}
