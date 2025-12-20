using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    class ClientListModel : INotifyPropertyChanged
    {
        private List<ClientModel> allClients = new List<ClientModel>();
        private List<UL> allULs;
        private List<FL> allFLs;
        private readonly Infrastructure.MobileOperator _context;

        public ClientListModel(Infrastructure.MobileOperator context)
        {
            _context = context;

            allFLs = _context.FL.ToList();
            allULs = _context.UL.ToList();

            foreach (UL ul in allULs)
                allClients.Add(new ULModel(ul, _context));

            foreach (FL fl in allFLs)
                allClients.Add(new FLModel(fl, _context));

            allClients = allClients.OrderBy(x => x.Id).ToList();
        }

        public ClientListModel(int rateId, Infrastructure.MobileOperator context)
        {
            _context = context;
            allClients = _context.Client
                .Where(i => i.RateId == rateId)
                .ToList()
                .Select(i => new ClientModel(i, _context))
                .ToList();
        }

        public List<ClientModel> AllClients
        {
            get { return allClients; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}