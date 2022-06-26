using CryptoAppv3.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Threading;

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
    public class RefreshCommand: CommandBase
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
    public class LiveSearchCommand : CommandBase
    {
        private readonly OrderBookChartViewModel orderBookChartViewModel;
        public LiveSearchCommand(OrderBookChartViewModel orderBookChartViewModel, Action<Task> onLoaded = null)
        {
            this.orderBookChartViewModel = orderBookChartViewModel;
        }
        protected override async Task ExecuteAsync(object? parameter)
        {  
            while(true)
                await orderBookChartViewModel.Refresh();
        }
    }

}
