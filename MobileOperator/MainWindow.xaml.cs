using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MobileOperator.pages;

namespace MobileOperator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int userId = 0;
        private int status = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CallButton_Click(object sender, RoutedEventArgs e)
        {
            DialerWindow dialerWindow = new DialerWindow();
            dialerWindow.Show();
        }

        private void OpenServices_Click(object sender, RoutedEventArgs e)
        {
            ServicesWindow servicesWindow = new ServicesWindow(userId, status);
            servicesWindow.Show();
        }

        private void OpenRates_Click(object sender, RoutedEventArgs e)
        {
            RatesWindow ratesWindow = new RatesWindow(userId, status);
            ratesWindow.Show();
        }

        private void OpenDetailing_Click(object sender, RoutedEventArgs e)
        {
            DetailingWindow detailingWindow = new DetailingWindow(userId, status);
            detailingWindow.Show();
        }

        private void OpenDetailing2_Click(object sender, RoutedEventArgs e)
        {
            DetailingWindow2 detailingWindow2 = new DetailingWindow2(userId, status);
            detailingWindow2.Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Login loginWindow = new Login();
            loginWindow.Show();
        }
    }
}