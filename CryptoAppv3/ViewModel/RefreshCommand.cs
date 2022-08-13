using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CryptoAppv3.ViewModel
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;


        public bool CanExecute(object? parameter)
        {
            return true;
        }


        public async void Execute(object? parameter)
        {
            await ExecuteAsync(parameter);

        }

        protected abstract Task ExecuteAsync(object? parameter);

        protected void OnCanExecutedChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }
    }
    public class RefreshCommand : CommandBase
    {
        private readonly OrderBookChartViewModel orderBookChartViewModel;
        public RefreshCommand(OrderBookChartViewModel orderBookChartViewModel, Action<Task> onLoaded = null)
        {
            this.orderBookChartViewModel = orderBookChartViewModel;
        }
        protected override async Task ExecuteAsync(object? parameter)
        {
            await orderBookChartViewModel.Refresh();
        }
    }
    public class LiveSearchStartCommand : CommandBase
    {
        private readonly OrderBookChartViewModel orderBookChartViewModel;
        public LiveSearchStartCommand(OrderBookChartViewModel orderBookChartViewModel, Action<Task> onLoaded = null)
        {
            this.orderBookChartViewModel = orderBookChartViewModel;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            orderBookChartViewModel.btnRefreshEnabled = false;
            orderBookChartViewModel.btnStopEnabled = true;
            orderBookChartViewModel.btnStartEnabled = false;

            orderBookChartViewModel.liveSearch = true;
            while (orderBookChartViewModel.liveSearch)
            {
                await orderBookChartViewModel.RefreshLiveSearch();
            }
        }
    }
    public class LiveSearchStopCommand : CommandBase
    {
        private readonly OrderBookChartViewModel orderBookChartViewModel;
        public LiveSearchStopCommand(OrderBookChartViewModel orderBookChartViewModel, Action<Task> onLoaded = null)
        {
            this.orderBookChartViewModel = orderBookChartViewModel;
        }

        protected override async Task ExecuteAsync(object? parameter)
        {
            orderBookChartViewModel.btnRefreshEnabled = true;
            orderBookChartViewModel.btnStopEnabled = false;
            orderBookChartViewModel.btnStartEnabled = true;

            orderBookChartViewModel.liveSearch = false;
        }
    }

}
