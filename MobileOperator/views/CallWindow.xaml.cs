using System.Windows;

namespace MobileOperator.views
{
    public partial class CallWindow : Window
    {
        public CallWindow(int userId, string phoneNumber)
        {
            InitializeComponent();
            var viewModel = new viewmodels.CallViewModel(userId, phoneNumber);
            viewModel.CloseRequested += () => Close();
            DataContext = viewModel;
        }
    }
}