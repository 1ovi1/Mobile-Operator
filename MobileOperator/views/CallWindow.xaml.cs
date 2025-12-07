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

namespace MobileOperator.pages
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
