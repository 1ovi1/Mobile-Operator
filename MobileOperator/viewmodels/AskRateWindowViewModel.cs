using System.ComponentModel;
using System.Runtime.CompilerServices;

// не реализовал
namespace MobileOperator.viewmodels
{
    class AskRateWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

}
