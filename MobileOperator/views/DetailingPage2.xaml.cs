using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для DetailingWindow2.xaml
    /// </summary>
    public partial class DetailingPage2 : Page
    {
        public DetailingPage2(int userId, int status, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new DetailingPage2ViewModel(userId, status, context);
        }
    }
}
