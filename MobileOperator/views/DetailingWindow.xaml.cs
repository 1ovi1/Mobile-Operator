using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для DetailingWindow.xaml
    /// </summary>
    public partial class DetailingWindow : Page
    {
        public DetailingWindow(int userId, int status, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new DetailingWindowViewModel(userId, status, context);
        }
    }
}
