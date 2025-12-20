using System.Linq;
using MobileOperator.Domain.Entities;

namespace MobileOperator.models
{
    public class AdminModel : UserModel
    {
        private Admin _admin = new Admin();

        public AdminModel(Infrastructure.MobileOperator context) : base(context) 
        { 
        }
        
        public AdminModel(Admin a, Infrastructure.MobileOperator context) : base(a.User, context)
        {
            _admin = a;
        }

        public AdminModel(Admin a, User u, Infrastructure.MobileOperator context) : base(u, context)
        {
            _admin = a;
        }

        public AdminModel(int id, Infrastructure.MobileOperator context) : base(id, context)
        {
            _admin = _context.Admin.FirstOrDefault(i => i.UserId == id);
        }

        public string Login
        {
            get { return _admin?.Login; }
            set
            {
                if (_admin != null)
                {
                    _admin.Login = value;
                    OnPropertyChanged("AdminLogin"); 
                }
            }
        }
    }
}