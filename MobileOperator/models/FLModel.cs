using System.Linq;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class FLModel : ClientModel
    {
        private FL _fl = new FL();

        public FLModel(Infrastructure.MobileOperator context) : base(context) { }

        public FLModel(FL fl, Infrastructure.MobileOperator context) : base(fl.UserId, context)
        {
            _fl = fl;
        }

        public FLModel(int id, Infrastructure.MobileOperator context) : base(id, context)
        {
            _fl = _context.FL.FirstOrDefault(i => i.UserId == id) ?? new FL();
        }

        public override string FIO
        {
            get { return _fl?.FIO; }
            set
            {
                if (_fl != null)
                {
                    _fl.FIO = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string PassportDetails
        {
            get { return _fl?.PassportDetails; }
            set
            {
                if (_fl != null)
                {
                    _fl.PassportDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        public override bool Save()
        {
            if (_context == null) return false;

            var flEntity = _context.FL.FirstOrDefault(u => u.UserId == Id);
            var clientEntity = _context.Client.FirstOrDefault(c => c.UserId == Id);

            if (flEntity != null && clientEntity != null)
            {
                clientEntity.Balance = Balance;
                clientEntity.RateId = RateId;
                clientEntity.Minutes = Minutes;
                clientEntity.Number = Number;
                clientEntity.SMS = SMS;
                clientEntity.GB = GB;

                flEntity.FIO = this.FIO;
                flEntity.PassportDetails = this.PassportDetails;

                return _context.SaveChanges() > 0;
            }
            else
            {
                User u = new User() { Password = this.Password };
                _context.User.Add(u);
                _context.SaveChanges();

                Client c = new Client()
                {
                    Balance = this.Balance,
                    RateId = this.RateId,
                    Minutes = this.Minutes,
                    Number = this.Number,
                    SMS = this.SMS,
                    GB = this.GB,
                    UserId = u.Id
                };
                _context.Client.Add(c);

                FL newFl = new FL()
                {
                    FIO = this.FIO,
                    PassportDetails = this.PassportDetails,
                    UserId = u.Id
                };
                _context.FL.Add(newFl);

                if (_context.SaveChanges() > 0)
                {
                    this.Id = u.Id;
                    return AddRateHistory();
                }
                return false;
            }
        }

        private bool AddRateHistory()
        {
            if (RateId == 0) return true;
            var history = new RateHistory
            {
                ClientId = this.Id,
                RateId = this.RateId,
                FromDate = System.DateTime.Now
            };
            _context.RateHistory.Add(history);
            return _context.SaveChanges() > 0;
        }

        public override bool Remove()
        {
            if (_context == null) return false;

            var flEntity = _context.FL.FirstOrDefault(u => u.UserId == this.Id);
            var clientEntity = _context.Client.FirstOrDefault(c => c.UserId == this.Id);
            var userEntity = _context.User.FirstOrDefault(u => u.Id == this.Id);

            if (clientEntity != null)
            {
                var rateHistory = _context.RateHistory.Where(i => i.ClientId == clientEntity.UserId).ToList();
                _context.RateHistory.RemoveRange(rateHistory);
                var serviceHistory = _context.ServiceHistory.Where(i => i.ClientId == clientEntity.UserId).ToList();
                _context.ServiceHistory.RemoveRange(serviceHistory);
                var calls = _context.Call.Where(i => i.CallerId == clientEntity.UserId || i.CalledId == clientEntity.UserId).ToList();
                _context.Call.RemoveRange(calls);
                _context.Client.Remove(clientEntity);
            }
            if (flEntity != null) _context.FL.Remove(flEntity);
            if (userEntity != null) _context.User.Remove(userEntity);

            return _context.SaveChanges() > 0;
        }
    }
}
