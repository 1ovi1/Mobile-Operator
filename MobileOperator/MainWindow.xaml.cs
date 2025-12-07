using System.Windows;
using MobileOperator.pages;

namespace MobileOperator
{
    public partial class MainWindow : Window
    {
        private int userId = 0;
        private int status = 0;

        public MainWindow()
        {
            InitializeComponent();
            
            // Подписка на событие изменения вкладки
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
        }

        private void MainTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1 && ServicesFrame.Content == null)
            {
                // Загрузка страницы услуг
                ServicesFrame.Navigate(new ServicesWindow(userId, status));
            }
            else if (MainTabControl.SelectedIndex == 2 && RatesFrame.Content == null)
            {
                // Загрузка страницы тарифов
                RatesFrame.Navigate(new RatesWindow(userId, status));
            }
            else if (MainTabControl.SelectedIndex == 3 && DetailingFrame.Content == null)
            {
                // Загрузка страницы звонков
                DetailingFrame.Navigate(new DetailingWindow(userId, status));
            }
            else if (MainTabControl.SelectedIndex == 4 && Detailing2Frame.Content == null)
            {
                // Загрузка страницы тарифов и услуг
                Detailing2Frame.Navigate(new DetailingWindow2(userId, status));
            }
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            DialerWindow dialerWindow = new DialerWindow();
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