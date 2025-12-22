using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using MobileOperator.models;
using MobileOperator.views;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace MobileOperator.viewmodels
{
    class DetailingWindow2ViewModel : INotifyPropertyChanged
    {
        private int userId, status;
        private ClientModel client;
        private Detailing2Model detailing;
        private DateTime from = new DateTime(2025, 01, 01);
        private DateTime till = DateTime.Now;
        private readonly Infrastructure.MobileOperator _context;
        
        public ObservableCollection<RateHistoryModel> Rates { get; set; }
        public ObservableCollection<ServiceHistoryModel> Services { get; set; }

        public DetailingWindow2ViewModel(int userId, int status, Infrastructure.MobileOperator context)
        {
            this.userId = userId;
            this.status = status;
            _context = context;

            if (status == 2)
                client = new ULModel(userId, _context);
            else
                client = new FLModel(userId, _context);

            Rates = new ObservableCollection<RateHistoryModel> { };
            Services = new ObservableCollection<ServiceHistoryModel> { };
        }

        public DateTime From
        {
            get { return from; }
            set { from = value; OnPropertyChanged("From"); }
        }
        public DateTime Till
        {
            get { return till; }
            set { till = value; OnPropertyChanged("Till"); }
        }

        private RelayCommand getDetailingCommand;
        public RelayCommand GetDetailingCommand
        {
            get
            {
                return getDetailingCommand ??
                       (getDetailingCommand = new RelayCommand(obj =>
                       {
                           Rates?.Clear();
                           Services?.Clear();
                           
                           _context.ChangeTracker.Clear();

                           var fromLocal = from.Date;
                           var tillLocal = till.Date.AddDays(1).AddTicks(-1);

                           var fromUtc = DateTime.SpecifyKind(fromLocal, DateTimeKind.Local).ToUniversalTime();
                           var tillUtc = DateTime.SpecifyKind(tillLocal, DateTimeKind.Local).ToUniversalTime();
                           
                           var clientDb = _context.Client.FirstOrDefault(c => c.UserId == userId);
                           if (clientDb == null) return;
                           
                           detailing = new Detailing2Model(clientDb.UserId, fromUtc, tillUtc, _context);

                           foreach (var rate in detailing.AllRates)
                               Rates.Add(rate);
                           
                           foreach (var service in detailing.AllServices)
                               Services.Add(service);
                       }));
            }
        }
        
        private RelayCommand printToExelDetailingCommand;
        public RelayCommand PrintToExelDetailingCommand
        {
            get
            {
                return printToExelDetailingCommand ??
                  (printToExelDetailingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          Excel.Application ExcelApp = new Excel.Application();
                          ExcelApp.Application.Workbooks.Add(Type.Missing);
                          Excel.Worksheet workSheet = (Excel.Worksheet)ExcelApp.ActiveSheet;
                          workSheet.Columns.ColumnWidth = 25;

                          workSheet.Cells[1, 1] = "История тарифов и услуг";
                          workSheet.Cells[2, 1] = $"Клиент: {client.Number}"; 
                          workSheet.Cells[3, 1] = $"Период: {from:dd.MM.yyyy} - {till:dd.MM.yyyy}";

                          int startRow = 5;
                          workSheet.Cells[startRow, 1] = "Наименование тарифа";
                          workSheet.Cells[startRow, 2] = "Дата подключения тарифа";
                          workSheet.Cells[startRow, 3] = "Дата отключения тарифа";
                          workSheet.Cells[startRow, 4] = " ";
                          workSheet.Cells[startRow, 5] = "Наименование услуги";
                          workSheet.Cells[startRow, 6] = "Дата подключения услуги";
                          workSheet.Cells[startRow, 7] = "Дата отключения услуги";

                          for (int i = 0; i < Rates.Count; i++)
                          {
                              workSheet.Cells[i + startRow + 1, 1] = (Rates[i].RateName).ToString();
                              workSheet.Cells[i + startRow + 1, 2] = (Rates[i].FromDateToString).ToString();
                              workSheet.Cells[i + startRow + 1, 3] = (Rates[i].TillDateToString).ToString();
                          }
                          for (int i = 0; i < Services.Count; i++)
                          {
                              workSheet.Cells[i + startRow + 1, 5] = (Services[i].ServiceName).ToString();
                              workSheet.Cells[i + startRow + 1, 6] = (Services[i].FromDateToString).ToString();
                              workSheet.Cells[i + startRow + 1, 7] = (Services[i].TillDateToString).ToString();
                          }

                          int maxCount = (Rates.Count > Services.Count) ? Rates.Count : Services.Count;
                          int footerRow = startRow + maxCount + 2;

                          workSheet.Cells[footerRow, 1] = $"Смен тарифа: {Rates.Count}";
                          workSheet.Cells[footerRow, 5] = $"Подключений услуг: {Services.Count}";

                          SaveFileDialog svg = new SaveFileDialog();
                          if (svg.ShowDialog() == true)
                          {
                              string pathToXmlFile = svg.FileName + ".xlsx";
                              workSheet.SaveAs(pathToXmlFile);
                              MessageBox.Show("Файл сохранен");
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private RelayCommand printToWordDetailingCommand;
        public RelayCommand PrintToWordDetailingCommand
        {
            get
            {
                return printToWordDetailingCommand ??
                  (printToWordDetailingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          Word.Application application = new Word.Application();
                          Object missing = Type.Missing;
                          application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                          Word.Document document = application.ActiveDocument;

                          Word.Paragraph headerPara = document.Content.Paragraphs.Add(ref missing);
                          headerPara.Range.Text = $"История операций\nКлиент: {client.Number}\nПериод: {from:dd.MM.yyyy} - {till:dd.MM.yyyy}\n";
                          headerPara.Range.InsertParagraphAfter();

                          Word.Range range = document.Range();
                          range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

                          Object behiavor = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                          Object autoFitBehiavor = Word.WdAutoFitBehavior.wdAutoFitFixed;
                          int count;
                          if (Services.Count > Rates.Count)
                              count = Services.Count;
                          else count = Rates.Count;

                          document.Tables.Add(range, count + 1, 7, ref behiavor, ref autoFitBehiavor);
                          Word.Table table = document.Tables[1];

                          table.Cell(1, 1).Range.Text = "Тариф";
                          table.Cell(1, 2).Range.Text = "Подключен";
                          table.Cell(1, 3).Range.Text = "Отключен";
                          table.Cell(1, 4).Range.Text = " ";
                          table.Cell(1, 5).Range.Text = "Услуга";
                          table.Cell(1, 6).Range.Text = "Подключена";
                          table.Cell(1, 7).Range.Text = "Отключена";

                          for (int i = 0; i < Rates.Count; i++)
                          {
                              table.Cell(i + 2, 1).Range.Text = (Rates[i].RateName).ToString();
                              table.Cell(i + 2, 2).Range.Text = (Rates[i].FromDateToString).ToString();
                              table.Cell(i + 2, 3).Range.Text = (Rates[i].TillDateToString).ToString();
                          }
                          for (int i = 0; i < Services.Count; i++)
                          {
                              table.Cell(i + 2, 5).Range.Text = (Services[i].ServiceName).ToString();
                              table.Cell(i + 2, 6).Range.Text = (Services[i].FromDateToString).ToString();
                              table.Cell(i + 2, 7).Range.Text = (Services[i].TillDateToString).ToString();
                          }

                          Word.Range footerRange = document.Range();
                          footerRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                          footerRange.InsertParagraphAfter();
                          Word.Paragraph footerPara = document.Content.Paragraphs.Add(footerRange);
                          footerPara.Range.Text = $"\nВсего записей по тарифам: {Rates.Count}\nВсего записей по услугам: {Services.Count}";

                          SaveFileDialog svg = new SaveFileDialog();
                          if (svg.ShowDialog() == true)
                          {
                              string pathToDocFile = svg.FileName + ".docx";
                              document.SaveAs(pathToDocFile);
                              MessageBox.Show("Файл сохранен");
                          }
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}