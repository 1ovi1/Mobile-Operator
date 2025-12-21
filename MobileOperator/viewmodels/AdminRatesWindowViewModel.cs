using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.Infrastructure;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    public class AdminRatesViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;
        public ObservableCollection<RateModel> Rates { get; set; }

        public AdminRatesViewModel()
        {
            _context = new Infrastructure.MobileOperator(App.DbOptions);
            Rates = new ObservableCollection<RateModel>();
            LoadRates();
        }

        private void LoadRates()
        {
            Rates.Clear();
            var dbRates = _context.Rate.ToList();
            foreach (var rate in dbRates)
            {
                Rates.Add(new RateModel(rate, context: _context));
            }
        }

        private RateModel _selectedRate;
        public RateModel SelectedRate
        {
            get => _selectedRate;
            set { _selectedRate = value; OnPropertyChanged(); }
        }

        private RelayCommand _addRateCommand;
        public RelayCommand AddRateCommand
        {
            get
            {
                return _addRateCommand ?? (_addRateCommand = new RelayCommand(obj =>
                {
                    MessageBox.Show("Функционал добавления тарифа");
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}