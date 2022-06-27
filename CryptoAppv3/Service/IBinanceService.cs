using CryptoAppv3.Models;
using System.Threading.Tasks;

namespace CryptoAppv3.Service
{
    public interface IBinanceService
    {
        Task<ChartDataBinance> getChartData(string symbol = "BTCUSDT", string limit = "200");
    }
}
