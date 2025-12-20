using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.Domain.Entities;

namespace MobileOperator.Models
{
    public class CallModel : INotifyPropertyChanged
    {
        private Call _call;

        public CallModel()
        {
            _call = new Call();
            _call.CallTime = DateTime.Now;
        }

        public CallModel(Call call)
        {
            _call = call;
        }
        
        public Call Entity => _call;

        public int Id
        {
            get => _call.Id;
            set
            {
                if (_call.Id != value)
                {
                    _call.Id = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CallerId
        {
            get => _call.CallerId;
            set
            {
                _call.CallerId = value;
                OnPropertyChanged();
            }
        }

        public string CallerNumber
        {
            get => _call.CallerNumber;
            set
            {
                _call.CallerNumber = value;
                OnPropertyChanged();
            }
        }

        public int CalledId
        {
            get => _call.CalledId ?? 0;
            set
            {
                _call.CalledId = value;
                OnPropertyChanged();
            }
        }

        public string CalledNumber
        {
            get => _call.CalledNumber;
            set
            {
                _call.CalledNumber = value;
                OnPropertyChanged();
            }
        }

        public DateTime Time
        {
            get => _call.CallTime;
            set
            {
                _call.CallTime = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TimeToString));
            }
        }

        public string TimeToString => _call.CallTime.ToString("G");

        public TimeSpan Duration
        {
            get => _call.Duration;
            set
            {
                _call.Duration = value;
                OnPropertyChanged();
            }
        }
        
        public int TypeId
        {
            get => _call.TypeId; 
            set
            {
                _call.TypeId = value;
                OnPropertyChanged();
            }
        }
        
        public string TypeName
        {
            get => _call.CallType?.Name ?? string.Empty;
        }

        public decimal Cost
        {
            get => _call.Cost;
            set
            {
                _call.Cost = value;
                OnPropertyChanged();
            }
        }
        
        private string _direction;
        public string Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                OnPropertyChanged();
            }
        }

        private string _number;
        public string Number
        {
            get => _number;
            set
            {
                _number = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
