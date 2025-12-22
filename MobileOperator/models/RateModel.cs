using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class RateModel : INotifyPropertyChanged
    {
        private Rate _rate;
        private readonly Infrastructure.MobileOperator _context;

        public RateModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            _rate = new Rate();
        }

        public RateModel(Rate r, Infrastructure.MobileOperator context)
        {
            _context = context;
            _rate = r;
        }

        public RateModel(int id, Infrastructure.MobileOperator context)
        {
            _context = context;
            _rate = _context.Rate.FirstOrDefault(x => x.Id == id) ?? new Rate();
        }

        public int Id { get => _rate.Id; set => _rate.Id = value; }
        public string Name { get => _rate.Name; set => _rate.Name = value; }
        public bool Corporate { get => _rate.Corporate ?? false; set => _rate.Corporate = value; }
        public decimal ConnectionCost { get => _rate.ConnectionCost ?? 0; set => _rate.ConnectionCost = value; }
        public decimal Cost { get => _rate.Cost ?? 0; set => _rate.Cost = value; }
        public int Minutes { get => _rate.Minutes ?? 0; set => _rate.Minutes = value; }
        public int GB { get => (int)_rate.GB; set => _rate.GB = value; }
        public int SMS { get => _rate.SMS ?? 0; set => _rate.SMS = value; }
        public decimal CityCost { get => _rate.CityCost ?? 0; set => _rate.CityCost = value; }
        public decimal IntercityCost { get => _rate.IntercityCost ?? 0; set => _rate.IntercityCost = value; }
        public decimal InternationalCost { get => _rate.InternationalCost ?? 0; set => _rate.InternationalCost = value; }
        public decimal SMSCost { get => _rate.SMSCost ?? 0; set => _rate.SMSCost = value; }
        public decimal GBCost { get => _rate.GBCost ?? 0; set => _rate.GBCost = value; }

        public bool Save()
        {
            if (_context == null) return false;
            
            if (_rate.Id == 0)
            {
                _context.Rate.Add(_rate);
            }
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool Remove()
        {
            if (_context == null || _rate.Id == 0) return false;
            
            var rateHistory = _context.RateHistory.Where(i => i.RateId == _rate.Id).ToList();
            if (rateHistory.Any())
            {
                _context.RateHistory.RemoveRange(rateHistory);
            }

            var clients = _context.Client.Where(i => i.RateId == _rate.Id).ToList();
            foreach (var client in clients)
            {
                client.RateId = null;
            }

            _context.Rate.Remove(_rate);
            
            try
            {
                return _context.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
