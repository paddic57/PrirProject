using CryptoAppv3.Models;
using CryptoAppv3.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CryptoAppv3.ViewModel
{
    public class OrderBookChartViewModel
    {
        private readonly IBinanceService binanceService;


        public SeriesCollection asksChartValuesSeries { get; set; }
        public SeriesCollection bidsChartValuesSeries { get; set; }

        public List<string> bidsLabel { get; set; }
        public List<string> asksLabel { get; set; }
        public Func<double, string> YFormatter { get; set; }
        public string stringTitle { get; set; }

        public ICommand RefreshCommand { get; }

        public OrderBookChartViewModel(IBinanceService binanceService)
        {
            this.binanceService = binanceService;
            RefreshCommand = new RefreshCommand(this);
            asksChartValuesSeries = new SeriesCollection();
            bidsChartValuesSeries = new SeriesCollection();
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
            YFormatter = value => value.ToString("0.00");

            addDataToSeries(chartData);
        }

        public async Task Refresh()
        {
            ChartDataBinance chartData = await binanceService.getChartData();

            asksChartValuesSeries.RemoveAt(0);
            bidsChartValuesSeries.RemoveAt(0);

            addDataToSeries(chartData);
            
            

        }
        public void addDataToSeries(ChartDataBinance chartData)
        {
            asksChartValuesSeries.Add(new LineSeries
            {
                Values = new ChartValues<double>(chartData.asksQuantity),
                PointGeometry = null,
                Fill = Brushes.Red,
                Stroke = Brushes.Red,
            });

            bidsChartValuesSeries.Add(new LineSeries
            {
                Values = new ChartValues<double>(chartData.bidsQuantity),
                PointGeometry = null,
                Fill = Brushes.Green,
                Stroke = Brushes.Green,
            });
            bidsLabel = chartData.bidsPrices;
            asksLabel = chartData.asksPrices;
        }
    }
}
