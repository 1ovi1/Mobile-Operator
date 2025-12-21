using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminDetailingPage : Page
    {
        public AdminDetailingPage()
        {
            InitializeComponent();
            DataContext = new AdminDetailingViewModel();
        }
    }
}