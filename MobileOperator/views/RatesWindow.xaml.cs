using MobileOperator.pages;
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
using System.Windows.Shapes;
using MobileOperator.viewmodels;

namespace MobileOperator
{
    /// <summary>
    /// Логика взаимодействия для RatesWindow.xaml
    /// </summary>
    public partial class RatesWindow : Page
    {
        private int userId;
        private int status;
        public RatesWindow(int userId, int status)
        {
            InitializeComponent();
            this.userId = userId;
            this.status = status;
            DataContext = new RatesViewModel(userId, status);
        }

        private void ChangeRateButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeRateWindow changeRateWindow = new ChangeRateWindow(userId);
            changeRateWindow.Show();
        }
    }
}
