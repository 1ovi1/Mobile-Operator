using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class ServiceHistoryModel
    {
        private ServiceHistory _serviceHistory;
        private readonly Infrastructure.MobileOperator _context;

        public ServiceHistoryModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            _serviceHistory = new ServiceHistory();
        }

        public ServiceHistoryModel(ServiceHistory history)
        {
            _serviceHistory = history;
        }

        public ServiceHistoryModel(ServiceHistory history, Infrastructure.MobileOperator context)
        {
            _context = context;
            _serviceHistory = history;
        }

        public int Id
        {
            get { return _serviceHistory.Id; }
            set { _serviceHistory.Id = value; }
        }

        public int ClientId
        {
            get { return _serviceHistory.ClientId; }
            set { _serviceHistory.ClientId = value; }
        }

        public int ServiceId
        {
            get { return _serviceHistory.ServiceId; }
            set { _serviceHistory.ServiceId = value; }
        }

        public string ServiceName
        {
            get
            {
                if (_serviceHistory.Service != null)
                    return _serviceHistory.Service.Name;

                if (_context != null)
                {
                    return _context.Service.FirstOrDefault(s => s.Id == _serviceHistory.ServiceId)?.Name;
                }
                return "Неизвестно";
            }
        }

        public DateTime FromDate
        {
            get { return _serviceHistory.FromDate; }
            set { _serviceHistory.FromDate = value; }
        }

        public string FromDateToString => _serviceHistory.FromDate.ToString("G");

        public DateTime? TillDate
        {
            get { return _serviceHistory.TillDate; }
            set { _serviceHistory.TillDate = value; }
        }

        public string TillDateToString => _serviceHistory.TillDate?.ToString("G") ?? "";
    }
}
