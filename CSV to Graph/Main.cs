using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using LicenseActivation;
using Microsoft.Win32;
using Image = iTextSharp.text.Image;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using ScottPlot;

namespace CSV_Graph
{
    public partial class Main : Form
    {

        string file;

        string sos = "";
        string eos = "";

        public static bool licenseActivated = false;

        ScottPlot.WinForms.FormsPlot formsPlot = new ScottPlot.WinForms.FormsPlot();

        DataTable dt = new DataTable();

        string[] content = new string[0];

        List<ScottPlot.Plottables.Signal> source = new List<ScottPlot.Plottables.Signal>();

        public Main()
        {
            string reply = keyread("VG");

            if (reply == null)
            {

                Form1 licenseActivation = new Form1();

                licenseActivation.ShowDialog();

                licenseActivated = licenseActivation.activated;
            }
            else if (reply != null)
            {

                if (bool.Parse(reply))
                {
                    licenseActivated = true;
                }
                else
                {
                    Form1 licenseActivation = new Form1();

                    licenseActivation.ShowDialog();

                    licenseActivated = licenseActivation.activated;
                }
            }

            if (!licenseActivated)
            {
                Environment.Exit(0);
            }

            InitializeComponent();

            if (!Directory.Exists(FileLocator.mainLocation))
            {
                Directory.CreateDirectory(FileLocator.mainLocation);
            }
        }

        public string keyread(string KeyName)
        {
            try
            {
                // Opening the registry key
                RegistryKey rk = Registry.CurrentUser;
                // Open a subKey as read-only
                RegistryKey sk1 = rk.OpenSubKey(@"SOFTWARE\VG");
                // If the RegistrySubKey doesn't exist -> (null)
                if (sk1 == null)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        // If the RegistryKey exists I get its value
                        // or null is returned.
                        return (string)sk1.GetValue(KeyName.ToUpper());
                    }
                    catch (Exception ex1)
                    {
                        System.Windows.MessageBox.Show(ex1.Message);
                        return null;
                    }
                }
            }
            catch (Exception ex1)
            {
                System.Windows.MessageBox.Show(ex1.Message);
                return null;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            formsPlot.Dock = DockStyle.Fill;
            tabControl.TabPages[0].Controls.Add(formsPlot);
            formsPlot.MouseHover += FormsPlot_MouseMove;
        }

        private void FormsPlot_MouseMove(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
            }
            catch { }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else 
                {
                    dt.Clear();
                    source.Clear();
                    content = new string[0];
                    Environment.Exit(0); 
                }
            }
            catch { }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                About about = new About();
                about.ShowDialog();
            }
            catch
            {

            }
        }

        private void pngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SFD.Filter = "PNG (*.png)|*.png";
                if (SFD.ShowDialog() == DialogResult.OK)
                {

                    System.Windows.MessageBox.Show("Graph image exported");
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Couldn't perform the requested action!");
            }
        }

        private void pdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SFD.Filter = "PDF (*.pdf)|*.pdf";
                if (SFD.ShowDialog() == DialogResult.OK)
                {

                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                    PdfWriter.GetInstance(pdfDoc, new FileStream(SFD.FileName, FileMode.Create));
                    pdfDoc.Open();
                    Image chartImage = Image.GetInstance("chartImage.png");
                    // Scale the image to fit the entire page
                    chartImage.ScaleAbsolute(pdfDoc.PageSize.Width, pdfDoc.PageSize.Height);

                    // Position the image at the top-left corner
                    chartImage.SetAbsolutePosition(0, 0);

                    pdfDoc.Add(chartImage);
                    pdfDoc.Close();
                    System.Windows.MessageBox.Show("Graph PDF exported");
                }
            }
            catch
            {
                System.Windows.MessageBox.Show("Couldn't perform the requested action!");
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Options options = new Options();
                Options.ParentForm = this;
                options.ShowDialog();
            }
            catch
            { }
        }

        public void temp(string title, string xLabel, string yLabel, decimal markerSize, bool markerVisible, string yMax, string yMin, bool autoScale)
        {
            foreach(var signal in source)
            {
                signal.MaximumMarkerSize = markerVisible ? (int)markerSize : 0;
            }

            if (autoScale)
            {
                formsPlot.Plot.Axes.AutoScale();
            }
            else
            {
                formsPlot.Plot.Axes.AutoScaleX();
                formsPlot.Plot.Axes.SetLimitsY(int.Parse(yMin), int.Parse(yMax));
            }
            
            formsPlot.Plot.Title(title);
            formsPlot.Plot.YLabel(yLabel);
            formsPlot.Plot.XLabel(xLabel);
            formsPlot.Refresh();
        }

        public void change(string title, string x, string y, decimal markerSize, bool markerVisible, string ymax, string ymin, bool autoScale)
        {
            try
            {
                temp(title, x, y, markerSize, markerVisible, ymax, ymin, autoScale);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //MessageBox.Show("There was some error performing the action");
            }
        }

        public void openf()
        {
            try
            {
                if (helpToolStripMenuItem.Enabled == true)
                {
                    source.Clear();
                    formsPlot.Plot.Clear();
                }

                helpToolStripMenuItem.Enabled = true;

                content = File.ReadAllLines(file);

                int dataStartingIndex = 0;

                string detection = content[0].Split(',')[0];

                string ModelNumber = "";

                int DeviceID = -1;

                FileDataList list = null;

                if (detection.IndexOf("MODEL") != -1)
                {
                    ModelNumber = detection.Split(':')[1];
                    dataStartingIndex = 2;
                    if (!Directory.Exists(FileLocator.mainLocation))
                    {
                        Directory.CreateDirectory(FileLocator.mainLocation);
                    }
                    DeviceID = int.Parse(content[1].Split(',')[0].Split(':')[1]);
                    list = new FileDataList(DeviceID);
                }

                string[] headers = content[dataStartingIndex].Split(',');

                string tn = headers[0];
                int len = headers.Length - 1;

                dt = new DataTable();

                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }

                for (int i = dataStartingIndex + 1; i < content.Length; i++)
                {
                    string[] data = content[i].Split(',');
                    if (data.Length == headers.Length)
                    {
                        dt.Rows.Add(data);
                    }
                }

                dataView.DataSource = dt;

                DateTime[] dateTimes = dt.AsEnumerable().Select(row => row.Field<string>(0)).Where(str => DateTime.TryParse(str, out _)).Select(str => DateTime.Parse(str)).ToArray();
                double[] dateTimesOADate = dateTimes.Select(dt => dt.ToOADate()).ToArray();
                for (int i = 1; i < headers.Length; i++)
                {
                    double[] data = dt.AsEnumerable().Select(row => row.Field<string>(i)).Where(strValue => float.TryParse(strValue, out _)).Select(strValue => double.Parse(strValue)).ToArray();
                    if(dateTimesOADate.Length == data.Length)
                    {
                        var signal = formsPlot.Plot.Add.Signal(data);
                        
                        signal.Label = headers[i];
                        signal.Data.XOffset = dateTimesOADate[0];
                        signal.Data.Period = dateTimesOADate[1] - dateTimesOADate[0];
                        signal.MaximumMarkerSize = 10;

                        source.Add(signal);
                    }
                }
                formsPlot.Plot.Legend.IsVisible = true;
                formsPlot.Plot.Axes.DateTimeTicksBottom();
                formsPlot.Plot.Axes.AutoScale();
                formsPlot.Plot.Render();
                formsPlot.Refresh();

                DateTime startTS = dateTimes[0];
                DateTime endTS = dateTimes[dateTimes.Length - 1];
                string timespanB = "";
                timespanB = (endTS - startTS).ToString();
                filterStart.MinDate = startTS; filterStart.MaxDate = endTS;
                filterEnd.MinDate = startTS; filterEnd.MaxDate = endTS;
                filterStart.Value = startTS;
                filterEnd.Value = endTS;
                if (list != null)
                {
                    string newFileLocation = Path.Combine(FileLocator.mainLocation, DeviceID.ToString(), Path.GetFileNameWithoutExtension(file) + ".dat");
                    using (File.Create(Path.Combine(FileLocator.mainLocation, DeviceID.ToString(), Path.GetFileNameWithoutExtension(file) + ".dat"))) { }
                    using (StreamWriter sw = new StreamWriter(newFileLocation, false))
                    {
                        for (int i = dataStartingIndex; i < content.Length; i++)
                        {
                            sw.WriteLine(content[i]);
                        }
                    }

                    list.addInfo(startTS, endTS, content[dataStartingIndex].Split(',').ToList<string>(), Path.GetFileNameWithoutExtension(file));
                    list.SaveFileInfo(DeviceID);
                }

            }
            catch (Exception Ex)
            {
                System.Windows.MessageBox.Show("The file is being used by another application. Close and try again. " + Ex.Message);
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (dt != null && filterStart.Value < filterEnd.Value)
                {
                    var startIndex = -1;
                    var endIndex = -1;

                    // Parse filterStart and filterEnd as DateTime objects
                    DateTime startDateTime = filterStart.Value;
                    DateTime endDateTime = filterEnd.Value;

                    // Find the index of the row with a datetime value equal to or greater than filterStart
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DateTime rowDateTime = Convert.ToDateTime(dt.Rows[i][0]); // Assuming column 0 contains datetime values

                        if (rowDateTime >= startDateTime)
                        {
                            startIndex = i;
                            break;
                        }
                    }

                    // Find the index of the row with a datetime value equal to or greater than filterEnd
                    for (int i = startIndex; i < dt.Rows.Count; i++)
                    {
                        DateTime rowDateTime = Convert.ToDateTime(dt.Rows[i][0]); // Assuming column 0 contains datetime values

                        if (rowDateTime >= endDateTime)
                        {
                            endIndex = i;
                            break;
                        }
                    }

                    if (startIndex != -1 && endIndex != -1)
                    {
                        DisplayRowsInRange(startIndex, endIndex);
                    }
                    else
                    {
                        // Handle case when no matching rows are found
                        MessageBox.Show("No matching rows found.");
                    }
                }
                else
                {
                    // Handle case when dt is null or filterStart is not less than filterEnd
                    MessageBox.Show("Invalid input values.");
                }
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                MessageBox.Show(ex.Message);
            }

        }

        private void DisplayRowsInRange(int startRow, int endRow)
        {
            // Clone the structure of the original DataTable
            DataTable filteredDataTable = dt.Clone();

            // Copy the desired rows to the new DataTable
            for (int i = startRow; i < endRow && i < dt.Rows.Count; i++)
            {
                filteredDataTable.ImportRow(dt.Rows[i]);
            }

            // Set the DataSource property of DataGridView to the filtered DataTable
            dataView.DataSource = filteredDataTable;


        }

        private void clearFilter_Click(object sender, EventArgs e)
        {
            dataView.DataSource = dt;
            dataView.Refresh();
        }

        private void exportPDFButton_Click(object sender, EventArgs e)
        {
            ExportPDF();
        }

        private void ExportPDF()
        {
            SFD.Filter = "PDF (*.pdf)|*.pdf";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                // Create a new PDF document in landscape orientation
                Document doc = new Document(PageSize.LETTER.Rotate());
                BaseFont headerFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font titleFont = new Font(headerFont, 25, 1, BaseColor.BLACK);
                Font subtitleFont = new Font(headerFont, 20, 1, BaseColor.BLACK);
                Paragraph titlePara = new Paragraph(Properties.Settings.Default.Title, titleFont);
                titlePara.Alignment = Element.ALIGN_CENTER;
                Paragraph subtitlePara = new Paragraph(Properties.Settings.Default.Subtitle, subtitleFont);
                subtitlePara.Alignment = Element.ALIGN_CENTER;

                // Create an instance of your custom PdfPageEvent class
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(SFD.FileName, FileMode.Create));
                PageNumberEventHandler pageEventHandler = new PageNumberEventHandler();
                writer.PageEvent = pageEventHandler;
                Font chf = new Font(headerFont, 10, 1, BaseColor.WHITE);

                doc.Open();

                if (!String.IsNullOrEmpty(Properties.Settings.Default.Logo))
                {
                    if (File.Exists(Properties.Settings.Default.Logo))
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromFile(Properties.Settings.Default.Logo), System.Drawing.Imaging.ImageFormat.Jpeg);

                        float width = 150f; // Replace with your desired width
                        float height = 100f; // Replace with your desired height

                        // Set absolute position and scale to fit within the specified rectangle
                        img.SetAbsolutePosition(50f, doc.PageSize.Height - 100);
                        img.ScaleToFit(width, height);

                        // Add the image to the PDF document
                        doc.Add(img);
                    }
                }

                if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.Title))
                {
                    doc.Add(titlePara);
                    doc.Add(Chunk.NEWLINE);
                }

                if (!String.IsNullOrWhiteSpace(Properties.Settings.Default.Subtitle))
                {
                    doc.Add(subtitlePara);
                    doc.Add(Chunk.NEWLINE);
                }

                // Iterate through the dataGridView in segments of 13 channels
                int startIndex = 0;
                while (startIndex < dataView.Columns.Count)
                {
                    int endIndex = Math.Min(startIndex + 6, dataView.Columns.Count - 1);
                    // Create a PdfPTable for the current segment
                    PdfPTable table = new PdfPTable(endIndex - startIndex + 1); // Number of columns

                    if (startIndex > 0)
                    {
                        table = new PdfPTable(endIndex - startIndex + 2);
                    }

                    table.WidthPercentage = 100; // Set width to occupy full page width


                    if (startIndex > 0)
                    {
                        DataGridViewColumn col = dataView.Columns[0];
                        PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, chf));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = BaseColor.ORANGE;
                        table.AddCell(cell);
                    }

                    // Add header row with column names
                    for (int i = startIndex; i <= endIndex; i++)
                    {
                        DataGridViewColumn col = dataView.Columns[i];
                        PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, chf));
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.BackgroundColor = BaseColor.ORANGE;

                        table.AddCell(cell);
                    }

                    // Add data rows
                    foreach (DataGridViewRow row in dataView.Rows)
                    {
                        if (startIndex > 0)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(row.Cells[0].Value.ToString(), chf));
                                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);
                            }
                            else
                            {
                                table.AddCell(new Phrase());
                            }
                        }
                        for (int i = startIndex; i <= endIndex; i++)
                        {
                            if (row.Cells[i].Value != null)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(row.Cells[i].Value.ToString(), chf));
                                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                                table.AddCell(cell);
                            }
                            else
                            {
                                table.AddCell(new Phrase());
                            }
                        }
                    }
                    table.HeaderRows = 1;
                    // Add the table to the PDF document
                    doc.Add(table);
                    doc.Add(new Paragraph(Environment.NewLine));
                    startIndex = endIndex + 1; // Move to the next segment
                }

                doc.Close();
                System.Windows.Forms.MessageBox.Show("The PDF file was exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataView != null)
            {
                SFD = new System.Windows.Forms.SaveFileDialog();
                SFD.Filter = "CSV (*.csv)|*.csv";
                if (SFD.ShowDialog(this) == DialogResult.OK)
                {
                    int columnCount = dataView.Columns.Count;
                    string[] outputCsv = new string[dataView.Rows.Count];
                    try
                    {
                        string columnNames = string.Join(",", dataView.Columns.Cast<DataGridViewColumn>().Select(col => col.HeaderText));
                        outputCsv[0] += columnNames;

                        for (int i = 1; (i - 1) < dataView.Rows.Count - 1; i++)
                        {
                            outputCsv[i] = string.Join(",", dataView.Rows[i].Cells.Cast<DataGridViewCell>().Select(cell => cell.Value?.ToString() ?? ""));
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show("Some error exporting the CSV", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    string fna = SFD.FileName;
                    Thread csvWrite = new Thread(() =>
                    {
                        using (StreamWriter writer = new StreamWriter(fna))
                        {
                            foreach (string line in outputCsv)
                            {
                                writer.WriteLine(line);
                            }
                        }
                    });
                    csvWrite.Start();

                    System.Windows.Forms.MessageBox.Show("Data Exported Successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReportCustomizer customizer = new ReportCustomizer();
            customizer.ShowDialog();
        }

        private void OFD_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                file = OFD.FileName;
                try
                {
                    if (Path.GetExtension(OFD.FileName).ToLower() == ".dat")
                    {
                        openf();
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.ToString());
                }
            }
        }

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dateTimeSelector selector = new dateTimeSelector(Directory.GetDirectories(FileLocator.mainLocation).ToList<string>());
            DialogResult selectorResult = selector.ShowDialog();
            if (selectorResult == DialogResult.OK)
            {
                FileDataList fileInformation = new FileDataList(int.Parse(Path.GetFileName(selector.selectedFolder)));

                try
                {
                    DateTime startSelection = selector.selectionStart;
                    DateTime endSelection = selector.selectionEnd;

                    //List<string> files = fileInformation.fileList.Where(fileData => DateTime.Compare(fileData.startTimeStamp, startSelection) >= 0 && DateTime.Compare(fileData.endTimeStamp, endSelection) <= 0 ).Select(fileData => Path.Combine(FileLocator.mainLocation, Path.GetFileName(selector.selectedFolder), fileData.fileName + ".dat")).ToList();

                    List<string> files = new List<string>();

                    foreach (var fileData in fileInformation.fileList)
                    {
                        // Check if fileData is within the specified range
                        if (fileData.startTimeStamp >= startSelection && fileData.endTimeStamp <= endSelection)
                        {
                            string filePath = Path.Combine(FileLocator.mainLocation, Path.GetFileName(selector.selectedFolder), fileData.fileName + ".dat");
                            files.Add(filePath);
                        }
                        else
                        {
                            // Debugging information to understand the comparison results
                            int comparisonResultStart = startSelection.CompareTo(fileData.startTimeStamp);
                            int comparisonResultEnd = endSelection.CompareTo(fileData.endTimeStamp);

                            MessageBox.Show($"Start: {comparisonResultStart}, End: {comparisonResultEnd}");

                            // Check if fileData.endTimeStamp is within the range (debugging purpose)
                            if (fileData.endTimeStamp >= startSelection && fileData.endTimeStamp <= endSelection)
                            {
                                MessageBox.Show($"End timestamp within the range: {fileData.fileName}");
                            }
                        }
                    }

                    if (files.Count > 0)
                    {
                        var dataTables = files.Select(LoadCsv).ToList();
                        dt = MergeDataTables(dataTables, startSelection, endSelection);
                        dataView.DataSource = dt;
                        LoadGraph();
                        filterStart.Value = startSelection;
                        filterEnd.Value = endSelection;
                    }
                    else
                    {
                        MessageBox.Show("No data in this range", "Empty Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        static DataTable LoadCsv(string filePath)
        {
            DataTable dataTable = new DataTable();

            using (StreamReader reader = new StreamReader(filePath))
            {
                // Assuming the first line contains column headers
                string[] headers = reader.ReadLine().Split(',');

                // Add columns to DataTable
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header.Trim());
                }

                // Read and add data rows
                while (!reader.EndOfStream)
                {
                    string[] rows = reader.ReadLine().Split(',');
                    DataRow dataRow = dataTable.NewRow();

                    for (int i = 0; i < headers.Length; i++)
                    {
                        dataRow[i] = rows[i].Trim();
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            return dataTable;
        }

        static DataTable MergeDataTables(IEnumerable<DataTable> dataTables, DateTime startTimestamp, DateTime endTimestamp)
        {
            // Assuming the first column is the timestamp
            DataTable mergedTable = dataTables.First().Copy();

            foreach (DataTable dataTable in dataTables.Skip(1))
            {
                // Merge additional columns
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (!mergedTable.Columns.Contains(column.ColumnName))
                    {
                        mergedTable.Columns.Add(column.ColumnName, column.DataType);
                    }
                }

                // Merge rows based on timestamp
                var query = from row1 in mergedTable.AsEnumerable()
                            join row2 in dataTable.AsEnumerable() on row1[0] equals row2[0] into gj
                            from subRow in gj.DefaultIfEmpty()
                            select subRow != null ? subRow : mergedTable.NewRow();

                // Copy merged data to the new DataTable
                DataTable mergedData = query.CopyToDataTable();
                mergedTable = mergedData.Copy();
                DataRow[] filteredRows = mergedTable.Select($"Timestamp >= #{startTimestamp:yyyy-MM-dd HH:mm:ss}# AND Timestamp <= #{endTimestamp:yyyy-MM-dd HH:mm:ss}#");

                // Create a new DataTable with the filtered rows
                DataTable filteredDataTable = mergedTable.Clone(); // Clone the structure (columns) of the original table

                foreach (DataRow row in filteredRows)
                {
                    filteredDataTable.ImportRow(row);
                }

                mergedTable = filteredDataTable.Copy();
            }

            return mergedTable;
        }

        void LoadGraph()
        {
            if (helpToolStripMenuItem.Enabled == true)
            {

            }

            helpToolStripMenuItem.Enabled = true;

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                //var series = new Series
                //{
                //    Name = dt.Columns[i].ColumnName,
                //    IsVisibleInLegend = true,
                //    ChartType = SeriesChartType.Spline,
                //    MarkerStyle = MarkerStyle.Circle,
                //    MarkerSize = 10,
                //    MarkerStep = (int)(dt.Rows.Count - 1) / 10,
                //    BorderWidth = 3,
                //};

                // Add the series to the Chart control

            }



            sos = dt.Rows[0][0].ToString();
            eos = dt.Rows[dt.Rows.Count - 1][0].ToString();

        }
    }

    public class PageNumberEventHandler : PdfPageEventHelper
    {
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Add page number to the bottom-right corner of each page with some margin
            float margin = 10f;
            float fontSize = 12f;

            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = document.PageSize.Width - 2 * margin;
            table.HorizontalAlignment = Element.ALIGN_RIGHT;

            PdfPCell cell = new PdfPCell(new Phrase("Page " + writer.PageNumber, new Font(Font.FontFamily.HELVETICA, fontSize)));
            cell.Border = 0;
            table.AddCell(cell);

            table.WriteSelectedRows(0, -1, margin, document.Bottom + margin, writer.DirectContent);
        }
    }
}
