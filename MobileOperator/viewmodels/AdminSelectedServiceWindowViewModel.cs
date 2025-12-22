using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class AdminSelectesServiceWindowViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;
        private ServiceModel service;
        private bool createMod;
        private Window window;
        
        public AdminSelectesServiceWindowViewModel(int serviceId, Window window, Infrastructure.MobileOperator context)
        {
            _context = context;
            this.window = window;
            service = new ServiceModel(serviceId, _context);
            createMod = false;
        }
        
        public AdminSelectesServiceWindowViewModel(Window window, Infrastructure.MobileOperator context)
        {
            _context = context;
            this.window = window;
            service = new ServiceModel(_context) { Name = "Новая услуга" };
            createMod = true;
        }

        public int Id
        {
            get => service.Id;
            set => service.Id = value;
        }

        public string Name
        {
            get => service.Name ?? "";
            set
            {
                service.Name = value;
                OnPropertyChanged("Name");
            }
        }

        public decimal ConnectionCost
        {
            get => service.ConnectionCost;
            set
            {
                service.ConnectionCost = value;
                OnPropertyChanged("ConnectionCost");
            }
        }

        public decimal Cost
        {
            get => service.Cost;
            set
            {
                service.Cost = value;
                OnPropertyChanged("Cost");
            }
        }

        public string Condition
        {
            get => service.Conditions ?? "";
            set
            {
                service.Conditions = value;
                OnPropertyChanged("Condition");
            }
        }

        public bool Create => createMod;

        private RelayCommand updateServiceCommand;
        public RelayCommand UpdateServiceCommand
        {
            get
            {
                return updateServiceCommand ??
                  (updateServiceCommand = new RelayCommand(obj =>
                  {
                      if (service.Save())
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

        private RelayCommand removeServiceCommand;
        public RelayCommand RemoveServiceCommand
        {
            get
            {
                return removeServiceCommand ??
                  (removeServiceCommand = new RelayCommand(obj =>
                  {
                      if (MessageBox.Show("Вы уверены, что хотите удалить эту услугу?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          if (service.Remove())
                          {
                              MessageBox.Show("Услуга удалена!");
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
