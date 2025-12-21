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
//     class AdminSelectesRateWindowViewModel : INotifyPropertyChanged
//     {
//         private int rateId;
//         private RateModel rate;
//         private bool createMod = false;
//         private Window window;
//         public AdminSelectesRateWindowViewModel(int rateId, Window window)
//         {
//             this.rateId = rateId;
//             this.window = window;
//             rate = new RateModel(rateId);
//             createMod = false;
//         }
//         public AdminSelectesRateWindowViewModel(Window window)
//         {
//             this.window = window;
//             rate = new RateModel()
//             { Name = "Новый тариф" };
//             createMod = true;
//         }
//         public int Id
//         {
//             get { return rate.Id; }
//             set { rate.Id = value; }
//         }
//         public string Rate
//         {
//             get { if (rate.Name != null) return rate.Name; else return ""; }
//             set
//             {
//                 rate.Name = value;
//                 OnPropertyChanged("RateName");
//             }
//         }
//         public bool Corporate
//         {
//             get { if (rate.Corporate != null) return (bool)rate.Corporate; else return false; }
//             set { rate.Corporate = value; }
//         }
//         public decimal ConnectionCost
//         {
//             get { if (rate.ConnectionCost != null) return (decimal)rate.ConnectionCost; else return 0; }
//             set
//             {
//                 rate.ConnectionCost = value;
//                 OnPropertyChanged("RateConnectionCost");
//             }
//         }
//         public decimal Cost
//         {
//             get { if (rate.Cost != null) return (decimal)rate.Cost; else return 0; }
//             set
//             {
//                 rate.Cost = value;
//                 OnPropertyChanged("RateCost");
//             }
//         }
//         public int Minutes
//         {
//             get { if (rate.Minutes != null) return (int)rate.Minutes; else return 0; }
//             set
//             {
//                 rate.Minutes = value;
//                 OnPropertyChanged("RateMinutes");
//             }
//         }
//         public int GB
//         {
//             get { if (rate.GB != null) return (int)rate.GB; else return 0; }
//             set
//             {
//                 rate.GB = value;
//                 OnPropertyChanged("RateGB");
//             }
//         }
//         public int SMS
//         {
//             get { if (rate.SMS != null) return (int)rate.SMS; else return 0; }
//             set
//             {
//                 rate.SMS = value;
//                 OnPropertyChanged("RateSMS");
//             }
//         }
//         public decimal CityCost
//         {
//             get { if (rate.CityCost != null) return (decimal)rate.CityCost; else return 0; }
//             set
//             {
//                 rate.CityCost = value;
//                 OnPropertyChanged("RateCityCost");
//             }
//         }
//         public decimal IntercityCost
//         {
//             get { if (rate.IntercityCost != null) return (decimal)rate.IntercityCost; else return 0; }
//             set
//             {
//                 rate.IntercityCost = value;
//                 OnPropertyChanged("RateIntercityCost");
//             }
//         }
//         public decimal InternationalCost
//         {
//             get { if (rate.InternationalCost != null) return (decimal)rate.InternationalCost; else return 0; }
//             set
//             {
//                 rate.InternationalCost = value;
//                 OnPropertyChanged("RateInternationalCoat");
//             }
//         }
//
//         public decimal SMSCost
//         {
//             get { if (rate.SMSCost != null) return (decimal)rate.SMSCost; else return 0; }
//             set
//             {
//                 rate.SMSCost = value;
//                 OnPropertyChanged("RateSMSCost");
//             }
//         }
//         public decimal GBCost
//         {
//             get { if (rate.GBCost != null) return (decimal)rate.GBCost; else return 0; }
//             set
//             {
//                 rate.GBCost = value;
//                 OnPropertyChanged("RateGBCost");
//             }
//         }
//
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
//         private RelayCommand updateRateCommand;
//         public RelayCommand UpdateRateCommand
//         {
//             get
//             {
//                 return updateRateCommand ??
//                   (updateRateCommand = new RelayCommand(obj =>
//                   {
//                       if (rate.Save())
//                       {
//                           AdminRatesWindow rates = new AdminRatesWindow();
//                           rates.WindowState = window.WindowState;
//                           rates.Top = window.Top;
//                           rates.Left = window.Left;
//                           rates.Height = window.Height;
//                           rates.Width = window.Width;
//                           rates.Show();
//                           window.Close();
//                           MessageBox.Show("Сохренение прошло успешно!");
//                       }
//                       else
//                           MessageBox.Show("Произошла ошибка, попробуйте еще раз!");
//                   }));
//             }
//         }
//         private RelayCommand removeRateCommand;
//         public RelayCommand RemoveRateCommand
//         {
//             get
//             {
//                 return removeRateCommand ??
//                   (removeRateCommand = new RelayCommand(obj =>
//                   {
//                       AskRateWindow ask = new AskRateWindow(rate.Id, window);
//                       ask.Show();
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
