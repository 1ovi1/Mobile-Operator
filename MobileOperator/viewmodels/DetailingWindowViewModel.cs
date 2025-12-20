using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using MobileOperator.models;
using MobileOperator.Models;
using MobileOperator.views;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace MobileOperator.viewmodels
{
    class DetailingWindowViewModel : INotifyPropertyChanged
    {
        private int userId, status;
        private ClientModel client;
        private DetailingModel detailing;
        private DateTime from = new DateTime(2025, 01, 01);
        private DateTime till = DateTime.Now;
        private readonly Infrastructure.MobileOperator _context = new Infrastructure.MobileOperator(App.DbOptions);
        public ObservableCollection<CallModel> Calls { get; set; }

        public DetailingWindowViewModel(int userId, int status)
        {
            this.userId = userId;
            this.status = status;

            if (status == 2)
                client = new ULModel(userId, _context);
            else
                client = new FLModel(userId, _context);

            Calls = new ObservableCollection<CallModel> { };
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
                      if (Calls != null) Calls.Clear();
                      
                      _context.ChangeTracker.Clear();

                      var fromUtc = from.ToUniversalTime();
                      var tillUtc = till.ToUniversalTime();

                      var clientDb = _context.Client.FirstOrDefault(c => c.UserId == userId);
                      if (clientDb == null) return;
                      
                      detailing = new DetailingModel(clientDb.UserId, client.Number, fromUtc, tillUtc, _context);

                      foreach (var c in detailing.AllCalls)
                      {
                          Calls.Add(c);
                      }
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
                          workSheet.Columns.ColumnWidth = 20;

                          workSheet.Cells[1, 1] = "Детализация звонков";
                          workSheet.Cells[2, 1] = $"Клиент: {client.Number}"; 
                          workSheet.Cells[3, 1] = $"Период: {from:dd.MM.yyyy} - {till:dd.MM.yyyy}";

                          int startRow = 5;
                          workSheet.Cells[startRow, 1] = "Собеседник";
                          workSheet.Cells[startRow, 2] = "Тип соединения";
                          workSheet.Cells[startRow, 3] = "Направление";
                          workSheet.Cells[startRow, 4] = "Списание (руб)";
                          workSheet.Cells[startRow, 5] = "Время";
                          workSheet.Cells[startRow, 6] = "Продолжительность";

                          decimal totalCost = 0;

                          for (int i = 0; i < Calls.Count; i++)
                          {
                              workSheet.Cells[i + startRow + 1, 1] = (Calls[i].Number).ToString();
                              workSheet.Cells[i + startRow + 1, 2] = (Calls[i].TypeName).ToString();
                              workSheet.Cells[i + startRow + 1, 3] = (Calls[i].Direction).ToString();
                              workSheet.Cells[i + startRow + 1, 4] = (Calls[i].Cost).ToString();
                              workSheet.Cells[i + startRow + 1, 5] = (Calls[i].Time).ToString();
                              workSheet.Cells[i + startRow + 1, 6] = (Calls[i].Duration).ToString();

                              if (decimal.TryParse(Calls[i].Cost.ToString(), out decimal cost))
                                  totalCost += cost;
                          }

                          int footerRow = startRow + Calls.Count + 2;
                          workSheet.Cells[footerRow, 1] = "ИТОГО:";
                          workSheet.Cells[footerRow, 4] = $"{totalCost} руб.";
                          workSheet.Cells[footerRow, 6] = $"Всего звонков: {Calls.Count}";

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
                          headerPara.Range.Text = $"Детализация звонков\nКлиент: {client.Number}\nПериод: {from:dd.MM.yyyy} - {till:dd.MM.yyyy}\n";
                          headerPara.Range.InsertParagraphAfter();

                          Word.Range range = document.Range(); 
                          range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

                          Object behiavor = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                          Object autoFitBehiavor = Word.WdAutoFitBehavior.wdAutoFitFixed;
                          
                          document.Tables.Add(range, Calls.Count + 1, 6, ref behiavor, ref autoFitBehiavor);
                          Word.Table table = document.Tables[1];

                          table.Cell(1, 1).Range.Text = "Собеседник";
                          table.Cell(1, 2).Range.Text = "Тип";
                          table.Cell(1, 3).Range.Text = "Напр.";
                          table.Cell(1, 4).Range.Text = "Сумма";
                          table.Cell(1, 5).Range.Text = "Время";
                          table.Cell(1, 6).Range.Text = "Длит.";

                          decimal totalCost = 0;

                          for (int i = 0; i < Calls.Count; i++)
                          {
                              table.Cell(i + 2, 1).Range.Text = (Calls[i].Number).ToString();
                              table.Cell(i + 2, 2).Range.Text = (Calls[i].TypeName).ToString();
                              table.Cell(i + 2, 3).Range.Text = (Calls[i].Direction).ToString();
                              table.Cell(i + 2, 4).Range.Text = (Calls[i].Cost).ToString();
                              table.Cell(i + 2, 5).Range.Text = (Calls[i].Time).ToString();
                              table.Cell(i + 2, 6).Range.Text = (Calls[i].Duration).ToString();

                              if (decimal.TryParse(Calls[i].Cost.ToString(), out decimal cost))
                                  totalCost += cost;
                          }

                          Word.Range footerRange = document.Range();
                          footerRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                          footerRange.InsertParagraphAfter();
                          Word.Paragraph footerPara = document.Content.Paragraphs.Add(footerRange);
                          footerPara.Range.Text = $"\nИтого списаний: {totalCost} руб.\nКоличество звонков: {Calls.Count}";

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
