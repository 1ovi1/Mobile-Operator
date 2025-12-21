using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class AppViewModel : INotifyPropertyChanged
    {
        public static event Action BalanceUpdated;
        public event Action OpenDialerRequested;

        public static void UpdateBalance()
        {
            BalanceUpdated?.Invoke();
        }

        private int userId, status;
        private ClientModel client;
        private RateModel rate;

        private readonly Infrastructure.MobileOperator _context = new Infrastructure.MobileOperator(App.DbOptions);

        private ServiceListModel allServices;
        public ObservableCollection<ServiceModel> Services { get; set; }

        public AppViewModel(int userId, int status)
        {
            this.userId = userId;
            this.status = status;

            allServices = new ServiceListModel(_context);

            if (status == 2)
                client = new ULModel(userId, _context);
            else
                client = new FLModel(userId, _context);

            rate = new RateModel(client.RateId, _context);

            Services = new ObservableCollection<ServiceModel> { };
            foreach (int serviceId in allServices.ClientServices(client.Id))
            {
                Services.Add(new ServiceModel(serviceId, _context));
            }

            BalanceUpdated += RefreshBalance;
        }

        private void RefreshBalance()
        {
            try
            {
                using (var ctx = new Infrastructure.MobileOperator(App.DbOptions))
                {
                    var dbData = ctx.Client
                        .Where(c => c.UserId == userId)
                        .Select(c => new { c.Balance, c.RateId, c.Minutes, c.SMS, c.GB })
                        .FirstOrDefault();

                    if (dbData != null)
                    {

                        client.Balance = (decimal)dbData.Balance;
                        OnPropertyChanged("Balance");

                        client.Minutes = dbData.Minutes;
                        OnPropertyChanged("Minutes");

                        client.SMS = dbData.SMS ?? 0;
                        OnPropertyChanged("SMS");

                        client.GB = dbData.GB ?? 0;
                        OnPropertyChanged("GB");

                        if (client.RateId != dbData.RateId)
                        {
                            client.RateId = (int)dbData.RateId;
                            rate = new RateModel(client.RateId, ctx);

                            OnPropertyChanged("Rate");
                            OnPropertyChanged("Cost");
                            OnPropertyChanged("Corporate");
                        }
                    }

                    RefreshServices(ctx);
                }
            }
            catch { }
        }


        private void RefreshServices(Infrastructure.MobileOperator ctx)
        {
            var currentServices = ctx.ServiceHistory
                .Where(h => h.ClientId == client.Id && h.TillDate == null)
                .Select(h => h.ServiceId)
                .ToList();

            var servicesToRemove = Services
                .Where(s => !currentServices.Contains(s.Id))
                .ToList();

            foreach (var service in servicesToRemove)
            {
                Services.Remove(service);
            }

            foreach (int serviceId in currentServices)
            {
                if (!Services.Any(s => s.Id == serviceId))
                {
                    Services.Add(new ServiceModel(serviceId, ctx));
                }
            }

            OnPropertyChanged("Services");
        }

        public string Rate
        {
            get { return rate.Name; }
            set { rate.Name = value; }
        }

        public decimal Cost
        {
            get { return rate.Cost; }
            set { rate.Cost = value; }
        }
        public int Minutes
        {
            get { return client.Minutes; }
            set { rate.Minutes = value; }
        }
        public int SMS
        {
            get { return client.SMS; }
            set { rate.SMS = value; }
        }
        public int GB
        {
            get { return client.GB; }
            set { client.GB = value; }
        }
        public decimal Balance
        {
            get { return client.Balance; }
            set { client.Balance = value; }
        }
        decimal amount = 0.00m;
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }
        public string ClientName
        {
            get
            {
                if (client.Status == 2)
                    return client.OrganizationName;
                else
                    return client.FIO;
            }
            set { }
        }
        public string ClientNumber
        {
            get { return client.Number; }
            set { }
        }
        public string Corporate
        {
            get
            {
                if (rate.Corporate == true)
                    return "Корпоративный";
                else return "Некорпоративный";
            }
            set
            {
                if (value == "Корпоративный")
                    rate.Corporate = true;
                else if (value == "Некорпоративный")
                    rate.Corporate = false;
            }
        }

        public RelayCommand OpenRates => new RelayCommand(obj => { });
        public RelayCommand OpenServices => new RelayCommand(obj => { });
        public RelayCommand OpenDetailing => new RelayCommand(obj => { });
        public RelayCommand OpenDetailing2 => new RelayCommand(obj => { });
        public RelayCommand LogOutCommand => new RelayCommand(obj => { });

        private RelayCommand payCommand;
        public RelayCommand PayCommand
        {
            get
            {
                return payCommand ??
                  (payCommand = new RelayCommand(obj =>
                  {
                      decimal balance = client.Balance;
                      balance += amount;
                      client.Balance = balance;
                      OnPropertyChanged("Balance");

                      if (client.Save())
                          MessageBox.Show("Баланс успешно пополнен!");
                      else
                          MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
                      Amount = 0.00m;
                      OnPropertyChanged("Amount");
                  }));
            }
        }

        private RelayCommand openDialerCommand;
        public RelayCommand OpenDialerCommand
        {
            get
            {
                return openDialerCommand ??
                       (openDialerCommand = new RelayCommand(obj =>
                       {
                           OpenDialerRequested?.Invoke();
                       }));
            }
        }

        private RelayCommand simulateNextMonthCommand;
        public RelayCommand SimulateNextMonthCommand
        {
            get
            {
                return simulateNextMonthCommand ??
                       (simulateNextMonthCommand = new RelayCommand(obj =>
                       {
                           decimal rateCost = rate.Cost;
                           if (client.Balance >= rateCost)
                           {
                               client.Balance -= rateCost;
                               
                               _context.WriteOff.Add(new MobileOperator.Domain.Entities.WriteOff
                               {
                                   ClientId = userId,
                                   Amount = rateCost,
                                   WriteOffDate = DateTime.UtcNow,
                                   Category = "Абонентская плата",
                                   Description = $"Плата за тариф '{rate.Name}'"
                               });
                               
                               client.Minutes = rate.Minutes;
                               client.GB = rate.GB;
                               client.SMS = rate.SMS;
                           }
                           else
                           {
                               client.Minutes = 0;
                               client.GB = 0;
                               client.SMS = 0;
                           }
                           
                           var servicesToProcess = Services.ToList();

                           foreach (var service in servicesToProcess)
                           {
                               if (client.Balance >= service.Cost)
                               {
                                   client.Balance -= service.Cost;

                                   _context.WriteOff.Add(new MobileOperator.Domain.Entities.WriteOff
                                   {
                                       ClientId = userId,
                                       Amount = service.Cost,
                                       WriteOffDate = DateTime.UtcNow,
                                       Category = "Абонентская плата",
                                       Description = $"Плата за услугу '{service.Name}'"
                                   });
                               }
                               else
                               {
                                   if (service.DisconnectService(client.Id))
                                   {
                                       Services.Remove(service);
                                   }
                               }
                           }
                           
                           if (client.Save())
                           {
                               OnPropertyChanged("Balance");
                               OnPropertyChanged("Minutes");
                               OnPropertyChanged("GB");
                               OnPropertyChanged("SMS");
                           }
                           else
                           {
                               MessageBox.Show("Ошибка при обновлении данных месяца.");
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
