using System.Windows;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для DialerWindow.xaml
    /// </summary>
    public partial class DialerWindow : Window
    {
        public DialerWindow()
        {
            InitializeComponent();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            CallWindow callWindow = new CallWindow();
            callWindow.Show();
        }
    }
}
