
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YourDoctor.WiForms.Administrator.objDoctor;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;


namespace YourDoctor.WiForms.Patient
{
    /// <summary>
    /// Логика взаимодействия для pagePatient.xaml
    /// </summary>
    public partial class pagePatient : System.Windows.Controls.Page
    {
       
        public pagePatient()
        {
            InitializeComponent();
            
        }
        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "select * from patient order by id asc;";
            Connection.Table_Fill("Пациент", sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Пациент"].DefaultView;

            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
        }




        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new addPatient());
        }

        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

           
            if (nurseDataGrid.SelectedItem != null) // Если выбрана строка
            {
                this.NavigationService.Navigate(new customerPatient(nurseDataGrid.SelectedIndex));
            }

        }

        private void btn_printLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем новый документ Excel
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create("Пациенты.xlsx", SpreadsheetDocumentType.Workbook);

                // Добавляем рабочую книгу
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Добавляем лист
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Добавляем лист в книгу
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Пациенты" };
                sheets.Append(sheet);

                // Получаем таблицу "Пациент" из объекта DataSet
                DataTable dataTable = Connection.ds.Tables["Пациент"];

                // Заполняем заголовок
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Добавляем строку заголовка
                Row titleRow = new Row();
                Cell titleCell = CreateTextCell("Список пациентов медицинского центра ООО Ваш Доктор");
                titleRow.AppendChild(titleCell);
                sheetData.AppendChild(titleRow);

                // Определяем диапазон объединения ячеек заголовка
                string firstColumn = "A";
                string lastColumn = Convert.ToChar('A' + dataTable.Columns.Count - 1).ToString();
                string headerRange = $"{firstColumn}1:{lastColumn}1";

                // Создаем объединение ячеек
                MergeCells mergeCells;
                if (worksheetPart.Worksheet.Elements<MergeCells>().Any())
                {
                    mergeCells = worksheetPart.Worksheet.Elements<MergeCells>().First();
                }
                else
                {
                    mergeCells = new MergeCells();
                    if (worksheetPart.Worksheet.Elements<CustomSheetView>().Any())
                    {
                        worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<CustomSheetView>().First());
                    }
                    else
                    {
                        worksheetPart.Worksheet.InsertAfter(mergeCells, worksheetPart.Worksheet.Elements<SheetData>().First());
                    }
                }

                // Добавляем объединение ячеек в документ
                MergeCell mergeCell = new MergeCell() { Reference = headerRange };
                mergeCells.Append(mergeCell);
                // Заполняем заголовки столбцов
                Row headerRow = new Row();
                foreach (DataColumn column in dataTable.Columns)
                {
                    Cell cell = CreateTextCell(column.ColumnName);
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);



                // Заполняем данными из таблицы
                foreach (DataRow row in dataTable.Rows)
                {
                    Row dataRow = new Row();
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        Cell cell = CreateTextCell(row[column.ColumnName].ToString());
                        dataRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(dataRow);
                }
                


                // Сохраняем и закрываем документ
                workbookPart.Workbook.Save();
                spreadsheetDocument.Dispose();

                MessageBox.Show("Excel файл успешно создан.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании Excel файла: " + ex.Message);
            }


        }
        private Cell CreateTextCell(string text)
        {
            Cell cell = new Cell();
            cell.DataType = CellValues.String;
            cell.CellValue = new CellValue(text);
            return cell;
        }
    }
    }


