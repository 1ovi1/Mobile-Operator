using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using MobileOperator.Domain.Entities;
using MobileOperator.Infrastructure;
using MobileOperator.models;
using MobileOperator.views;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;

namespace MobileOperator.viewmodels
{
    public class AdminDetailing2ViewModel : INotifyPropertyChanged
    {
        private readonly Infrastructure.MobileOperator _context;
        
        public ObservableCollection<RateHistoryModel> RateHistory { get; set; }
        public ObservableCollection<ServiceHistoryModel> ServiceHistory { get; set; }

        private string _searchName;
        private string _searchNumber;
        private string _foundClientNumber = "";

        public AdminDetailing2ViewModel(Infrastructure.MobileOperator context)
        {
            _context = context;
            RateHistory = new ObservableCollection<RateHistoryModel>();
            ServiceHistory = new ObservableCollection<ServiceHistoryModel>();
        }

        public string SearchName
        {
            get => _searchName;
            set { _searchName = value; OnPropertyChanged(); }
        }
        public string SearchNumber
        {
            get => _searchNumber;
            set { _searchNumber = value; OnPropertyChanged(); }
        }

        private RelayCommand _getHistoryCommand;
        public RelayCommand GetHistoryCommand
        {
            get
            {
                return _getHistoryCommand ?? (_getHistoryCommand = new RelayCommand(obj =>
                {
                    RateHistory.Clear();
                    ServiceHistory.Clear();
                    _context.ChangeTracker.Clear();

                    Client clientDb = null;

                    if (!string.IsNullOrWhiteSpace(SearchNumber))
                    {
                        clientDb = _context.Client.FirstOrDefault(c => c.Number == SearchNumber);
                    }
                    else if (!string.IsNullOrWhiteSpace(SearchName))
                    {
                        var fl = _context.FL.FirstOrDefault(f => f.FIO.Contains(SearchName));
                        if (fl != null) clientDb = _context.Client.FirstOrDefault(c => c.UserId == fl.UserId);
                        
                        if (clientDb == null)
                        {
                            var ul = _context.UL.FirstOrDefault(u => u.OrganizationName.Contains(SearchName));
                            if (ul != null) clientDb = _context.Client.FirstOrDefault(c => c.UserId == ul.UserId);
                        }
                    }

                    if (clientDb == null)
                    {
                        MessageBox.Show("Клиент не найден");
                        return;
                    }

                    _foundClientNumber = clientDb.Number;

                    var fromUtc = new DateTime(2000, 1, 1).ToUniversalTime();
                    var tillUtc = DateTime.Now.AddDays(1).ToUniversalTime();

                    var detailing = new Detailing2Model(clientDb.UserId, fromUtc, tillUtc, _context);

                    foreach (var rate in detailing.AllRates)
                        RateHistory.Add(rate);

                    foreach (var service in detailing.AllServices)
                        ServiceHistory.Add(service);
                }));
            }
        }

        private RelayCommand _printToExelDetailingCommand;
        public RelayCommand PrintToExelDetailingCommand
        {
            get
            {
                return _printToExelDetailingCommand ?? (_printToExelDetailingCommand = new RelayCommand(obj =>
                {
                    if (RateHistory.Count == 0 && ServiceHistory.Count == 0) return;
                    try
                    {
                        Excel.Application ExcelApp = new Excel.Application();
                        ExcelApp.Application.Workbooks.Add(Type.Missing);
                        Excel.Worksheet workSheet = (Excel.Worksheet)ExcelApp.ActiveSheet;
                        workSheet.Columns.ColumnWidth = 25;

                        workSheet.Cells[1, 1] = "История тарифов и услуг (Админ)";
                        workSheet.Cells[2, 1] = $"Клиент: {_foundClientNumber}";
                        workSheet.Cells[3, 1] = $"Дата формирования: {DateTime.Now:dd.MM.yyyy}";

                        int startRow = 5;
                        workSheet.Cells[startRow, 1] = "Наименование тарифа";
                        workSheet.Cells[startRow, 2] = "Дата подключения тарифа";
                        workSheet.Cells[startRow, 3] = "Дата отключения тарифа";
                        workSheet.Cells[startRow, 4] = " ";
                        workSheet.Cells[startRow, 5] = "Наименование услуги";
                        workSheet.Cells[startRow, 6] = "Дата подключения услуги";
                        workSheet.Cells[startRow, 7] = "Дата отключения услуги";

                        for (int i = 0; i < RateHistory.Count; i++)
                        {
                            workSheet.Cells[i + startRow + 1, 1] = (RateHistory[i].RateName).ToString();
                            workSheet.Cells[i + startRow + 1, 2] = (RateHistory[i].FromDateToString).ToString();
                            workSheet.Cells[i + startRow + 1, 3] = (RateHistory[i].TillDateToString).ToString();
                        }
                        for (int i = 0; i < ServiceHistory.Count; i++)
                        {
                            workSheet.Cells[i + startRow + 1, 5] = (ServiceHistory[i].ServiceName).ToString();
                            workSheet.Cells[i + startRow + 1, 6] = (ServiceHistory[i].FromDateToString).ToString();
                            workSheet.Cells[i + startRow + 1, 7] = (ServiceHistory[i].TillDateToString).ToString();
                        }

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

        private RelayCommand _printToWordDetailingCommand;
        public RelayCommand PrintToWordDetailingCommand
        {
            get
            {
                return _printToWordDetailingCommand ?? (_printToWordDetailingCommand = new RelayCommand(obj =>
                {
                    if (RateHistory.Count == 0 && ServiceHistory.Count == 0) return;
                    try
                    {
                        Word.Application application = new Word.Application();
                        Object missing = Type.Missing;
                        application.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                        Word.Document document = application.ActiveDocument;

                        Word.Paragraph headerPara = document.Content.Paragraphs.Add(ref missing);
                        headerPara.Range.Text = $"История операций (Админ)\nКлиент: {_foundClientNumber}\nДата: {DateTime.Now:dd.MM.yyyy}\n";
                        headerPara.Range.InsertParagraphAfter();

                        Word.Range range = document.Range();
                        range.Collapse(Word.WdCollapseDirection.wdCollapseEnd);

                        Object behiavor = Word.WdDefaultTableBehavior.wdWord9TableBehavior;
                        Object autoFitBehiavor = Word.WdAutoFitBehavior.wdAutoFitFixed;
                        int count = Math.Max(ServiceHistory.Count, RateHistory.Count);

                        document.Tables.Add(range, count + 1, 7, ref behiavor, ref autoFitBehiavor);
                        Word.Table table = document.Tables[1];

                        table.Cell(1, 1).Range.Text = "Тариф";
                        table.Cell(1, 2).Range.Text = "Подключен";
                        table.Cell(1, 3).Range.Text = "Отключен";
                        table.Cell(1, 4).Range.Text = " ";
                        table.Cell(1, 5).Range.Text = "Услуга";
                        table.Cell(1, 6).Range.Text = "Подключена";
                        table.Cell(1, 7).Range.Text = "Отключена";

                        for (int i = 0; i < RateHistory.Count; i++)
                        {
                            table.Cell(i + 2, 1).Range.Text = (RateHistory[i].RateName).ToString();
                            table.Cell(i + 2, 2).Range.Text = (RateHistory[i].FromDateToString).ToString();
                            table.Cell(i + 2, 3).Range.Text = (RateHistory[i].TillDateToString).ToString();
                        }
                        for (int i = 0; i < ServiceHistory.Count; i++)
                        {
                            table.Cell(i + 2, 5).Range.Text = (ServiceHistory[i].ServiceName).ToString();
                            table.Cell(i + 2, 6).Range.Text = (ServiceHistory[i].FromDateToString).ToString();
                            table.Cell(i + 2, 7).Range.Text = (ServiceHistory[i].TillDateToString).ToString();
                        }

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
