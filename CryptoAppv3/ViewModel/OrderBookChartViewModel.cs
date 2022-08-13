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
                lblCryptoName = value;
                Refresh();
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
        public ICommand LiveSearchStartCommand { get; }
        public ICommand LiveSearchStopCommand { get; }

        public ChartDataBinance chartData;
        public bool liveSearch;

        public int step { get; set; }

        private string _lblCryptoName;

        public string lblCryptoName
        {
            get
            {
                return _lblCryptoName;
            }
            set
            {
                _lblCryptoName = "Orderbook Kryptowaluty: " + value;
                OnPropertyChanged("lblCryptoName");
            }
        }

        private bool _btnStopEnabled;

        public bool btnStopEnabled
        {
            get
            {
                return _btnStopEnabled;
            }
            set
            {
                if (_btnStopEnabled == value)
                    return;
                _btnStopEnabled = value;
                OnPropertyChanged("btnStopEnabled");
            }
        }
        private bool _btnStartEnabled;
        public bool btnStartEnabled
        {
            get
            {
                return _btnStartEnabled;
            }
            set
            {
                if (_btnStartEnabled == value)
                    return;
                _btnStartEnabled = value;
                OnPropertyChanged("btnStartEnabled");
            }
        }

        private bool _btnRefreshEnabled;
        public bool btnRefreshEnabled
        {
            get
            {
                return _btnRefreshEnabled;
            }
            set
            {
                if (_btnRefreshEnabled == value)
                    return;
                _btnRefreshEnabled = value;
                OnPropertyChanged("btnRefreshEnabled");
            }
        }

        public OrderBookChartViewModel(IBinanceService binanceService)
        {
            timer = 1;
            this.binanceService = binanceService;
            RefreshCommand = new RefreshCommand(this);
            LiveSearchStartCommand = new LiveSearchStartCommand(this);
            LiveSearchStopCommand = new LiveSearchStopCommand(this);
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
            refreshTime = new List<int> { 1, 2, 4, 5, 6, 8, 9, 10, 30, 60 };
            cbxSymbolsList = new List<string> { "BTCUSDT", "ETHUSDT", "BNBUSDT", "XRPUSDT", "ADAUSDT", "SOLUSDT", "DOTUSDT", "DOGEBTC" , "AVAXUSDT",  "TRXUSDT" };
            lblCryptoName = cbxSymbolsList[0];
            cbxSelectedSymbol = cbxSymbolsList[0];
            btnStopEnabled = false;
            btnStartEnabled = true;
            btnRefreshEnabled = true;

            chartData = await binanceService.getChartData();
            addDataToSeries(chartData);
        }
        public static async Task Delay()
        {
            await Task.Delay(timer*1000);
        }
        public async Task Refresh()
        {
            if (cbxSelectedSymbol != null)
                chartData = await binanceService.getChartData(cbxSelectedSymbol);
            else
                chartData = await binanceService.getChartData(cbxSymbolsList[0]);
            

            asksChartValuesSeries.RemoveAt(0);
            bidsChartValuesSeries.RemoveAt(0);

            addDataToSeries(chartData);
        }
        public async Task RefreshLiveSearch()
        {
            await Delay();
            if (liveSearch)
            { 
                await Refresh();
            }    
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
