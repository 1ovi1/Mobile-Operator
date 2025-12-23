using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using MobileOperator.Domain.Entities;
using MobileOperator.Infrastructure;
using MobileOperator.views;

namespace MobileOperator.viewmodels;

public class CallViewModel : INotifyPropertyChanged
{
    private int _userId;
    private string _phoneNumber;
    private string _callStatus;
    private string _callDuration;
    private DispatcherTimer _connectionTimer;
    private DispatcherTimer _durationTimer;
    private TimeSpan _elapsedTime;
    private bool _isConnected = false;
    private readonly MobileOperator.Infrastructure.MobileOperator _context;

    public event Action CloseRequested;

    public CallViewModel(int userId, string phoneNumber, Infrastructure.MobileOperator context)
    {
        _userId = userId;
        _phoneNumber = phoneNumber;
        CallStatus = "Набор номера...";
        CallDuration = "00:00";
        _context = context;
        
        _connectionTimer = new DispatcherTimer();
        _connectionTimer.Interval = TimeSpan.FromSeconds(3);
        _connectionTimer.Tick += ConnectionTimer_Tick;
        _connectionTimer.Start();
    }

    private void ConnectionTimer_Tick(object sender, EventArgs e)
    {
        _connectionTimer.Stop();
        _isConnected = true;
        CallStatus = "Разговор";
        
        _elapsedTime = TimeSpan.Zero;
        _durationTimer = new DispatcherTimer();
        _durationTimer.Interval = TimeSpan.FromSeconds(1);
        _durationTimer.Tick += DurationTimer_Tick;
        _durationTimer.Start();
    }

    private void DurationTimer_Tick(object sender, EventArgs e)
    {
        _elapsedTime = _elapsedTime.Add(TimeSpan.FromSeconds(1));
        CallDuration = _elapsedTime.ToString(@"mm\:ss");
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set { _phoneNumber = value; OnPropertyChanged(); }
    }

    public string CallStatus
    {
        get => _callStatus;
        set { _callStatus = value; OnPropertyChanged(); }
    }

    public string CallDuration
    {
        get => _callDuration;
        set { _callDuration = value; OnPropertyChanged(); }
    }

    public ICommand EndCallCommand => new RelayCommand(obj =>
    {
        EndCall();
    });

    private void EndCall()
    {
        _connectionTimer?.Stop();
        _durationTimer?.Stop();

        if (_isConnected)
        {
            ProcessCallData();
        }

        CloseRequested?.Invoke();
        
        AppViewModel.UpdateBalance();
    }

    private void ProcessCallData()
    {
        try
        {
            {
                var client = _context.Client.FirstOrDefault(c => c.UserId == _userId);

                if (client != null)
                {
                    Random rnd = new Random();
                    int callType = rnd.Next(1, 4);
                    
                    var rate = _context.Rate.FirstOrDefault(r => r.Id == client.RateId);
                    decimal pricePerMinute = 0;
                    
                    if (rate != null)
                    {
                        switch (callType)
                        {
                            case 1: pricePerMinute = rate.CityCost ?? 0; break;
                            case 2: pricePerMinute = rate.IntercityCost ?? 0; break;
                            case 3: pricePerMinute = rate.InternationalCost ?? 0; break;
                            default: pricePerMinute = 2.0m; break;
                        }
                    }

                    int totalMinutes = (int)Math.Ceiling(_elapsedTime.TotalMinutes);
                    decimal callCost = 0;

                    int currentMinutes = client.Minutes;
                    
                    if (currentMinutes >= totalMinutes)
                    {
                        client.Minutes = currentMinutes - totalMinutes;
                    }
                    else
                    {
                        int minutesToPay = totalMinutes - currentMinutes;
                        client.Minutes = 0;
                        
                        callCost = minutesToPay * pricePerMinute;

                        decimal currentBalance = client.Balance ?? 0;
                        client.Balance = currentBalance - callCost;
                    }
                    
                    if (callCost > 0)
                    {
                        var writeOff = new WriteOff()
                        {
                            ClientId = client.UserId,
                            Amount = callCost,
                            WriteOffDate = DateTime.UtcNow,
                            Category = "Звонок",
                            Description = $"Исходящий звонок на {PhoneNumber}. Длительность: {_elapsedTime:mm\\:ss}"
                        };
                        _context.WriteOff.Add(writeOff);
                    }

                    var newCall = new Call()
                    {
                        CallerId = client.UserId,
                        CallerNumber = client.Number,
                        CalledNumber = _phoneNumber,
                        CallTime = DateTime.UtcNow,
                        Duration = _elapsedTime,
                        TypeId = callType,
                        Cost = callCost
                    };

                    _context.Call.Add(newCall);
                    _context.SaveChanges();
                }
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show("Ошибка при сохранении звонка: " + ex.Message);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
