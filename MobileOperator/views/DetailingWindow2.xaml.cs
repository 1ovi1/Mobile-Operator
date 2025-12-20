using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для DetailingWindow2.xaml
    /// </summary>
    public partial class DetailingWindow2 : Page
    {
        public DetailingWindow2(int userId, int status)
        {
            InitializeComponent();
            DataContext = new DetailingWindow2ViewModel(userId, status);
        }
    }
}
