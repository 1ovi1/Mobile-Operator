using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminRatesPage : Page
    {
        public AdminRatesPage()
        {
            InitializeComponent();
            DataContext = new AdminRatesViewModel();
        }
    }
}