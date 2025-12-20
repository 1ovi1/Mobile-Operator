using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class ServiceModel : INotifyPropertyChanged
    {
        private Service _service;
        private readonly Infrastructure.MobileOperator _context;

        public ServiceModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            _service = new Service();
        }

        public ServiceModel(Service service, Infrastructure.MobileOperator context)
        {
            _context = context;
            _service = service;
        }

        public ServiceModel(int id, Infrastructure.MobileOperator context)
        {
            _context = context;
            _service = _context.Service.FirstOrDefault(s => s.Id == id) ?? new Service();
        }

        public Service Entity => _service;

        public int Id
        {
            get => _service.Id;
            set
            {
                if (_service.Id != value)
                {
                    _service.Id = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _service.Name;
            set
            {
                _service.Name = value;
                OnPropertyChanged();
            }
        }

        public decimal Cost
        {
            get => _service.Cost ?? 0;
            set
            {
                _service.Cost = value;
                OnPropertyChanged();
            }
        }

        public decimal ConnectionCost
        {
            get => _service.ConnectionCost ?? 0;
            set
            {
                _service.ConnectionCost = value;
                OnPropertyChanged();
            }
        }

        public string Conditions
        {
            get => _service.Conditions;
            set
            {
                _service.Conditions = value;
                OnPropertyChanged();
            }
        }
        

        public bool ConnectService(int clientId)
        {
            if (_context == null) return false;
            
            if (_context.ServiceHistory.Any(h => h.ClientId == clientId && h.ServiceId == this.Id && h.TillDate == null))
            {
                return false;
            }
            
            var history = new ServiceHistory()
            {
                ClientId = clientId,
                ServiceId = this.Id,
                FromDate = DateTime.UtcNow,
                TillDate = null
            };

            _context.ServiceHistory.Add(history);
            return _context.SaveChanges() > 0;
        }

        public bool DisconnectService(int clientId)
        {
            if (_context == null) return false;
            
            var activeService = _context.ServiceHistory
                .FirstOrDefault(i => i.ClientId == clientId && 
                                     i.ServiceId == this.Id && 
                                     i.TillDate == null);

            if (activeService != null)
            {
                activeService.TillDate = DateTime.UtcNow;
                return _context.SaveChanges() > 0;
            }
            
            return false;
        }

        public bool Save()
        {
            if (_context == null) return false;

            var s = _context.Service.FirstOrDefault(x => x.Id == Id);
            if (s != null)
            {
                s.Name = this.Name;
                s.Cost = this.Cost;
                s.ConnectionCost = this.ConnectionCost;
                s.Conditions = this.Conditions;
            }
            else
            {
                s = new Service()
                {
                    Name = this.Name,
                    Cost = this.Cost,
                    ConnectionCost = this.ConnectionCost,
                    Conditions = this.Conditions
                };
                _context.Service.Add(s);
            }

            return _context.SaveChanges() > 0;
        }

        public bool Remove()
        {
            if (_context == null) return false;

            var s = _context.Service.FirstOrDefault(x => x.Id == Id);
            if (s == null) return false;
            
            var history = _context.ServiceHistory.Where(h => h.ServiceId == s.Id).ToList();
            _context.ServiceHistory.RemoveRange(history);

            _context.Service.Remove(s);
            return _context.SaveChanges() > 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
