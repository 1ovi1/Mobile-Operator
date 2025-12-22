using System.Windows;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminSelectedServiceWindow : Window
    {
        public AdminSelectedServiceWindow(int serviceId, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new AdminSelectesServiceWindowViewModel(serviceId, this, context);
        }
        
        public AdminSelectedServiceWindow(Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new AdminSelectesServiceWindowViewModel(this, context);
        }
    }
}