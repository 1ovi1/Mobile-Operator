using System.Windows;
using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для RatesWindow.xaml
    /// </summary>
    public partial class RatesWindow : Page
    {
        private int userId;
        private int status;
        public RatesWindow(int userId, int status)
        {
            InitializeComponent();
            this.userId = userId;
            this.status = status;
            DataContext = new RateWindowViewModel(userId, status);
        }

        private void ChangeRateButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeRateWindow changeRateWindow = new ChangeRateWindow(userId);
            changeRateWindow.Show();
        }
    }
}
