using CryptoAppv3.Models;
using CryptoAppv3.Service;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppv3.ViewModel
{
    public class OrderBookChartViewModel
    {
        private readonly IBinanceService binanceService;

        public ChartValues<double> bidsChartValuesSeries { get; set; }
        public ChartValues<double> asksChartValuesSeries { get; set; }

        public List<string> bidsLabel { get; set; }
        public List<string> asksLabel { get; set; }

        public OrderBookChartViewModel(IBinanceService binanceService)
        {
            this.binanceService = binanceService;
        }

        public static OrderBookChartViewModel LoadViewModel(IBinanceService binanceService, Action<Task> onLoaded = null)
        {
            OrderBookChartViewModel orderBookChartViewModel = new OrderBookChartViewModel(binanceService);
            orderBookChartViewModel.Load().ContinueWith( x => onLoaded?.Invoke(x));
            return orderBookChartViewModel;
        }
        public async Task Load()
        {
            ChartDataBinance chartData = await binanceService.getChartData();

            bidsChartValuesSeries = new ChartValues<double>(chartData.bidsQuantity);
            asksChartValuesSeries = new ChartValues<double>(chartData.asksQuantity);

            bidsLabel = chartData.bidsPrices;
            asksLabel = chartData.asksPrices;
        }
    }
}
