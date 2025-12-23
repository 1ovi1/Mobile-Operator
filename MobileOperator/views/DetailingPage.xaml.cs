using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для DetailingWindow.xaml
    /// </summary>
    public partial class DetailingPage : Page
    {
        public DetailingPage(int userId, int status, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new DetailingPageViewModel(userId, status, context);
        }
    }
}
