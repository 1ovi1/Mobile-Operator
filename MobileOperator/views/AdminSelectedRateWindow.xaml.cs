using System.Windows;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для AdminSelectedRateWindow.xaml
    /// </summary>
    public partial class AdminSelectedRateWindow : Window
    {
        public AdminSelectedRateWindow(int rateId, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new AdminSelectesRateWindowViewModel(rateId, this, context);
        }
        
        public AdminSelectedRateWindow(Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new AdminSelectesRateWindowViewModel(this, context);
        }
    }
}