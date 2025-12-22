using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class WriteOffsPage : Page
    {
        public WriteOffsPage(int userId, int status, Infrastructure.MobileOperator context)
        {
            InitializeComponent();
            DataContext = new WriteOffsViewModel(userId, status, context);
        }
    }
}