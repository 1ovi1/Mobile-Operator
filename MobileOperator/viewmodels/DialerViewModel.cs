using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MobileOperator.views;

namespace MobileOperator.viewmodels;

public class DialerViewModel : INotifyPropertyChanged
{
    private int _userId;
    private string _phoneNumber = "";

    public event Action<string> CallRequested;
    public event Action CloseRequested;

    public DialerViewModel(int userId)
    {
        _userId = userId;
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged();
        }
    }

    public ICommand DigitCommand => new RelayCommand(param =>
    {
        if (PhoneNumber.Length < 12)
        {
            PhoneNumber += param.ToString();
        }
    });

    public ICommand BackspaceCommand => new RelayCommand(obj =>
    {
        if (PhoneNumber.Length > 0)
        {
            PhoneNumber = PhoneNumber.Substring(0, PhoneNumber.Length - 1);
        }
    });

    public ICommand CallCommand => new RelayCommand(obj =>
    {
        if (!string.IsNullOrWhiteSpace(PhoneNumber))
        {
            CallRequested?.Invoke(PhoneNumber);
        }
    });

    public ICommand CloseCommand => new RelayCommand(obj =>
    {
        CloseRequested?.Invoke();
    });

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}