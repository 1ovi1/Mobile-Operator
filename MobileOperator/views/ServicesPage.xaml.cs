using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для ServicesWindow.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage(int userId, int status, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new ServicesWindowViewModel(userId, status, context);
        }
    }
}
