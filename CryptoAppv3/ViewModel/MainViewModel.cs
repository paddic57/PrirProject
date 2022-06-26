using CryptoAppv3.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoAppv3.ViewModel
{
    public class MainViewModel
    {
        public OrderBookChartViewModel orderBookChartViewModel { get; set; }
        public MainViewModel()
        {
            IBinanceService binanceService = new BinanceService();
            orderBookChartViewModel = OrderBookChartViewModel.LoadViewModel(binanceService);
        }
    }
}
