using System.Windows.Controls;
using MobileOperator.viewmodels;

namespace MobileOperator.views
{
    public partial class WriteOffsPage : Page
    {
        public WriteOffsPage(int userId, int status)
        {
            InitializeComponent();
            DataContext = new WriteOffsViewModel(userId, status);
        }
    }
}