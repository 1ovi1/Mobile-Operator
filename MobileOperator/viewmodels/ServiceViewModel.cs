using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class ServiceViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context = new Infrastructure.MobileOperator(App.DbOptions);

        private ServiceModel service;

        public ServiceViewModel()
        {
            service = new ServiceModel(_context);
        }

        public ServiceViewModel(ServiceModel s)
        {
            service = s;
        }

        public ServiceViewModel(int id)
        {
            service = new ServiceModel(id, _context);
        }

        private ServicesWindowViewModel servicesList;
        public ServicesWindowViewModel ServicesList
        {
            get { return servicesList; }
            set { servicesList = value; }
        }
        private int clientId;
        public int ClientId
        {
            get { return clientId; }
            set { clientId = value; }
        }
        public int Id
        {
            get { return service.Id; }
            set { service.Id = value; }
        }
        public string Name
        {
            get { return service.Name; }
            set { service.Name = value; }
        }

        public decimal Cost
        {
            get { return (decimal)service.Cost; }
            set { service.Cost = value; }
        }
        public decimal ConnectionCost
        {
            get { return (decimal)service.ConnectionCost; }
            set { service.ConnectionCost = value; }
        }

        public string Conditions
        {
            get { return service.Conditions; }
            set { service.Conditions = value; }
        }

        private RelayCommand connectServiceCommand;
        public RelayCommand ConnectServiceCommand
        {
            get
            {
                return connectServiceCommand ??
                  (connectServiceCommand = new RelayCommand(obj =>
                  {
                      int? idService = obj as int?;
                      
                      var clientDb = _context.Client.FirstOrDefault(c => c.UserId == clientId);
                      if (clientDb == null)
                      {
                          MessageBox.Show("Клиент не найден!");
                          return;
                      }

                      ClientModel client = new ClientModel(clientId, _context);

                      if (idService != null)
                      {
                          ServiceModel selelectService = new ServiceModel((int)idService, _context);
                          decimal totalCost = selelectService.Cost + selelectService.ConnectionCost;

                          if (client.Balance < totalCost)
                              MessageBox.Show("На вашем счету недостаточно средств для подключения услуги!");
                          else
                          {
                              client.Balance -= totalCost;
                              
                              if (totalCost > 0)
                              {
                                  var writeOff = new MobileOperator.Domain.Entities.WriteOff
                                  {
                                      ClientId = clientDb.UserId,
                                      Amount = totalCost,
                                      WriteOffDate = System.DateTime.UtcNow,
                                      Category = "Услуга",
                                      Description = $"Подключение услуги '{selelectService.Name}'"
                                  };
                                  _context.WriteOff.Add(writeOff);
                              }

                              if (selelectService.ConnectService(clientDb.UserId))
                              {
                                  servicesList.ConnectionServices.Add(this);
                                  servicesList.AvailableServices.Remove(this);

                                  AppViewModel.UpdateBalance();

                                  MessageBox.Show("Подключение прошло успешно!");
                                  OnPropertyChanged("ConnectionServices");
                                  OnPropertyChanged("AvailableServices");
                              }
                              else
                                  MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
                          }
                      }
                  }));
            }

        }

        private RelayCommand disconnectServiceCommand;
        public RelayCommand DisconnectServiceCommand
        {
            get
            {
                return disconnectServiceCommand ??
                  (disconnectServiceCommand = new RelayCommand(obj =>
                  {
                      int? idService = obj as int?;
                      
                      var clientDb = _context.Client.FirstOrDefault(c => c.UserId == clientId);
                      if (clientDb == null)
                      {
                          MessageBox.Show("Клиент не найден!");
                          return;
                      }

                      if (idService != null)
                      {
                          ServiceModel selelectService = new ServiceModel((int)idService, _context);
                          
                          if (selelectService.DisconnectService(clientDb.UserId))
                          {
                              servicesList.ConnectionServices.Remove(this);
                              servicesList.AvailableServices.Add(this);

                              AppViewModel.UpdateBalance();

                              OnPropertyChanged("ConnectionServices");
                              OnPropertyChanged("AvailableServices");
                              MessageBox.Show("Отключение прошло успешно!");
                          }
                          else
                              MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
                      }
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
