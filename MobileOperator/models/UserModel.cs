using System.ComponentModel;
using System.Runtime.CompilerServices;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class UserModel : INotifyPropertyChanged
    {
        protected User _user = new User();
        protected int _status;

        protected readonly Infrastructure.MobileOperator _context;
        
        public UserModel(Infrastructure.MobileOperator context) 
        { 
            _context = context;
        }
        
        public UserModel(User u, Infrastructure.MobileOperator context)
        {
            _context = context;
            _user = u;
            _status = CheckStatus();
        }
        
        public UserModel(int id, Infrastructure.MobileOperator context)
        {
            _context = context;
            _user = _context.User.FirstOrDefault(i => i.Id == id);
            _status = CheckStatus();
        }

        private int CheckStatus()
        {
            if (_user == null || _user.Id == 0) return 3;
            
            if (_context.Admin.Any(a => a.UserId == _user.Id))
                return 1;
            
            if (_context.UL.Any(u => u.UserId == _user.Id))
                return 2;

            return 3; 
        }

        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public int Id
        {
            get { return _user?.Id ?? 0; }
            set 
            { 
                if (_user != null) _user.Id = value; 
            }
        }

        public string Password
        {
            get { return _user?.Password; }
            set 
            { 
                if (_user != null) _user.Password = value; 
            }
        }

        public virtual string Number { get; set; }

        public virtual bool Save() { return false; }
        public virtual bool Remove() { return false; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
