using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileOperator.models
{
    class ServiceListModel : INotifyPropertyChanged
    {
        private List<ServiceModel> allServices;
        private readonly Infrastructure.MobileOperator _context;

        public ServiceListModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            LoadServices();
        }

        private void LoadServices()
        {
            allServices = _context.Service
                .ToList()
                .Select(i => new ServiceModel(i, _context))
                .ToList();
        }

        public List<ServiceModel> AllServices
        {
            get { return allServices; }
        }

        public List<int> ClientServices(int clientId)
        {
            var clientServices = _context.ServiceHistory
                .Where(i => i.ClientId == clientId && i.TillDate == null)
                .Select(i => i.ServiceId)
                .ToList();

            return clientServices;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}