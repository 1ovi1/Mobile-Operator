using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileOperator.models
{
    public class LoginModel : INotifyPropertyChanged
    {
        private int _id;
        private string _password;
        private string _login;
        private int _status;

        public int Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged(); }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public string Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}