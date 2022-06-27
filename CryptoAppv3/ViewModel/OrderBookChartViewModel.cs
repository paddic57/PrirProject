using CryptoAppv3.Models;
using CryptoAppv3.Service;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace CryptoAppv3.ViewModel
{
    public class OrderBookChartViewModel : INotifyPropertyChanged
    {
        private readonly IBinanceService binanceService;


        public SeriesCollection asksChartValuesSeries { get; set; }
        public SeriesCollection bidsChartValuesSeries { get; set; }

        public List<string> bidsLabel { get; set; }
        public List<string> asksLabel { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public List<string> cbxSymbolsList { get; set; }
        public List<int> refreshTime { get; set; }

        public static int timer { get; set; }

        private string _cbxSelectedSymbol;

        public string cbxSelectedSymbol
        {
            get
            {
                return _cbxSelectedSymbol;
            }
            set
            {
                if (_cbxSelectedSymbol == value)
                    return;
                _cbxSelectedSymbol = value;
                OnPropertyChanged("cbxSelectedSymbol");
            }
        }
        public int settimer
        {
            get
            {
                return timer;
            }
            set
            {
                if (timer == value)
                    return;
                timer = value;
                OnPropertyChanged("refreshTime");
            }

        }

        public ICommand RefreshCommand { get; }
        public ICommand LiveSearchCommand { get; }

        public int step { get; set; }

        public OrderBookChartViewModel(IBinanceService binanceService)
        {
            this.binanceService = binanceService;
            RefreshCommand = new RefreshCommand(this);
            LiveSearchCommand = new LiveSearchCommand(this);
            asksChartValuesSeries = new SeriesCollection();
            bidsChartValuesSeries = new SeriesCollection();
        }

        public static OrderBookChartViewModel LoadViewModel(IBinanceService binanceService, Action<Task> onLoaded = null)
        {
            OrderBookChartViewModel orderBookChartViewModel = new OrderBookChartViewModel(binanceService);
            orderBookChartViewModel.Load().ContinueWith(x => onLoaded?.Invoke(x));
            return orderBookChartViewModel;
        }
        public async Task Load()
        {
            refreshTime = new List<int> { 2, 4, 5, 6, 8, 9, 10, 30, 60 };
            cbxSymbolsList = new List<string> { "BTCUSDT", "BTCUSDC", "TRXUSDT", "DOGEBTC" };
            ChartDataBinance chartData = await binanceService.getChartData();
            addDataToSeries(chartData);
        }
        public static async Task Delay()
        {
            await Task.Delay(timer);
        }
        public async Task Refresh()
        {
            await Delay();
            ChartDataBinance chartData = await binanceService.getChartData(cbxSelectedSymbol);

            asksChartValuesSeries.RemoveAt(0);
            bidsChartValuesSeries.RemoveAt(0);

            addDataToSeries(chartData);



        }
        public void addDataToSeries(ChartDataBinance chartData)
        {
            step = chartData.asksPrices.Count / 4 - 1;
            bidsLabel = new List<string>(chartData.bidsPrices);
            asksLabel = new List<string>(chartData.asksPrices);
            OnPropertyChanged("bidsLabel");
            OnPropertyChanged("asksLabel");
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
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
