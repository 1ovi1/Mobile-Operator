using System.Windows;
using System.Windows.Controls;

namespace MobileOperator.views
{
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
            DataContext = new viewmodels.AdminMainWindowViewModel(this);
        }

        private void AdminTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (AdminTabControl.SelectedIndex == 1 && RatesFrame.Content == null)
                {
                    RatesFrame.Navigate(new AdminRatesPage());
                }
                else if (AdminTabControl.SelectedIndex == 2 && ServicesFrame.Content == null)
                {
                    ServicesFrame.Navigate(new AdminServicesPage());
                }
                else if (AdminTabControl.SelectedIndex == 3 && DetailingFrame.Content == null)
                {
                    DetailingFrame.Navigate(new AdminDetailingPage());
                }
                else if (AdminTabControl.SelectedIndex == 4 && HistoryFrame.Content == null)
                {
                    HistoryFrame.Navigate(new AdminDetailingPage2());
                }
            }
        }
    }
}