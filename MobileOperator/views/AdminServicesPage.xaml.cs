using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminServicesPage : Page
    {
        public AdminServicesPage(Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new AdminServicesViewModel(context);
        }
    }
}