using System;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MobileOperator.models
{
    public class ClientModel : UserModel
    {
        protected Client _client;

        public ClientModel(Infrastructure.MobileOperator context) : base(context)
        {
            _client = new Client();
        }

        public ClientModel(Client client, Infrastructure.MobileOperator context) : base(client.User, context)
        {
            _client = client;
        }

        public ClientModel(int id, Infrastructure.MobileOperator context) : base(id, context)
        {
            _client = _context.Client.FirstOrDefault(c => c.UserId == id) ?? new Client();
        }

        public Client Entity => _client;

        public override string Number
        {
            get { return _client?.Number; }
            set
            {
                if (_client != null)
                {
                    _client.Number = value;
                    OnPropertyChanged("Number");
                }
            }
        }

        public decimal Balance
        {
            get { return _client?.Balance ?? 0; }
            set
            {
                if (_client != null)
                {
                    _client.Balance = value;
                    OnPropertyChanged("Balance");
                }
            }
        }

        public int Minutes
        {
            get { return _client?.Minutes ?? 0; }
            set
            {
                if (_client != null)
                {
                    _client.Minutes = value;
                    OnPropertyChanged("Minutes");
                }
            }
        }

        public int SMS
        {
            get { return _client?.SMS ?? 0; }
            set
            {
                if (_client != null)
                {
                    _client.SMS = value;
                    OnPropertyChanged("SMS");
                }
            }
        }

        public int GB
        {
            get { return _client?.GB ?? 0; }
            set
            {
                if (_client != null)
                {
                    _client.GB = value;
                    OnPropertyChanged("GB");
                }
            }
        }

        public int RateId
        {
            get { return _client?.RateId ?? 0; }
            set
            {
                if (_client != null)
                {
                    _client.RateId = value;
                    OnPropertyChanged("RateId");
                }
            }
        }

        public virtual string OrganizationName { get; set; }
        public virtual string Address { get; set; }
        public virtual string FIO { get; set; }
        public virtual string PassportDetails { get; set; }

        public override bool Save()
        {
            if (_context == null || _client == null) return false;

            try
            {
                var entry = _context.Entry(_client);
                if (entry.State == EntityState.Detached)
                {
                    _context.Client.Update(_client);
                }

                return _context.SaveChanges() >= 0;
            }
            catch
            {
                return false;
            }
        }

        public bool ChangeRate(int newRateId)
        {
            this.RateId = newRateId;
            return AddRateHistory();
        }

        protected bool AddRateHistory()
        {
            if (_context == null || _client == null) return false;

            var clientId = _client.UserId;
            
            var oldHistory = _context.RateHistory
                .FirstOrDefault(i => i.ClientId == clientId && i.TillDate == null);

            if (oldHistory != null)
            {
                oldHistory.TillDate = DateTime.UtcNow;
            }
            
            if (this.RateId <= 0)
            {
                return _context.SaveChanges() >= 0;
            }

            var newHistory = new RateHistory
            {
                ClientId = clientId,
                RateId = this.RateId,
                FromDate = DateTime.UtcNow,
                TillDate = null
            };

            _context.RateHistory.Add(newHistory);

            return _context.SaveChanges() >= 0;
        }
    }
}
