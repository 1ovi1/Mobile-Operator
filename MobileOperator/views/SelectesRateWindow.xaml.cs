using System.Windows;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для SelectesRateWindow.xaml
    /// </summary>
    public partial class SelectesRateWindow : Window
    {
        public SelectesRateWindow(int userId, int rateId)
        {
            InitializeComponent();
            DataContext = new ViewRateWindowViewModel(userId, rateId, this);
        }
    }
}
