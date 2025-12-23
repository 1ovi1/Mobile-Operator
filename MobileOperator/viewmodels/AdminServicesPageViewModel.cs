using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    public class AdminServicesViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;
        public ObservableCollection<ServiceModel> Services { get; set; }

        public AdminServicesViewModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            Services = new ObservableCollection<ServiceModel>();
            LoadServices();
        }

        private void LoadServices()
        {
            Services.Clear();
            var dbServices = _context.Service.ToList();
            foreach (var s in dbServices)
            {
                Services.Add(new ServiceModel(s, _context));
            }
        }

        private ServiceModel _selectedService;
        public ServiceModel SelectedService
        {
            get => _selectedService;
            set
            {
                _selectedService = value;
                OnPropertyChanged();
            }
        }
        
        private RelayCommand _addServiceCommand;
        public RelayCommand AddServiceCommand
        {
            get
            {
                return _addServiceCommand ?? (_addServiceCommand = new RelayCommand(obj =>
                {
                    views.AdminSelectedServiceWindow serviceWindow = new views.AdminSelectedServiceWindow(_context);
                    serviceWindow.ShowDialog();
                    LoadServices();
                }));
            }
        }
        
        private RelayCommand _viewServiceCommand;
        public RelayCommand ViewServiceCommand
        {
            get
            {
                return _viewServiceCommand ?? (_viewServiceCommand = new RelayCommand(obj =>
                {
                    ServiceModel target = obj as ServiceModel ?? SelectedService;

                    if (target != null)
                    {
                        views.AdminSelectedServiceWindow serviceWindow = new views.AdminSelectedServiceWindow(target.Id, _context);
                        
                        serviceWindow.ShowDialog();
                        
                        LoadServices();
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
