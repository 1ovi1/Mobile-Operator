// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;
// using MobileOperator.Model;
// using System.Collections.ObjectModel;
// using System.ComponentModel;
// using System.Runtime.CompilerServices;
// using System.Windows;
// using MobileOperator.views;
//
// namespace MobileOperator.ViewModel
// {
//     class ViewClientWindowViewModel : INotifyPropertyChanged
//     {
//         private int clientId, clientStatus;
//         private ClientModel client;
//         private RateListModel allRates;
//         private Window window;
//         public ObservableCollection<RateModel> Rates { get; set; }
//         private bool createMode;
//         public ViewClientWindowViewModel(int clientId, int clientStatus, Window window)
//         {
//             this.clientId = clientId;
//             this.clientStatus = clientStatus;
//             this.window = window;
//             allRates = new RateListModel();
//             Rates = new ObservableCollection<RateModel> { };
//             if (clientStatus == 2)
//             {
//                 client = new ULModel(clientId);
//                 foreach (RateModel rate in allRates.AllCorporateRates)
//                     Rates.Add(new RateModel()
//                     {
//                         Name = rate.Name,
//                         Id = rate.Id
//                     });
//             }
//             else if (clientStatus == 3)
//             {
//                 client = new FLModel(clientId);
//                 foreach (RateModel rate in allRates.AllNotCorporateRates)
//                     Rates.Add(new RateModel()
//                     {
//                         Name = rate.Name,
//                         Id = rate.Id
//                     });
//             }
//             createMode = false;
//         }
//         public ViewClientWindowViewModel(Window window)
//         {
//             this.window = window;
//             allRates = new RateListModel();
//             Rates = new ObservableCollection<RateModel> { };
//             createMode = true;
//         }
//
//         bool statusFL = false, statusUL = false;
//         public bool ChectedFLStatus
//         {
//             get { return statusFL; }
//             set
//             {
//                 statusFL = value;
//                 client = new FLModel();
//                 client.Status = 3;
//                 if (Rates.Count != 0)
//                     Rates.Clear();
//                 foreach (RateModel rate in allRates.AllNotCorporateRates)
//                     Rates.Add(new RateModel()
//                     {
//                         Name = rate.Name,
//                         Id = rate.Id
//                     });
//                 OnPropertyChanged("ChectedFLStatus");
//             }
//         }
//         public bool ChectedULStatus
//         {
//             get { return statusUL; }
//             set
//             {
//                 statusUL = value;
//                 client = new ULModel();
//                 client.Status = 2;
//                 if (Rates.Count != 0)
//                     Rates.Clear();
//                 foreach (RateModel rate in allRates.AllCorporateRates)
//                     Rates.Add(new RateModel()
//                     {
//                         Name = rate.Name,
//                         Id = rate.Id
//                     });
//                 OnPropertyChanged("ChectedULStatus");
//             }
//         }
//         public bool Create
//         {
//             get { return createMode; }
//             set { }
//         }
//         public string FIO
//         {
//             get { if (client != null) return client.FIO; else return ""; }
//             set { client.FIO = value; OnPropertyChanged("FIO"); }
//         }
//         public string OrganizationName
//         {
//             get { if (client != null) return client.OrganizationName; else return ""; }
//             set { client.OrganizationName = value; OnPropertyChanged("OrganizationName"); }
//         }
//         public string PassportDetails
//         {
//             get { if (client != null) return client.PasportDetales; else return ""; }
//             set { client.PasportDetales = value; OnPropertyChanged("PassportDetails"); }
//         }
//         public string Address
//         {
//             get { if (client != null) return client.Address; else return ""; }
//             set { client.Address = value; OnPropertyChanged("Address"); }
//         }
//         public string Number
//         {
//             get { if (client != null) return client.Number; else return ""; }
//             set { client.Number = value; OnPropertyChanged("Number"); }
//         }
//         public int GB
//         {
//             get { if (client != null) return client.GB; else return 0; }
//             set { client.GB = value; OnPropertyChanged("GB"); }
//         }
//         public int SMS
//         {
//             get { if (client != null) return client.SMS; else return 0; }
//             set { client.SMS = value; OnPropertyChanged("SMS"); }
//         }
//         public int Minutes
//         {
//             get { if (client != null) return client.Minutes; else return 0; }
//             set { client.Minutes = value; OnPropertyChanged("Minutes"); }
//         }
//         public decimal Balance
//         {
//             get { if (client != null) return client.Balance; else return 0; }
//             set { client.Balance = value; OnPropertyChanged("Balance"); }
//         }
//         public int Rate
//         {
//             get { if (client != null) return client.Rate; else return 0; }
//             set { client.Rate = value; OnPropertyChanged("Rate"); }
//         }
//         public string Password
//         {
//             get { if (client != null) return client.Password; else return ""; }
//             set { client.Password = value; OnPropertyChanged("Password"); }
//         }
//         public int Status
//         {
//             get { if (client != null) return client.Status; else return 0; }
//             set { client.Status = value; }
//         }
//
//
//         private RelayCommand updateClientCommand;
//         public RelayCommand UpdateClientCommand
//         {
//             get
//             {
//                 return updateClientCommand ??
//                   (updateClientCommand = new RelayCommand(obj =>
//                   {
//                       if (client.Save())
//                       {
//                           AdminMainWindow main = new AdminMainWindow();
//                           main.WindowState = window.WindowState;
//                           main.Top = window.Top;
//                           main.Left = window.Left;
//                           main.Height = window.Height;
//                           main.Width = window.Width;
//                           main.Show();
//                           window.Close();
//                           MessageBox.Show("Сохренение прошло успешно!");
//                       }
//                       else
//                           MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
//                   }));
//             }
//         }
//         private RelayCommand delClientCommand;
//         public RelayCommand DelClientCommand
//         {
//             get
//             {
//                 return delClientCommand ??
//                   (delClientCommand = new RelayCommand(obj =>
//                   {
//                       if (client.Remove())
//                       {
//                           AdminMainWindow main = new AdminMainWindow();
//                           main.WindowState = window.WindowState;
//                           main.Top = window.Top;
//                           main.Left = window.Left;
//                           main.Height = window.Height;
//                           main.Width = window.Width;
//                           main.Show();
//                           window.Close();
//                           MessageBox.Show("Удаление прошло успешно!");
//                       }
//                       else
//                           MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
//                   }));
//             }
//         }
//         private RelayCommand openAdminMainWindow;
//         public RelayCommand OpenAdminMainWindow
//         {
//             get
//             {
//                 return openAdminMainWindow ??
//                   (openAdminMainWindow = new RelayCommand(obj =>
//                   {
//                       AdminMainWindow main = new AdminMainWindow();
//                       main.WindowState = window.WindowState;
//                       main.Top = window.Top;
//                       main.Left = window.Left;
//                       main.Height = window.Height;
//                       main.Width = window.Width;
//                       main.Show();
//                       window.Close();
//                   }));
//             }
//         }
//         private RelayCommand openAdminRatesWindow;
//         public RelayCommand OpenAdminRatesWindow
//         {
//             get
//             {
//                 return openAdminRatesWindow ??
//                   (openAdminRatesWindow = new RelayCommand(obj =>
//                   {
//                       AdminRatesWindow rates = new AdminRatesWindow();
//                       rates.WindowState = window.WindowState;
//                       rates.Top = window.Top;
//                       rates.Left = window.Left;
//                       rates.Height = window.Height;
//                       rates.Width = window.Width;
//                       rates.Show();
//                       window.Close();
//                   }));
//             }
//         }
//         private RelayCommand openAdminServicesWindow;
//         public RelayCommand OpenAdminServicesWindow
//         {
//             get
//             {
//                 return openAdminServicesWindow ??
//                   (openAdminServicesWindow = new RelayCommand(obj =>
//                   {
//                       AdminServicesWindow services = new AdminServicesWindow();
//                       services.WindowState = window.WindowState;
//                       services.Top = window.Top;
//                       services.Left = window.Left;
//                       services.Height = window.Height;
//                       services.Width = window.Width;
//                       services.Show();
//                       window.Close();
//                   }));
//             }
//         }
//         private RelayCommand openAdminDetailWindow;
//         public RelayCommand OpenAdminDetailWindow
//         {
//             get
//             {
//                 return openAdminDetailWindow ??
//                   (openAdminDetailWindow = new RelayCommand(obj =>
//                   {
//                       AdminDetailingWindow detailing = new AdminDetailingWindow();
//                       detailing.WindowState = window.WindowState;
//                       detailing.Top = window.Top;
//                       detailing.Left = window.Left;
//                       detailing.Height = window.Height;
//                       detailing.Width = window.Width;
//                       detailing.Show();
//                       window.Close();
//                   }));
//             }
//         }
//         private RelayCommand openAdminDetailWindow2;
//         public RelayCommand OpenAdminDetailWindow2
//         {
//             get
//             {
//                 return openAdminDetailWindow2 ??
//                   (openAdminDetailWindow2 = new RelayCommand(obj =>
//                   {
//                       AdminDetailingWindow2 detailing = new AdminDetailingWindow2();
//                       detailing.WindowState = window.WindowState;
//                       detailing.Top = window.Top;
//                       detailing.Left = window.Left;
//                       detailing.Height = window.Height;
//                       detailing.Width = window.Width;
//                       detailing.Show();
//                       window.Close();
//                   }));
//             }
//         }
//
//         public event PropertyChangedEventHandler PropertyChanged;
//         public void OnPropertyChanged([CallerMemberName] string prop = "")
//         {
//             if (PropertyChanged != null)
//                 PropertyChanged(this, new PropertyChangedEventArgs(prop));
//         }
//     }
//
// }
