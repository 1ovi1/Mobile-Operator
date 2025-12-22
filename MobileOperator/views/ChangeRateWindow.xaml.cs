using System.Windows;
using System.Windows.Controls;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для ChangeRateWindow.xaml
    /// </summary>
    public partial class ChangeRateWindow : Window
    {
        public ChangeRateWindow(int userId, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new viewmodels.ChangeRateWindowViewModel(userId, context);
        }
    }
}
