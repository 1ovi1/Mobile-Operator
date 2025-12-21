using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminDetailingPage2 : Page
    {
        public AdminDetailingPage2()
        {
            InitializeComponent();
            DataContext = new AdminDetailing2ViewModel();
        }
    }
}