using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using MobileOperator.Infrastructure;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    public class AdminMainWindowViewModel : INotifyPropertyChanged
    {
        private Window _window;
        private readonly Infrastructure.MobileOperator _context;

        public ObservableCollection<ClientViewModel> Clients { get; set; }

        public AdminMainWindowViewModel(Window window, Infrastructure.MobileOperator context)
        {
            _window = window;
            _context = context;
            Clients = new ObservableCollection<ClientViewModel>();
            LoadClients();
        }

        private void LoadClients()
        {
            Clients.Clear();
            
            var dbClients = _context.Client
                .Include(c => c.Rate)
                .ToList();

            foreach (var client in dbClients)
            {
                string name = "Неизвестно";
                string type = "Неизвестно";
                
                var ulEntity = _context.UL.FirstOrDefault(u => u.UserId == client.UserId);
                
                if (ulEntity != null)
                {
                    name = ulEntity.OrganizationName;
                    type = "Юр. лицо";
                }
                else
                {
                    var flEntity = _context.FL.FirstOrDefault(f => f.UserId == client.UserId);
                    if (flEntity != null)
                    {
                        name = flEntity.FIO;
                        type = "Физ. лицо";
                    }
                }

                Clients.Add(new ClientViewModel
                {
                    Id = client.UserId,
                    NameOrOrg = name,
                    Number = client.Number,
                    Balance = (decimal)client.Balance,
                    RateName = client.Rate?.Name ?? "Нет тарифа",
                    ClientType = type,
                    FullModel = new ClientModel(client, _context)
                });
            }
        }

        private ClientViewModel _selectedClient;
        public ClientViewModel SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged("SelectedClient");
            }
        }

        private RelayCommand _logOutCommand;
        public RelayCommand LogOutCommand
        {
            get
            {
                return _logOutCommand ?? (_logOutCommand = new RelayCommand(obj =>
                {
                    UserSession.EndSession();
                    Login login = new Login(_context);
                    login.Show();
                    _window.Close();
                }));
            }
        }

        private RelayCommand _viewClientCommand;
        public RelayCommand ViewClientCommand
        {
            get
            {
                return _viewClientCommand ?? (_viewClientCommand = new RelayCommand(obj =>
                {
                    if (obj is ClientViewModel clientVm)
                    {
                        MessageBox.Show($"Открытие карточки клиента: {clientVm.NameOrOrg}");
                    }
                }));
            }
        }

        private RelayCommand _addClientCommand;
        public RelayCommand AddClientCommand
        {
            get
            {
                return _addClientCommand ?? (_addClientCommand = new RelayCommand(obj =>
                {
                     MessageBox.Show("Добавление нового клиента");
                }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

    // костыль
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string NameOrOrg { get; set; }
        public string Number { get; set; }
        public decimal Balance { get; set; }
        public string RateName { get; set; }
        public string ClientType { get; set; }
        public ClientModel FullModel { get; set; }
    }
}
