using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileOperator.viewmodels
{
    class DetailingWindow2ViewModel : INotifyPropertyChanged
    {
        private int userId;
        private int status;

        public DetailingWindow2ViewModel(int userId, int status)
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