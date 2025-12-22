using System.Windows;

namespace MobileOperator.views
{
    public partial class CallWindow : Window
    {
        public CallWindow(int userId, string phoneNumber, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            var viewModel = new viewmodels.CallViewModel(userId, phoneNumber, context);
            viewModel.CloseRequested += () => Close();
            DataContext = viewModel;
        }
    }
}