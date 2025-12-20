using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace MobileOperator.models
{
    class UserListModel : INotifyPropertyChanged
    {
        private List<ClientModel> allClients;
        private List<AdminModel> allAdmins;
        private readonly Infrastructure.MobileOperator _context;

        public UserListModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            LoadUsers();
        }

        private void LoadUsers()
        {
            allClients = _context.Client
                .Include(c => c.User)
                .ToList()
                .Select(c => new ClientModel(c, _context))
                .ToList();
            
            allAdmins = _context.Admin
                .Include(a => a.User)
                .ToList()
                .Select(a => new AdminModel(a, _context))
                .ToList();
        }

        public List<ClientModel> AllClients
        {
            get { return allClients; }
        }

        public List<AdminModel> AllAdmins
        {
            get { return allAdmins; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}