// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using System.Windows;
// using System.Windows.Controls;
// using System.Windows.Data;
// using System.Windows.Documents;
// using System.Windows.Input;
// using System.Windows.Media;
// using System.Windows.Media.Imaging;
// using System.Windows.Shapes;
// using MobileOperator.ViewModel;
//
// namespace MobileOperator
// {
//     /// <summary>
//     /// Логика взаимодействия для AdminSelectedRateWindow.xaml
//     /// </summary>
//     public partial class AdminSelectedRateWindow : Window
//     {
//         public AdminSelectedRateWindow(int rateId)
//         {
//             InitializeComponent();
//             DataContext = new AdminSelectesRateWindowViewModel(rateId, this);
//         }
//         public AdminSelectedRateWindow()
//         {
//             InitializeComponent();
//             DataContext = new AdminSelectesRateWindowViewModel(this);
//         }
//     }
// }
