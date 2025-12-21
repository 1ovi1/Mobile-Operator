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
//     class AskRateWindowViewModel : INotifyPropertyChanged
//     {
//         private RateListModel allRates;
//         private RateModel rate;
//         private int newrate;
//         private Window window, parentWindow;
//         public ObservableCollection<RateModel> Rates { get; set; }
//         public AskRateWindowViewModel(int rateId, Window window, Window parentWindow)
//         {
//             this.window = window;
//             this.parentWindow = parentWindow;
//             allRates = new RateListModel();
//             rate = new RateModel(rateId);
//             Rates = new ObservableCollection<RateModel> { };
//             if (rate.Corporate)
//                 foreach (RateModel rate in allRates.AllCorporateRates)
//                     Rates.Add(new RateModel()
//                     {
//                         Name = rate.Name,
//                         Id = rate.Id
//                     });
//             else
//                 foreach (RateModel rate in allRates.AllNotCorporateRates)
//                     Rates.Add(new RateModel()
//                     {
//                         Name = rate.Name,
//                         Id = rate.Id
//                     });
//         }
//         public int Rate
//         {
//             get { return newrate; }
//             set
//             {
//                 if (newrate == value)
//                     MessageBox.Show("Выберите новый тариф!");
//                 else
//                     newrate = value;
//                 OnPropertyChanged("Rates");
//             }
//         }
//
//         private RelayCommand updateRateCommand;
//         public RelayCommand UpdateRateCommand
//         {
//             get
//             {
//                 return updateRateCommand ??
//                   (updateRateCommand = new RelayCommand(obj =>
//                   {
//                       if (rate.Id == newrate)
//                           MessageBox.Show("Выберите новый тариф!");
//                       else
//                       {
//                           int count = 0;
//                           ClientListModel clients = new ClientListModel(rate.Id);
//                           foreach (ClientModel client in clients.AllClients)
//                           {
//                               client.Rate = newrate;
//                               client.Save();
//                               if (client.AddRateHistory())
//                                   count++;
//                           }
//                           if (count == clients.AllClients.Count())
//                           {
//                               MessageBox.Show("Переподключение всех абонентов прошло успешно!");
//                               window.Close();
//                               if (rate.Remove())
//                               {
//                                   AdminRatesWindow rates = new AdminRatesWindow();
//                                   rates.WindowState = parentWindow.WindowState;
//                                   rates.Top = parentWindow.Top;
//                                   rates.Left = parentWindow.Left;
//                                   rates.Height = parentWindow.Height;
//                                   rates.Width = parentWindow.Width;
//                                   rates.Show();
//                                   window.Close();
//                                   parentWindow.Close();
//                                   MessageBox.Show("Удаление прошло успешно!");
//                               }
//                               else
//                                   MessageBox.Show("Произошла ошибка удаления, попробуйте еще раз!");
//                           }
//                           else
//                               MessageBox.Show("Произошла ошибка переподключения, попроюуйте еще раз!");
//                       }
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
