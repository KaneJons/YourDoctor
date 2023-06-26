using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
using YourDoctor.WiForms.Patient;
using System.Web.UI.WebControls;


namespace YourDoctor.WiForms.Log
{
    /// <summary>
    /// Логика взаимодействия для pageLog.xaml
    /// </summary>
    public partial class pageLog : System.Windows.Controls.Page
    {
        public pageLog()
        {
            InitializeComponent();
            string sql = "SELECT * from doctor";
            Connection.Table_Fill("Доктор", sql);
            var soursefio = from row in Connection.ds.Tables["Доктор"].AsEnumerable()
                            let family = row.Field<string>("family")
                            let imy = row.Field<string>("imy")
                            let otchestvo = row.Field<string>("otchestvo")
                            select $"{family} {imy} {otchestvo}";

            comboBox1.Items.Clear();
            comboBox1.Items.Add(""); 

            foreach (var item in soursefio)
            {
                comboBox1.Items.Add(item);
            }
            filters["patient_fio"] = null;
            filters["doctor_fio"] = null;
            filters["date"] = null;
           

        }
        public void Page_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT appointment.id, CONCAT(doctor.family, ' ', doctor.imy, ' ', doctor.otchestvo) AS doctor_fio," +
                "CONCAT(patient.family, ' ', patient.imy, ' ', patient.otchestvo) AS patient_fio," +
                "service.name AS service_name, appointment.date, appointment.time, appointment.status, appointment.treatment " +
                "FROM appointment " +
                "JOIN doctor ON appointment.doctor_id = doctor.id " +
                "JOIN patient ON appointment.patient_id = patient.id " +
                "JOIN service ON appointment.service_id = service.id order by id asc;";
            Connection.Table_Fill("Запись", sql);
            nurseDataGrid.ItemsSource = Connection.ds.Tables["Запись"].DefaultView;

            nurseDataGrid.SelectionMode = (DataGridSelectionMode)DataGridSelectionUnit.FullRow;
            ApplyFilters();
        }

        Generate generate = new Generate();
        private void nurseDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (nurseDataGrid.SelectedItem != null) 
            {
                DataRowView selectedRow = nurseDataGrid.SelectedItem as DataRowView;

                if (selectedRow != null)
                {
                    int selectedId = Convert.ToInt32(selectedRow["id"]);
                    DataRowView foundRow = generate.FindRowById(selectedId, "Запись");

                    if (foundRow != null)
                    {
                        int selectedRowIndex = Connection.ds.Tables["Запись"].Rows.IndexOf(foundRow.Row);


                        CustomerLog customerLog = new CustomerLog(selectedRowIndex);
                        customerLog.selectedpatid = "";
                        customerLog.selecteddocid = "";
                        customerLog.selectedserid = "";
                        this.NavigationService.Navigate(new CustomerLog(selectedRowIndex));
                    }
                }
            }


        }

      
        private void btn_addLog_Click(object sender, RoutedEventArgs e)
        {
            AddLog addLog = new AddLog(); 
            addLog.selectedpatid = "";
            addLog.selecteddocid = "";
            addLog.selectedserid = "";

            this.NavigationService.Navigate(new AddLog());
        }

        private void btn_printLog_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем новый документ Excel
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create("Записи.xlsx", SpreadsheetDocumentType.Workbook);

                // Добавляем рабочую книгу
                WorkbookPart workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Добавляем лист
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                // Добавляем лист в книгу
                Sheets sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());
                Sheet sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Записи" };
                sheets.Append(sheet);

                // Получаем таблицу "Запись" из объекта DataSet
                DataTable dataTable = Connection.ds.Tables["Запись"];

                // Заполняем заголовок
                SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Добавляем строку заголовка
                Row titleRow = new Row();
                Cell titleCell = generate.CreateTextCell("Журнал записей к врачам медицинского центра ООО Ваш Доктор");
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
                foreach (System.Windows.Controls.DataGridColumn column in nurseDataGrid.Columns)
                {
                    Cell cell = generate.CreateTextCell(column.Header.ToString());
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                // Заполняем данными из таблицы
                foreach (DataRow row in dataTable.Rows)
                {
                    Row dataRow = new Row();
                    foreach (System.Windows.Controls.DataGridColumn column in nurseDataGrid.Columns)
                    {
                        var binding = column.ClipboardContentBinding as Binding;
                        if (binding != null)
                        {
                            var propertyName = binding.Path.Path;
                            var propertyValue = row[propertyName];
                            Cell cell = generate.CreateTextCell(propertyValue.ToString());
                            dataRow.AppendChild(cell);
                        }
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

        private Dictionary<string, string> filters = new Dictionary<string, string>();

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            filters["patient_fio"] = txtFilter.Text.Trim();
            ApplyFilters();
        }


        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            filters["doctor_fio"] = comboBox1.SelectedItem.ToString();
            ApplyFilters();
        }

        private void datePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePicker.SelectedDate.HasValue)
            {
                string selectedDate = datePicker.SelectedDate.Value.ToString("dd-MM-yyyy"); ;
                filters["date"] = selectedDate;
            }
            else
            {
                filters["date"] = null;
            }
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string filterExpression = string.Empty;
            foreach (var filter in filters)
            {
                if (!string.IsNullOrEmpty(filter.Value))
                {
                    if (!string.IsNullOrEmpty(filterExpression))
                        filterExpression += " AND ";

                    if (filter.Key == "date")
                    {
                        if (DateTime.TryParse(filter.Value, out DateTime selectedDate))
                        {
                            string formattedDate = selectedDate.ToString("yyyy-MM-dd");
                            filterExpression += $"{filter.Key} >= #{formattedDate}# AND {filter.Key} <= #{formattedDate}#";
                        }
                    }
                    else
                    {
                        filterExpression += $"{filter.Key} LIKE '%{filter.Value}%'";
                    }
                }
            }

            DataView dataView = Connection.ds.Tables["Запись"].DefaultView;
            dataView.RowFilter = filterExpression;
            nurseDataGrid.ItemsSource = dataView;
        }


        private void btn_remove_date_Click(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = null;
            filters["date"] = null;
            ApplyFilters();
        }

        
    }
}
