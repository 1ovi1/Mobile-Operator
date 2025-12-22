using System.Windows;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class ViewClientWindow : Window
    {
        public ViewClientWindow(int clientId, int clientStatus, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new ViewClientWindowViewModel(clientId, clientStatus, this, context);
        }
        
        public ViewClientWindow(Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new ViewClientWindowViewModel(this, context);
        }
    }
}