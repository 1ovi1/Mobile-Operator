using System.Windows;
using MobileOperator.Infrastructure;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class MainWindow : Window
    {
        private int _userId;
        private int _status;
        
        public MainWindow() : this(UserSession.UserId, UserSession.StatusId) 
        {
        }
        
        public MainWindow(int userId, int status)
        {
            InitializeComponent();
            
            _userId = userId;
            _status = status;
            
            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;
            
            var vm = new AppViewModel(_userId, _status);
            
            vm.OpenDialerRequested += () => 
            {
                DialerWindow dialerWindow = new DialerWindow(_userId);
                dialerWindow.Show();
            };
            
            DataContext = vm;
        }

        private void MainTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (MainTabControl.SelectedIndex == 1 && ServicesFrame.Content == null)
            {
                ServicesFrame.Navigate(new ServicesWindow(_userId, _status));
            }
            else if (MainTabControl.SelectedIndex == 2 && RatesFrame.Content == null)
            {
                RatesFrame.Navigate(new RatesWindow(_userId, _status));
            }
            else if (MainTabControl.SelectedIndex == 3 && DetailingFrame.Content == null)
            {
                DetailingFrame.Navigate(new DetailingWindow(_userId, _status));
            }
            else if (MainTabControl.SelectedIndex == 4 && Detailing2Frame.Content == null)
            {
                Detailing2Frame.Navigate(new DetailingWindow2(_userId, _status));
            }
            else if (MainTabControl.SelectedIndex == 5 && WriteOffsFrame.Content == null)
            {
                WriteOffsFrame.Navigate(new WriteOffsPage(_userId, _status));
            }
        }
    }
}
