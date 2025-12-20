using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class RateViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context = new Infrastructure.MobileOperator(App.DbOptions);

        private RateModel rate;

        public RateViewModel()
        {
            rate = new RateModel(_context);
        }

        public RateViewModel(RateModel r)
        {
            rate = r;
        }

        public RateViewModel(int id)
        {
            rate = new RateModel(id, _context);
        }

        public string Name { get => rate.Name; set => rate.Name = value; }
        public int Id { get => rate.Id; set => rate.Id = value; }
        public bool Corporate { get => rate.Corporate; set => rate.Corporate = value; }
        public decimal ConnectionCost { get => rate.ConnectionCost; set => rate.ConnectionCost = value; }
        public decimal Cost { get => rate.Cost; set => rate.Cost = value; }
        public int Minutes { get => rate.Minutes; set => rate.Minutes = value; }
        public int GB { get => rate.GB; set => rate.GB = value; }
        public int SMS { get => rate.SMS; set => rate.SMS = value; }
        public decimal CityCost { get => rate.CityCost; set => rate.CityCost = value; }
        public decimal IntercityCost { get => rate.IntercityCost; set => rate.IntercityCost = value; }
        public decimal InternationalCost { get => rate.InternationalCost; set => rate.InternationalCost = value; }
        public decimal SMSCost { get => rate.SMSCost; set => rate.SMSCost = value; }
        public decimal GBCost { get => rate.GBCost; set => rate.GBCost = value; }

        private int clientId;
        public int ClientId { get => clientId; set => clientId = value; }

        private RelayCommand changeRateCommand;
        public RelayCommand ChangeRateCommand
        {
            get
            {
                return changeRateCommand ??
                  (changeRateCommand = new RelayCommand(obj =>
                  {
                      int? rateId = obj as int?;
                      ClientModel client = new ClientModel(clientId, _context);

                      if (rateId != null)
                      {
                          if (client.RateId == (int)rateId)
                              MessageBox.Show("Вы уже подключены к данному тарифу!");
                          else
                          {
                              RateModel selectedRate = new RateModel((int)rateId, _context);
                              decimal totalCost = selectedRate.ConnectionCost + selectedRate.Cost;

                              if (client.Balance < totalCost)
                                  MessageBox.Show("На вашем счету недостаточно средств для подключения тарифа!");
                              else
                              {
                                  client.Balance -= totalCost;
                                  
                                  if (totalCost > 0)
                                  {
                                      var clientEntity = _context.Client.FirstOrDefault(c => c.UserId == clientId);
                                      if (clientEntity != null)
                                      {
                                          var writeOff = new MobileOperator.Domain.Entities.WriteOff
                                          {
                                              ClientId = clientEntity.UserId,
                                              Amount = totalCost,
                                              WriteOffDate = System.DateTime.UtcNow,
                                              Category = "Смена тарифа",
                                              Description = $"Подключение тарифа '{selectedRate.Name}'"
                                          };
                                          _context.WriteOff.Add(writeOff);
                                      }
                                  }

                                  if (client.ChangeRate(selectedRate.Id))
                                  {
                                      MessageBox.Show("Подключение прошло успешно!");
                                      AppViewModel.UpdateBalance();
                                  }
                                  else
                                      MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
                              }
                          }
                      }
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
