using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class ViewRateWindowViewModel : INotifyPropertyChanged
    {
        private int rateId, userId;
        private RateModel rate;
        private ClientModel client;
        private Window window;
        
        private readonly Infrastructure.MobileOperator _context = new Infrastructure.MobileOperator(App.DbOptions);

        public ViewRateWindowViewModel(int userId, int rateId, Window window)
        {
            this.rateId = rateId;
            this.userId = userId;
            this.window = window;
            
            rate = new RateModel(rateId, _context);
            client = new ClientModel(userId, _context);
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
        public decimal CityCost
        {
            get { return rate.CityCost; }
            set { rate.CityCost = value; }
        }
        public decimal IntercityCost
        {
            get { return rate.IntercityCost; }
            set { rate.IntercityCost = value; }
        }
        public decimal InternationalCost
        {
            get { return rate.InternationalCost; }
            set { rate.InternationalCost = value; }
        }
        public int GB
        {
            get { return rate.GB; }
            set { rate.GB = value; }
        }
        public int SMS
        {
            get { return rate.SMS; }
            set { rate.SMS = value; }
        }
        public int Minutes
        {
            get { return rate.Minutes; }
            set { rate.Minutes = value; }
        }
        public decimal ConnectionCost
        {
            get { return rate.ConnectionCost; }
            set { rate.ConnectionCost = value; }
        }
        public decimal SMSCost
        {
            get { return rate.SMSCost; }
            set { rate.SMSCost = value; }
        }
        public decimal GBCost
        {
            get { return rate.GBCost; }
            set { rate.GBCost = value; }
        }

        private RelayCommand changeRateCommand;
        public RelayCommand ChangeRateCommand
        {
            get
            {
                return changeRateCommand ??
                  (changeRateCommand = new RelayCommand(obj =>
                  {
                      if (client.RateId == rateId)
                          MessageBox.Show("Вы уже подключены к данному тарифу!");
                      else
                      {
                          if (client.Balance < rate.ConnectionCost + rate.Cost)
                              MessageBox.Show("На вашем счету недостаточно средств для подключения тарифа!");
                          else
                          {
                              client.Balance -= (rate.ConnectionCost + rate.Cost);
                              client.RateId = rate.Id;
                              
                              if (client.Save())
                              {
                                  MessageBox.Show("Подключение прошло успешно!");
                                  AppViewModel.UpdateBalance();
                              }
                              else
                                  MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
                          }
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
