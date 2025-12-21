using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using MobileOperator.Infrastructure;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    public class AdminServicesViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;
        public ObservableCollection<ServiceModel> Services { get; set; }

        public AdminServicesViewModel()
        {
            _context = new Infrastructure.MobileOperator(App.DbOptions);
            Services = new ObservableCollection<ServiceModel>();
            LoadServices();
        }

        private void LoadServices()
        {
            Services.Clear();
            var dbServices = _context.Service.ToList();
            foreach (var s in dbServices)
            {
                Services.Add(new ServiceModel(s, context: _context));
            }
        }

        private ServiceModel _selectedService;
        public ServiceModel SelectedService
        {
            get => _selectedService;
            set { _selectedService = value; OnPropertyChanged(); }
        }

        private RelayCommand _addServiceCommand;
        public RelayCommand AddServiceCommand
        {
            get
            {
                return _addServiceCommand ?? (_addServiceCommand = new RelayCommand(obj =>
                {
                    MessageBox.Show("Функционал добавления услуги");
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