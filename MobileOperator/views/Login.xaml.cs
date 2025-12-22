using System.Windows;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class Login : Window
    {
        public Login(Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new LoginWindowViewModel(this, context);
        }
    }
}