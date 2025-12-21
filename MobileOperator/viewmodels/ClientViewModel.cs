// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using System.ComponentModel;
// using System.Runtime.CompilerServices;
// using MobileOperator.Model;
// using System.Windows;
//
// namespace MobileOperator.ViewModel
// {
//     class ClientViewModel : INotifyPropertyChanged
//     {
//         private ClientModel client = new ClientModel();
//         public ClientViewModel() { }
//         public ClientViewModel(ClientModel c)
//         {
//             client = c;
//         }
//         public ClientViewModel(int id)
//         {
//             client = new ClientModel(id);
//         }
//         public string Number
//         {
//             get { return client.Number; }
//             set
//             {
//                 client.Number = value;
//             }
//         }
//         public decimal Balance
//         {
//             get { return (decimal)client.Balance; }
//             set
//             {
//                 client.Balance = value;
//             }
//         }
//         public int Minutes
//         {
//             get { return (int)client.Minutes; }
//             set
//             {
//                 client.Minutes = value;
//             }
//         }
//         public int SMS
//         {
//             get { return (int)client.SMS; }
//             set
//             {
//                 client.SMS = value;
//             }
//         }
//         public int GB
//         {
//             get { return (int)client.GB; }
//             set
//             {
//                 client.GB = value;
//             }
//         }
//         public int Rate
//         {
//             get { return (int)client.Rate; }
//             set
//             {
//                 client.Rate = value;
//             }
//         }
//         private string rateName;
//         public string RateName
//         {
//             get { return rateName; }
//             set { rateName = value; }
//         }
//
//         public string Corporate
//         {
//             get { if (client.Status == 2) return "Корпоративный"; else return "Некорпоративный"; }
//             set { }
//         }
//
//         public int Status
//         {
//             get { return client.Status; }
//             set { client.Status = value; }
//         }
//
//         public int Id
//         {
//             get { return client.Id; }
//             set { client.Id = value; }
//         }
//
//         public string OrganizationName
//         {
//             get { return client.OrganizationName; }
//             set { client.OrganizationName = value; }
//         }
//         public string FIO
//         {
//             get { return client.FIO; }
//             set { client.FIO = value; }
//         }
//         public string PasportDetales
//         {
//             get { return client.PasportDetales; }
//             set { client.PasportDetales = value; }
//         }
//         public string Address
//         {
//             get { return client.Address; }
//             set { client.Address = value; }
//         }
//
//         public string Name
//         {
//             get
//             {
//                 if (client.OrganizationName != null) return client.OrganizationName;
//                 else return client.FIO;
//             }
//             set { }
//         }
//
//
//         public event PropertyChangedEventHandler PropertyChanged;
//         public void OnPropertyChanged([CallerMemberName] string prop = "")
//         {
//             if (PropertyChanged != null)
//                 PropertyChanged(this, new PropertyChangedEventArgs(prop));
//         }
//     }
// }
