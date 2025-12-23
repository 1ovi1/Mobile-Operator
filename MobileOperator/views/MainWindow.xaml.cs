using System.Windows;
using MobileOperator.Infrastructure;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class MainWindow : Window
    {
        private int _userId;
        private int _status;
        private readonly Infrastructure.MobileOperator _context;
        
        public MainWindow(int userId, int status, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            
            _userId = userId;
            _status = status;
            _context = context;
            
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
            
            var vm = new AppViewModel(_userId, _status, _context);
            
            vm.OpenDialerRequested += () => 
            {
                DialerWindow dialerWindow = new DialerWindow(_userId, _context);
                dialerWindow.Show();
            };
            
            DataContext = vm;
        }

        private void MainTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1 && ServicesFrame.Content == null)
            {
                ServicesFrame.Navigate(new ServicesPage(_userId, _status, _context));
            }
            else if (MainTabControl.SelectedIndex == 2 && RatesFrame.Content == null)
            {
                RatesFrame.Navigate(new RatesPage(_userId, _status, _context));
            }
            else if (MainTabControl.SelectedIndex == 3 && DetailingFrame.Content == null)
            {
                DetailingFrame.Navigate(new DetailingPage(_userId, _status, _context));
            }
            else if (MainTabControl.SelectedIndex == 4 && Detailing2Frame.Content == null)
            {
                Detailing2Frame.Navigate(new DetailingPage2(_userId, _status, _context));
            }
            else if (MainTabControl.SelectedIndex == 5 && WriteOffsFrame.Content == null)
            {
                WriteOffsFrame.Navigate(new WriteOffsPage(_userId, _status, _context));
            }
        }
    }
}
