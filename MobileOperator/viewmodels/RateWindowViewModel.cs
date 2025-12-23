using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class RateWindowViewModel : INotifyPropertyChanged
    {
        private int userId, status;
        private RateModel rate;
        private ClientModel client;

        private readonly Infrastructure.MobileOperator _context;
        private readonly DispatcherTimer _timer;

        private RateListModel allRates;

        RateModel selectRate;

        public ObservableCollection<RateModel> Rates { get; set; }

        public RateWindowViewModel(int userId, int status, Infrastructure.MobileOperator context)
        {
            this.userId = userId;
            this.status = status;
            _context = context;

            selectRate = new RateModel(_context);

            allRates = new RateListModel(_context);
            Rates = new ObservableCollection<RateModel> { };

            if (status == 2)
            {
                client = new ULModel(userId, _context);
                foreach (RateModel rate in allRates.AllCorporateRates)
                    AddRateToCollection(rate);
            }
            else
            {
                client = new FLModel(userId, _context);
                foreach (RateModel rate in allRates.AllNotCorporateRates)
                    AddRateToCollection(rate);
            }

            rate = new RateModel(client.RateId, _context);

            AppViewModel.BalanceUpdated += RefreshData;
            
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(2);
            _timer.Tick += (s, e) => RefreshData();
            _timer.Start();
        }

        private void RefreshData()
        {
            try
            {
                _context.ChangeTracker.Clear();

                var dbClient = _context.Client.FirstOrDefault(c => c.UserId == userId);
                if (dbClient != null)
                {
                    if (client.RateId != (int)dbClient.RateId)
                    {
                        client.RateId = (int)dbClient.RateId;
                    }
                }

                rate = new RateModel(client.RateId, _context);

                OnPropertyChanged("Rate");
                OnPropertyChanged("Cost");
                OnPropertyChanged("Minutes");
                OnPropertyChanged("SMS");
                OnPropertyChanged("GB");
                OnPropertyChanged("CityCost");
                OnPropertyChanged("IntercityCost");
                OnPropertyChanged("InternationalCost");
                OnPropertyChanged("SMSCost");
                OnPropertyChanged("GBCost");
                OnPropertyChanged("Corporate");
            }
            catch { }
        }

        private void AddRateToCollection(RateModel rate)
        {
            Rates.Add(new RateModel(_context)
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
                Corporate = rate.Corporate
            });
        }

        public RateModel SelectedRate
        {
            get { return selectRate; }
            set { selectRate = value; }
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
            get { return rate.Minutes; }
            set { rate.Minutes = value; }
        }
        public int SMS
        {
            get { return rate.SMS; }
            set { rate.SMS = value; }
        }
        public int GB
        {
            get { return rate.GB; }
            set { rate.GB = value; }
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
        public string Corporate
        {
            get
            {
                if (rate.Corporate == true) return "Корпоративный";
                else return "Некорпоративный";
            }
            set
            {
                if (value == "Корпоративный") rate.Corporate = true;
                else if (value == "Некорпоративный") rate.Corporate = false;
            }
        }

        private RelayCommand changeRateCommand;
        public RelayCommand ChangeRateCommand
        {
            get
            {
                return changeRateCommand ??
                       (changeRateCommand = new RelayCommand(obj =>
                       {
                           ChangeRateWindow changeRateWindow = new ChangeRateWindow(userId, _context);
                           changeRateWindow.Show();
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
