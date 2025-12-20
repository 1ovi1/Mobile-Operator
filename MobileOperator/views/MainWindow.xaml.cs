using System.Windows;
using MobileOperator.viewmodels;
using MobileOperator.views;

namespace MobileOperator
{
    public partial class MainWindow : Window
    {
        private int userId = 5;
        private int status = 2;

        public MainWindow()
        {
            InitializeComponent();
            
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
            
            var vm = new AppViewModel(userId, status);
            
            vm.OpenDialerRequested += () => 
            {
                DialerWindow dialerWindow = new DialerWindow(userId);
                dialerWindow.Show();
            };
            
            DataContext = vm;
        }

        private void MainTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1 && ServicesFrame.Content == null)
            {
                ServicesFrame.Navigate(new ServicesWindow(userId, status));
            }
            else if (MainTabControl.SelectedIndex == 2 && RatesFrame.Content == null)
            {
                RatesFrame.Navigate(new RatesWindow(userId, status));
            }
            else if (MainTabControl.SelectedIndex == 3 && DetailingFrame.Content == null)
            {
                DetailingFrame.Navigate(new DetailingWindow(userId, status));
            }
            else if (MainTabControl.SelectedIndex == 4 && Detailing2Frame.Content == null)
            {
                Detailing2Frame.Navigate(new DetailingWindow2(userId, status));
            }
            else if (MainTabControl.SelectedIndex == 5 && WriteOffsFrame.Content == null)
            {
                WriteOffsFrame.Navigate(new WriteOffsPage(userId, status));
            }
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            DialerWindow dialerWindow = new DialerWindow(userId);
            dialerWindow.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
            this.Close();
        }
    }
}