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
//     class AdminSelectesServiceWindowViewModel : INotifyPropertyChanged
//     {
//         private int serviceId;
//         private ServiceModel service;
//         private bool createMod;
//         private Window window;
//         public AdminSelectesServiceWindowViewModel(int serviceId, Window window)
//         {
//             this.serviceId = serviceId;
//             this.window = window;
//             service = new ServiceModel(serviceId);
//             createMod = false;
//         }
//         public AdminSelectesServiceWindowViewModel(Window window)
//         {
//             this.window = window;
//             service = new ServiceModel()
//             { Name = "Новая услуга" };
//             createMod = true;
//         }
//         public int Id
//         {
//             get { return service.Id; }
//             set { service.Id = value; }
//         }
//         public string Name
//         {
//             get { if (service.Name != null) return service.Name; else return ""; }
//             set
//             {
//                 service.Name = value;
//                 OnPropertyChanged("ServiceName");
//             }
//         }
//         public decimal ConnectionCost
//         {
//             get { if (service.ConnectionCost != null) return (decimal)service.ConnectionCost; else return 0; }
//             set
//             {
//                 service.ConnectionCost = value;
//                 OnPropertyChanged("ServiceConnectionCost");
//             }
//         }
//         public decimal Cost
//         {
//             get { if (service.Cost != null) return (decimal)service.Cost; else return 0; }
//             set
//             {
//                 service.Cost = value;
//                 OnPropertyChanged("ServiceCost");
//             }
//         }
//         public string Condition
//         {
//             get { if (service.Conditions != null) return service.Conditions; else return ""; }
//             set
//             {
//                 service.Conditions = value;
//                 OnPropertyChanged("ServiceCondition");
//             }
//         }
//         public bool Create
//         {
//             get { return createMod; }
//             set { }
//         }
//
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
//         private RelayCommand updateServiceCommand;
//         public RelayCommand UpdateServiceCommand
//         {
//             get
//             {
//                 return updateServiceCommand ??
//                   (updateServiceCommand = new RelayCommand(obj =>
//                   {
//                       if (service.Save())
//                       {
//                           AdminServicesWindow services = new AdminServicesWindow();
//                           services.WindowState = window.WindowState;
//                           services.Top = window.Top;
//                           services.Left = window.Left;
//                           services.Height = window.Height;
//                           services.Width = window.Width;
//                           services.Show();
//                           window.Close();
//                           MessageBox.Show("Сохренение прошло успешно!");
//                       }
//                       else
//                           MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
//                   }));
//             }
//         }
//         private RelayCommand removeServiceCommand;
//         public RelayCommand RemoveServiceCommand
//         {
//             get
//             {
//                 return removeServiceCommand ??
//                   (removeServiceCommand = new RelayCommand(obj =>
//                   {
//                       if (service.Remove())
//                       {
//                           AdminServicesWindow services = new AdminServicesWindow();
//                           services.WindowState = window.WindowState;
//                           services.Top = window.Top;
//                           services.Left = window.Left;
//                           services.Height = window.Height;
//                           services.Width = window.Width;
//                           services.Show();
//                           window.Close();
//                           MessageBox.Show("Удаление прошло успешно!");
//                       }
//                       else
//                           MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
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
