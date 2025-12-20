using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using MobileOperator.Domain.Entities;
using MobileOperator.models;

namespace MobileOperator.models
{
    class Detailing2Model : INotifyPropertyChanged
    {
        private List<RateHistoryModel> allRates;
        private List<ServiceHistoryModel> allServices;
        private readonly Infrastructure.MobileOperator _context;

        public Detailing2Model(int clientId, DateTime from, DateTime till, Infrastructure.MobileOperator context)
        {
            _context = context;
            allRates = new List<RateHistoryModel>();
            allServices = new List<ServiceHistoryModel>();
            Search(clientId, from, till);
        }
        
        public Detailing2Model(string clientName, string clientNumber, DateTime from, DateTime till, Infrastructure.MobileOperator context)
        {
            _context = context;
            allRates = new List<RateHistoryModel>();
            allServices = new List<ServiceHistoryModel>();
            ClientModel client = new ClientModel(_context);
            
            if (!string.IsNullOrEmpty(clientName) && !string.IsNullOrEmpty(clientNumber))
            {
                var c = _context.Client.FirstOrDefault(i => i.Number == clientNumber);
                if (c != null)
                {
                    var fl = _context.FL.FirstOrDefault(f => f.UserId == c.UserId);
                    var ul = _context.UL.FirstOrDefault(u => u.UserId == c.UserId);

                    if (fl != null && fl.FIO == clientName)
                        client = new FLModel(fl, _context);
                    else if (ul != null && ul.OrganizationName == clientName)
                        client = new ULModel(ul, _context);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(clientName))
                {
                    var fl = _context.FL.FirstOrDefault(i => i.FIO == clientName);
                    if (fl != null)
                        client = new FLModel(fl, _context);
                    else
                    {
                        var ul = _context.UL.FirstOrDefault(i => i.OrganizationName == clientName);
                        if (ul != null)
                            client = new ULModel(ul, _context);
                    }
                }
                if (!string.IsNullOrEmpty(clientNumber))
                {
                    var c = _context.Client.FirstOrDefault(i => i.Number == clientNumber);
                    if (c != null)
                    {
                        var fl = _context.FL.FirstOrDefault(f => f.UserId == c.UserId);
                        if (fl != null)
                            client = new FLModel(fl, _context);
                        else
                        {
                            var ul = _context.UL.FirstOrDefault(u => u.UserId == c.UserId);
                            if (ul != null)
                                client = new ULModel(ul, _context);
                        }
                    }
                }
            }

            if (client.Id != 0)
            {
                Search(client.Id, from, till);
            }
        }

        private void Search(int clientId, DateTime from, DateTime till)
        {
            var rates = _context.RateHistory
                .Include(r => r.Rate)
                .Where(r => r.ClientId == clientId)
                .Where(r => r.FromDate <= till && (r.TillDate == null || r.TillDate >= from))
                .OrderByDescending(r => r.FromDate)
                .ToList();

            allRates = new List<RateHistoryModel>();
            foreach (var item in rates)
            {
                allRates.Add(new RateHistoryModel(item, _context));
            }
            
            var services = _context.ServiceHistory
                .Include(s => s.Service)
                .Where(s => s.ClientId == clientId)
                .Where(s => s.FromDate <= till && (s.TillDate == null || s.TillDate >= from))
                .OrderByDescending(s => s.FromDate)
                .ToList();

            allServices = new List<ServiceHistoryModel>();
            foreach (var item in services)
            {
                allServices.Add(new ServiceHistoryModel(item, _context));
            }
        }

        public List<RateHistoryModel> AllRates
        {
            get { return allRates; }
        }
        public List<ServiceHistoryModel> AllServices
        {
            get { return allServices; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
