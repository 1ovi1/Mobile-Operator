using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminRatesPage : Page
    {
        public AdminRatesPage(Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new AdminRatesViewModel(context);
        }
    }
}