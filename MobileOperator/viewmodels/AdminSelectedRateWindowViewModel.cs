using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class AdminSelectesRateWindowViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;
        private RateModel rate;
        private bool createMod = false;
        private Window window;
        
        public AdminSelectesRateWindowViewModel(int rateId, Window window, Infrastructure.MobileOperator context)
        {
            _context = context;
            this.window = window;
            rate = new RateModel(rateId, _context);
            createMod = false;
        }
        
        public AdminSelectesRateWindowViewModel(Window window, Infrastructure.MobileOperator context)
        {
            _context = context;
            this.window = window;
            rate = new RateModel(_context) { Name = "Новый тариф" };
            createMod = true;
        }

        public int Id
        {
            get => rate.Id;
            set => rate.Id = value;
        }

        public string Rate
        {
            get => rate.Name ?? "";
            set
            {
                rate.Name = value;
                OnPropertyChanged("Rate");
            }
        }

        public bool Corporate
        {
            get => rate.Corporate;
            set => rate.Corporate = value;
        }

        public decimal ConnectionCost
        {
            get => rate.ConnectionCost;
            set
            {
                rate.ConnectionCost = value;
                OnPropertyChanged("ConnectionCost");
            }
        }

        public decimal Cost
        {
            get => rate.Cost;
            set
            {
                rate.Cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public int Minutes
        {
            get => rate.Minutes;
            set
            {
                rate.Minutes = value;
                OnPropertyChanged("Minutes");
            }
        }

        public int GB
        {
            get => rate.GB;
            set
            {
                rate.GB = value;
                OnPropertyChanged("GB");
            }
        }

        public int SMS
        {
            get => rate.SMS;
            set
            {
                rate.SMS = value;
                OnPropertyChanged("SMS");
            }
        }

        public decimal CityCost
        {
            get => rate.CityCost;
            set
            {
                rate.CityCost = value;
                OnPropertyChanged("CityCost");
            }
        }

        public decimal IntercityCost
        {
            get => rate.IntercityCost;
            set
            {
                rate.IntercityCost = value;
                OnPropertyChanged("IntercityCost");
            }
        }

        public decimal InternationalCost
        {
            get => rate.InternationalCost;
            set
            {
                rate.InternationalCost = value;
                OnPropertyChanged("InternationalCost");
            }
        }

        public decimal SMSCost
        {
            get => rate.SMSCost;
            set
            {
                rate.SMSCost = value;
                OnPropertyChanged("SMSCost");
            }
        }

        public decimal GBCost
        {
            get => rate.GBCost;
            set
            {
                rate.GBCost = value;
                OnPropertyChanged("GBCost");
            }
        }

        public bool Create => createMod;

        private RelayCommand updateRateCommand;
        public RelayCommand UpdateRateCommand
        {
            get
            {
                return updateRateCommand ??
                  (updateRateCommand = new RelayCommand(obj =>
                  {
                      if (rate.Save())
                      {
                          MessageBox.Show("Сохранение прошло успешно!");
                          window.Close();
                      }
                      else
                      {
                          MessageBox.Show("Произошла ошибка при сохранении!");
                      }
                  }));
            }
        }

        private RelayCommand removeRateCommand;
        public RelayCommand RemoveRateCommand
        {
            get
            {
                return removeRateCommand ??
                  (removeRateCommand = new RelayCommand(obj =>
                  {
                      if (MessageBox.Show("Вы уверены, что хотите удалить этот тариф?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          if (rate.Remove())
                          {
                              MessageBox.Show("Тариф удален!");
                              window.Close();
                          }
                          else
                          {
                              MessageBox.Show("Ошибка при удалении!");
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
