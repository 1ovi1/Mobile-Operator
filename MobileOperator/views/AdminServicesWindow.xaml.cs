using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminServicesPage : Page
    {
        public AdminServicesPage()
        {
            InitializeComponent();
            DataContext = new AdminServicesViewModel();
        }
    }
}