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
    public class WriteOffsViewModel : INotifyPropertyChanged
    {
        private int userId;
        private int status;
        private ClientModel client;
        
        private DateTime from = new DateTime(2025, 01, 01);
        private DateTime till = DateTime.Now;
        
        private readonly Infrastructure.MobileOperator _context;

        public ObservableCollection<WriteOffModel> WriteOffs { get; set; }
        
        public WriteOffsViewModel(int userId, int status, Infrastructure.MobileOperator context)
        {
            this.userId = userId;
            this.status = status;
            _context = context;
            
            if (status == 2)
                client = new ULModel(userId, _context);
            else
                client = new FLModel(userId, _context);

            WriteOffs = new ObservableCollection<WriteOffModel>();
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

        private RelayCommand getWriteOffsCommand;
        public RelayCommand GetWriteOffsCommand
        {
            get
            {
                return getWriteOffsCommand ??
                       (getWriteOffsCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               WriteOffs.Clear();
                               _context.ChangeTracker.Clear();

                               var clientDb = _context.Client.FirstOrDefault(c => c.UserId == userId);
                               if (clientDb == null) return;

                               var fromUtc = DateTime.SpecifyKind(from.Date, DateTimeKind.Local).ToUniversalTime();
                               var tillUtc = DateTime.SpecifyKind(till.Date.AddDays(1), DateTimeKind.Local).ToUniversalTime();

                               var data = _context.WriteOff
                                   .Where(w => w.ClientId == clientDb.UserId &&
                                               w.WriteOffDate >= fromUtc &&
                                               w.WriteOffDate < tillUtc)
                                   .OrderByDescending(w => w.WriteOffDate)
                                   .ToList();

                               foreach (var item in data)
                               {
                                   WriteOffs.Add(new WriteOffModel(item));
                               }
                           }
                           catch (Exception ex)
                           {
                               MessageBox.Show("Ошибка загрузки списаний: " + ex.Message);
                           }
                       }));
            }
        }

        private RelayCommand printToExelCommand;
        public RelayCommand PrintToExelCommand
        {
            get
            {
                return printToExelCommand ??
                       (printToExelCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               Excel.Application ExcelApp = new Excel.Application();
                               ExcelApp.Application.Workbooks.Add(Type.Missing);
                               Excel.Worksheet workSheet = (Excel.Worksheet)ExcelApp.ActiveSheet;
                               workSheet.Columns.ColumnWidth = 25;

                               workSheet.Cells[1, 1] = "История списаний";
                               workSheet.Cells[2, 1] = $"Клиент: {client.Number}"; 
                               workSheet.Cells[3, 1] = $"Период: {from:dd.MM.yyyy} - {till:dd.MM.yyyy}";

                               int startRow = 5;
                               workSheet.Cells[startRow, 1] = "Дата";
                               workSheet.Cells[startRow, 2] = "Категория";
                               workSheet.Cells[startRow, 3] = "Сумма";
                               workSheet.Cells[startRow, 4] = "Описание";

                               for (int i = 0; i < WriteOffs.Count; i++)
                               {
                                   workSheet.Cells[i + startRow + 1, 1] = WriteOffs[i].DateToString;
                                   workSheet.Cells[i + startRow + 1, 2] = WriteOffs[i].Category;
                                   workSheet.Cells[i + startRow + 1, 3] = WriteOffs[i].Amount;
                                   workSheet.Cells[i + startRow + 1, 4] = WriteOffs[i].Description;
                               }

                               int footerRow = startRow + WriteOffs.Count + 2;
                               workSheet.Cells[footerRow, 1] = $"Всего записей: {WriteOffs.Count}";

                               decimal totalSum = WriteOffs.Sum(x => x.Amount);
                               workSheet.Cells[footerRow, 3] = $"Итого: {totalSum}";

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

        private RelayCommand printToWordCommand;
        public RelayCommand PrintToWordCommand
        {
            get
            {
                return printToWordCommand ??
                       (printToWordCommand = new RelayCommand(obj =>
                       {
                           try
                           {
                               Word.Application application = new Word.Application();
                               Object missing = Type.Missing;
                               application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                               Word.Document document = application.ActiveDocument;

                               Word.Paragraph headerPara = document.Content.Paragraphs.Add(ref missing);
                               headerPara.Range.Text = $"История списаний\nКлиент: {client.Number}\nПериод: {from:dd.MM.yyyy} - {till:dd.MM.yyyy}\n";
                               headerPara.Range.InsertParagraphAfter();

                               Word.Range range = document.Range();
                               range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

                               Object behiavor = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                               Object autoFitBehiavor = Word.WdAutoFitBehavior.wdAutoFitFixed;

                               document.Tables.Add(range, WriteOffs.Count + 1, 4, ref behiavor, ref autoFitBehiavor);
                               Word.Table table = document.Tables[1];

                               table.Cell(1, 1).Range.Text = "Дата";
                               table.Cell(1, 2).Range.Text = "Категория";
                               table.Cell(1, 3).Range.Text = "Сумма";
                               table.Cell(1, 4).Range.Text = "Описание";

                               for (int i = 0; i < WriteOffs.Count; i++)
                               {
                                   table.Cell(i + 2, 1).Range.Text = WriteOffs[i].DateToString;
                                   table.Cell(i + 2, 2).Range.Text = WriteOffs[i].Category;
                                   table.Cell(i + 2, 3).Range.Text = WriteOffs[i].Amount.ToString();
                                   table.Cell(i + 2, 4).Range.Text = WriteOffs[i].Description;
                               }

                               Word.Range footerRange = document.Range();
                               footerRange.Collapse(Word.WdCollapseDirection.wdCollapseEnd);
                               footerRange.InsertParagraphAfter();
                               Word.Paragraph footerPara = document.Content.Paragraphs.Add(footerRange);

                               decimal totalSum = WriteOffs.Sum(x => x.Amount);
                               footerPara.Range.Text = $"\nВсего записей: {WriteOffs.Count}\nОбщая сумма списаний: {totalSum}";

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
