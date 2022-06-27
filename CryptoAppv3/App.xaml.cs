using CryptoAppv3.ViewModel;
using System.Windows;

namespace CryptoAppv3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow()
            {
                DataContext = new MainViewModel()
            };
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
