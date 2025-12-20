using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для ServicesWindow.xaml
    /// </summary>
    public partial class ServicesWindow : Page
    {
        public ServicesWindow(int userId, int status)
        {
            InitializeComponent();
            DataContext = new ServicesWindowViewModel(userId, status);
        }
    }
}
