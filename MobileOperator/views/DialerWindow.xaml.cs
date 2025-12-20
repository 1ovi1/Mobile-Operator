using System.Windows;

namespace MobileOperator.views
{
    public partial class DialerWindow : Window
    {
        public DialerWindow(int userId)
        {
            InitializeComponent();
            var viewModel = new viewmodels.DialerViewModel(userId);
            viewModel.CloseRequested += () => Close();
            viewModel.CallRequested += (phoneNumber) =>
            {
                Close();
                var callWindow = new CallWindow(userId, phoneNumber);
                callWindow.Show();
            };
            DataContext = viewModel;
        }
    }
}