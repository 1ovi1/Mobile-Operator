using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileOperator.viewmodels
{
    class ServicesViewModel : INotifyPropertyChanged
    {
        private int userId;
        private int status;

        public ServicesViewModel(int userId, int status)
        {
            this.userId = userId;
            this.status = status;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}