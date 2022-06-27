using CryptoAppv3.Service;

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
