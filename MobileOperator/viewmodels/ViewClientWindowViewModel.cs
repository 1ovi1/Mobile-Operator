using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.Domain.Entities;
using MobileOperator.models;
using MobileOperator.Infrastructure;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    public class ViewClientWindowViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;
        private readonly Window _window;
        
        private ClientModel _client;
        
        private bool _createMode;

        public ObservableCollection<Rate> Rates { get; set; }
        
        public ViewClientWindowViewModel(int clientId, int clientStatus, Window window, Infrastructure.MobileOperator context)
        {
            _context = context;
            _window = window;
            Rates = new ObservableCollection<Rate>();
            _createMode = false;
            
            if (clientStatus == 2)
            {
                _client = new ULModel(clientId, _context);
                _statusUL = true;
            }
            else
            {
                _client = new FLModel(clientId, _context);
                _statusFL = true;
            }
            
            LoadRates();
        }
        
        public ViewClientWindowViewModel(Window window, Infrastructure.MobileOperator context)
        {
            _context = context;
            _window = window;
            Rates = new ObservableCollection<Rate>();
            _createMode = true;
            
            _client = new FLModel(_context);
            _statusFL = true;
            
            LoadRates();
        }
        

        public string FIO
        {
            get => _client.FIO;
            set { _client.FIO = value; OnPropertyChanged(); }
        }
        public string OrganizationName
        {
            get => _client.OrganizationName;
            set { _client.OrganizationName = value; OnPropertyChanged(); }
        }
        public string PassportDetails
        {
            get => _client.PassportDetails;
            set { _client.PassportDetails = value; OnPropertyChanged(); }
        }
        public string Address
        {
            get => _client.Address;
            set { _client.Address = value; OnPropertyChanged(); }
        }
        public string Number
        {
            get => _client.Number;
            set { _client.Number = value; OnPropertyChanged(); }
        }
        public int GB
        {
            get => _client.GB;
            set { _client.GB = value; OnPropertyChanged(); }
        }
        public int SMS
        {
            get => _client.SMS;
            set { _client.SMS = value; OnPropertyChanged(); }
        }
        public int Minutes
        {
            get => _client.Minutes;
            set { _client.Minutes = value; OnPropertyChanged(); }
        }
        public decimal Balance
        {
            get => _client.Balance;
            set { _client.Balance = value; OnPropertyChanged(); }
        }
        public int Rate
        {
            get => _client.RateId;
            set { _client.RateId = value; OnPropertyChanged(); }
        }
        public string Password
        {
            get => _client.Entity?.User?.Password; 
            set 
            { 
                if (_client.Entity?.User != null) 
                {
                    _client.Entity.User.Password = value; 
                    OnPropertyChanged(); 
                }
            }
        }

        public bool Create => _createMode;

        private bool _statusFL;
        public bool ChectedFLStatus
        {
            get => _statusFL;
            set
            {
                if (_statusFL != value)
                {
                    _statusFL = value;
                    if (value && _createMode)
                    {
                        SwitchModel(new FLModel(_context));
                    }
                    OnPropertyChanged();
                    LoadRates();
                }
            }
        }

        private bool _statusUL;
        public bool ChectedULStatus
        {
            get => _statusUL;
            set
            {
                if (_statusUL != value)
                {
                    _statusUL = value;
                    if (value && _createMode)
                    {
                        SwitchModel(new ULModel(_context));
                    }
                    OnPropertyChanged();
                    LoadRates();
                }
            }
        }

        private void SwitchModel(ClientModel newModel)
        {
            newModel.Number = _client.Number;
            newModel.Balance = _client.Balance;
            newModel.Minutes = _client.Minutes;
            newModel.SMS = _client.SMS;
            newModel.GB = _client.GB;
            newModel.RateId = _client.RateId;
            
            if (newModel.Entity.User != null && _client.Entity.User != null)
                newModel.Entity.User.Password = _client.Entity.User.Password;

            _client = newModel;
            OnPropertyChanged(string.Empty);
        }

        private void LoadRates()
        {
            Rates.Clear();
            bool isCorporate = _statusUL;
            
            var filteredRates = _context.Rate.Where(r => r.Corporate == isCorporate).ToList();
            foreach (var r in filteredRates)
            {
                Rates.Add(r);
            }
        }

        private RelayCommand _updateClientCommand;
        public RelayCommand UpdateClientCommand
        {
            get
            {
                return _updateClientCommand ?? (_updateClientCommand = new RelayCommand(obj =>
                {
                    if (_client.Save())
                    {
                        MessageBox.Show("Сохранение прошло успешно!");
                        _window.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ошибка сохранения! Проверьте данные.");
                    }
                }));
            }
        }

        private RelayCommand _delClientCommand;
        public RelayCommand DelClientCommand
        {
            get
            {
                return _delClientCommand ?? (_delClientCommand = new RelayCommand(obj =>
                {
                    if (MessageBox.Show("Вы уверены, что хотите удалить клиента?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        if (_client.Remove())
                        {
                            MessageBox.Show("Клиент удален.");
                            _window.Close();
                        }
                        else
                        {
                            MessageBox.Show("Ошибка при удалении.");
                        }
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
