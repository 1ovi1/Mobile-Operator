using System.Windows;
using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для RatesWindow.xaml
    /// </summary>
    public partial class RatesPage : Page
    {
        private int userId;
        private int status;
        public RatesPage(int userId, int status, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            this.userId = userId;
            this.status = status;
            DataContext = new RateWindowViewModel(userId, status, context);
        }
    }
}
