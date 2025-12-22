using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class AdminDetailingPage : Page
    {
        public AdminDetailingPage(Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new AdminDetailingViewModel(context);
        }
    }
}