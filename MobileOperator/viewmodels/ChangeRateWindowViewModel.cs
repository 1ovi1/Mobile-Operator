using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class ChangeRateWindowViewModel : INotifyPropertyChanged
    {
        private int userId;
        private ClientModel client;
        private RateListModel allRates;
        private readonly Infrastructure.MobileOperator _context = new Infrastructure.MobileOperator(App.DbOptions);

        public ObservableCollection<RateViewModel> Rates { get; set; }

        public ChangeRateWindowViewModel(int userId)
        {
            this.userId = userId;

            client = new ClientModel(userId, _context);
            allRates = new RateListModel(_context);

            Rates = new ObservableCollection<RateViewModel> { };

            if (client.Status == 2)
                foreach (RateModel rate in allRates.AllCorporateRates)
                    AddRateToCollection(rate);
            else if (client.Status == 3)
                foreach (RateModel rate in allRates.AllNotCorporateRates)
                    AddRateToCollection(rate);
        }

        private void AddRateToCollection(RateModel rate)
        {
            Rates.Add(new RateViewModel()
            {
                Name = rate.Name,
                ConnectionCost = rate.ConnectionCost,
                Minutes = rate.Minutes,
                GB = rate.GB,
                SMS = rate.SMS,
                Cost = rate.Cost,
                Id = rate.Id,
                CityCost = rate.CityCost,
                IntercityCost = rate.IntercityCost,
                InternationalCost = rate.InternationalCost,
                SMSCost = rate.SMSCost,
                GBCost = rate.GBCost,
                Corporate = rate.Corporate,
                ClientId = userId
            });
        }

        private RelayCommand connectRateCommand;
        public RelayCommand ConnectRateCommand
        {
            get
            {
                return connectRateCommand ?? (connectRateCommand = new RelayCommand(obj =>
                {
                    if (obj is RateViewModel rateVm)
                    {
                        if (client.RateId == rateVm.Id)
                        {
                            MessageBox.Show("Вы уже подключены к данному тарифу!");
                            return;
                        }

                        decimal totalCost = rateVm.ConnectionCost + rateVm.Cost;
                        if (client.Balance < totalCost)
                        {
                            MessageBox.Show("На вашем счету недостаточно средств для подключения тарифа!");
                            return;
                        }
                        
                        decimal oldBalance = client.Balance;
                        int oldRateId = client.RateId;

                        try
                        {
                            client.Balance -= totalCost;
                            
                            if (totalCost > 0)
                            {
                                var clientEntity = _context.Client.FirstOrDefault(c => c.UserId == userId);
                                if (clientEntity != null)
                                {
                                    var writeOff = new MobileOperator.Domain.Entities.WriteOff
                                    {
                                        ClientId = clientEntity.UserId,
                                        Amount = totalCost,
                                        WriteOffDate = DateTime.UtcNow,
                                        Category = "Смена тарифа",
                                        Description = $"Подключение тарифа '{rateVm.Name}'"
                                    };
                                    _context.WriteOff.Add(writeOff);
                                }
                            }

                            if (client.ChangeRate(rateVm.Id))
                            {
                                MessageBox.Show("Тариф успешно изменен!");
                                AppViewModel.UpdateBalance();
                            }
                            else
                            {
                                throw new Exception("База данных не сохранила изменения.");
                            }
                        }
                        catch (Exception ex)
                        {
                            client.Balance = oldBalance;
                            client.RateId = oldRateId; 

                            MessageBox.Show($"Произошла ошибка при смене тарифа: {ex.Message}\nИзменения отменены.");
                        }
                    }
                }));
            }
        }

        private void CloseCurrentWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.DataContext == this)
                {
                    window.Close();
                    break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
