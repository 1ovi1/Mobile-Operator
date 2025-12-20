using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class RateHistoryModel
    {
        private RateHistory _rateHistory;
        private readonly Infrastructure.MobileOperator _context;

        public RateHistoryModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            _rateHistory = new RateHistory();
        }

        public RateHistoryModel(RateHistory r)
        {
            _rateHistory = r;
        }

        public RateHistoryModel(RateHistory r, Infrastructure.MobileOperator context)
        {
            _context = context;
            _rateHistory = r;
        }

        public int Id
        {
            get { return _rateHistory.Id; }
            set { _rateHistory.Id = value; }
        }
        public int ClientId
        {
            get { return _rateHistory.ClientId; }
            set { _rateHistory.ClientId = value; }
        }
        public int RateId
        {
            get { return _rateHistory.RateId; }
            set { _rateHistory.RateId = value; }
        }
        public string RateName
        {
            get
            {
                if (_rateHistory.Rate != null) return _rateHistory.Rate.Name;
                if (_context != null)
                {
                    return _context.Rate.FirstOrDefault(x => x.Id == _rateHistory.RateId)?.Name;
                }
                return "Неизвестно";
            }
        }
        public DateTime FromDate
        {
            get { return _rateHistory.FromDate; }
            set { _rateHistory.FromDate = value; }
        }
        public string FromDateToString => _rateHistory.FromDate.ToString("G");
        
        public DateTime? TillDate
        {
            get { return _rateHistory.TillDate; }
            set { _rateHistory.TillDate = value; }
        }
        public string TillDateToString => _rateHistory.TillDate?.ToString("G") ?? "";
    }
}