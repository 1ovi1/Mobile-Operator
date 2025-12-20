using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class WriteOffModel : INotifyPropertyChanged
    {
        private WriteOff _writeOff;

        public WriteOffModel(WriteOff writeOff)
        {
            _writeOff = writeOff;
        }

        public int Id
        {
            get => _writeOff.Id;
            set { _writeOff.Id = value; OnPropertyChanged(); }
        }

        public int ClientId
        {
            get => _writeOff.ClientId;
            set { _writeOff.ClientId = value; OnPropertyChanged(); }
        }

        public DateTime Date
        {
            get => _writeOff.WriteOffDate;
            set { _writeOff.WriteOffDate = value; OnPropertyChanged(); }
        }

        public string DateToString => _writeOff.WriteOffDate.ToString("G");

        public decimal Amount
        {
            get => _writeOff.Amount;
            set { _writeOff.Amount = value; OnPropertyChanged(); }
        }

        public string Category
        {
            get => _writeOff.Category;
            set { _writeOff.Category = value; OnPropertyChanged(); }
        }

        public string Description
        {
            get => _writeOff.Description;
            set { _writeOff.Description = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}