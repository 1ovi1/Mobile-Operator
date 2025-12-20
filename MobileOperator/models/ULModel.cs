using System.Linq;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class ULModel : ClientModel
    {
        private UL _ul = new UL();

        public ULModel(Infrastructure.MobileOperator context) : base(context) { }

        public ULModel(UL ul, Infrastructure.MobileOperator context) : base(ul.UserId, context)
        {
            _ul = ul;
            if (_client == null || _client.UserId == 0)
                _client = _context.Client.FirstOrDefault(c => c.UserId == ul.UserId);
        }

        public ULModel(int id, Infrastructure.MobileOperator context) : base(id, context)
        {
            _ul = _context.UL.FirstOrDefault(i => i.UserId == id) ?? new UL();
        }

        public override string OrganizationName
        {
            get { return _ul?.OrganizationName; }
            set
            {
                if (_ul != null)
                {
                    _ul.OrganizationName = value;
                    OnPropertyChanged();
                }
            }
        }

        public override string Address
        {
            get { return _ul?.Address; }
            set
            {
                if (_ul != null)
                {
                    _ul.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public override bool Save()
        {
            if (_context == null) return false;

            var ulEntity = _context.UL.FirstOrDefault(u => u.UserId == Id);
            var clientEntity = _context.Client.FirstOrDefault(c => c.UserId == Id);

            if (ulEntity != null && clientEntity != null)
            {
                clientEntity.Balance = Balance;
                clientEntity.RateId = RateId;
                clientEntity.Minutes = Minutes;
                clientEntity.Number = Number;
                clientEntity.SMS = SMS;
                clientEntity.GB = GB;

                ulEntity.Address = this.Address;
                ulEntity.OrganizationName = this.OrganizationName;

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

                UL newUl = new UL()
                {
                    Address = this.Address,
                    OrganizationName = this.OrganizationName,
                    UserId = u.Id
                };
                _context.UL.Add(newUl);

                if (_context.SaveChanges() > 0)
                {
                    this.Id = u.Id;
                    _client = c;
                    _ul = newUl;
                    return AddRateHistory();
                }
                return false;
            }
        }

        public override bool Remove()
        {
            if (_context == null) return false;

            var clientEntity = _context.Client.FirstOrDefault(c => c.UserId == this.Id);

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

            var ulEntity = _context.UL.FirstOrDefault(u => u.UserId == this.Id);
            if (ulEntity != null) _context.UL.Remove(ulEntity);

            var userEntity = _context.User.FirstOrDefault(u => u.Id == this.Id);
            if (userEntity != null) _context.User.Remove(userEntity);

            return _context.SaveChanges() > 0;
        }
    }
}
