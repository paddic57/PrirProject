using CryptoAppv3.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppv3.Service
{
    public interface IBinanceService
    {
        Task<ChartDataBinance> getChartData(string symbol = "BTCUSDT", string limit = "20");
    }
}
