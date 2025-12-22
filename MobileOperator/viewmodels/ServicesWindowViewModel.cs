using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.models;
using MobileOperator.views;

namespace MobileOperator.viewmodels
{
    class ServicesWindowViewModel : INotifyPropertyChanged
    {
        private int userId, status;

        private readonly Infrastructure.MobileOperator _context;

        private ServiceListModel services;
        private ClientModel client;
        
        public ClientModel Client => client;
        public ServiceListModel Services => services;

        public ObservableCollection<ServiceViewModel> ConnectionServices { get; set; }
        public ObservableCollection<ServiceViewModel> AvailableServices { get; set; }
        
        public ServicesWindowViewModel(int userId, int status, Infrastructure.MobileOperator context)
        {
            this.userId = userId;
            this.status = status;
            _context = context;
            
            services = new ServiceListModel(_context);

            if (status == 2)
                client = new ULModel(userId, _context);
            else
                client = new FLModel(userId, _context);

            ConnectionServices = new ObservableCollection<ServiceViewModel> { };
            AvailableServices = new ObservableCollection<ServiceViewModel> { };

            foreach (int serviseId in services.ClientServices(userId))
                ConnectionServices.Add(new ServiceViewModel(serviseId, _context) { ClientId = userId, ServicesList = this });

            bool flag;
            foreach (ServiceModel service in services.AllServices)
            {
                flag = false;
                foreach (int serviseId in services.ClientServices(userId))
                    if (service.Id == serviseId)
                        flag = true;
                if (!flag)
                    AvailableServices.Add(new ServiceViewModel(_context)
                    {
                        Name = service.Name,
                        Cost = service.Cost,
                        ConnectionCost = service.ConnectionCost,
                        Conditions = service.Conditions,
                        Id = service.Id,
                        ClientId = userId,
                        ServicesList = this
                    });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
