using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using MobileOperator.views;
using MobileOperator.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MobileOperator.viewmodels
{
    public class LoginWindowViewModel : INotifyPropertyChanged
    {
        private Window _window;
        private readonly Infrastructure.MobileOperator _context;

        public LoginWindowViewModel(Window window, Infrastructure.MobileOperator context)
        {
            _window = window;
            _context = context;
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }

        private string _status = "Client";

        private RelayCommand _checkUserStatusCommand;
        public RelayCommand CheckUserStatusCommand
        {
            get
            {
                return _checkUserStatusCommand ??
                  (_checkUserStatusCommand = new RelayCommand(obj =>
                  {
                      _status = obj as string;
                  }));
            }
        }

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand ??
                  (_loginCommand = new RelayCommand(obj =>
                  {
                      var passwordBox = obj as PasswordBox;
                      if (passwordBox == null) return;

                      string password = passwordBox.Password;

                      if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(password))
                      {
                          MessageBox.Show("Введите логин и пароль");
                          return;
                      }

                      if (_status == "Client")
                      {
                          AuthenticateClient(password);
                      }
                      else if (_status == "Admin")
                      {
                          AuthenticateAdmin(password);
                      }
                  }));
            }
        }

        private void AuthenticateClient(string password)
        {
            var client = _context.Client
                .Include(c => c.User)
                .FirstOrDefault(c => c.Number == Username && c.User.Password == password);

            if (client != null)
            {
                bool isUl = _context.UL.Any(u => u.UserId == client.UserId);
                int statusId = isUl ? 2 : 1;
                
                UserSession.StartSession(client.UserId, statusId, "Client");
                
                MainWindow main = new MainWindow(client.UserId, statusId, _context);
                main.Show();
                _window.Close();
            }
            else
            {
                MessageBox.Show("Неверный номер телефона или пароль клиента.");
            }
        }

        private void AuthenticateAdmin(string password)
        {
            var admin = _context.Admin
                .Include(a => a.User)
                .FirstOrDefault(a => a.Login == Username && a.User.Password == password);

            if (admin != null)
            {
                UserSession.StartSession(admin.UserId, 0, "Admin");

                AdminMainWindow main = new AdminMainWindow(_context);
                main.Show();
                _window.Close();
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль администратора.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
