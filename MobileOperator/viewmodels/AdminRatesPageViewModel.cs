using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    public class AdminRatesViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;

        public ObservableCollection<RateModel> Rates { get; set; }

        public AdminRatesViewModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            Rates = new ObservableCollection<RateModel>();
            LoadRates();
        }

        private void LoadRates()
        {
            Rates.Clear();
            var dbRates = _context.Rate.ToList();
            foreach (var rate in dbRates)
            {
                Rates.Add(new RateModel(rate, _context));
            }
        }

        private RateModel _selectedRate;
        public RateModel SelectedRate
        {
            get => _selectedRate;
            set
            {
                _selectedRate = value;
                OnPropertyChanged();
            }
        }

        private RelayCommand _addRateCommand;
        public RelayCommand AddRateCommand
        {
            get
            {
                return _addRateCommand ?? (_addRateCommand = new RelayCommand(obj =>
                {
                    AdminSelectedRateWindow rateWindow = new AdminSelectedRateWindow(_context);
                    rateWindow.ShowDialog();
                    LoadRates();
                }));
            }
        }

        private RelayCommand _viewRateCommand;
        public RelayCommand ViewRateCommand
        {
            get
            {
                return _viewRateCommand ?? (_viewRateCommand = new RelayCommand(obj =>
                {
                    RateModel target = obj as RateModel ?? SelectedRate;

                    if (target != null)
                    {
                        AdminSelectedRateWindow rateWindow = new AdminSelectedRateWindow(target.Id, _context);
                        rateWindow.ShowDialog();
                        LoadRates();
                    }
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
