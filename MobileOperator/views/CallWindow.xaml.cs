using System.Windows;

namespace MobileOperator.views
{
    /// <summary>
    /// Логика взаимодействия для CallWindow.xaml
    /// </summary>
    public partial class CallWindow : Window
    {
        public CallWindow()
        {
            InitializeComponent();
        }
        private void EndCallButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
