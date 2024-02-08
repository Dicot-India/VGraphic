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
using System.Text;
using ScottPlot;

namespace CSV_Graph
{
    public partial class Main : Form
    {

        string file;

        public static bool licenseActivated = false;

        ScottPlot.WinForms.FormsPlot formsPlot = new ScottPlot.WinForms.FormsPlot();

        DataTable dt = new DataTable();

        string[] content = new string[0];

        List<ScottPlot.Plottables.Signal> source = new List<ScottPlot.Plottables.Signal>();

        DateTime x_value = new DateTime();

        double y_value = 0;

        System.Windows.Forms.Label labelTip = new System.Windows.Forms.Label();

        DataTable dtCopy = new DataTable();

        List<int> selectedIndices = new List<int>();

        bool coordinatesVisible = true;

        public Main()
        {
            string reply = keyRead("VG");

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

        public string keyRead(string KeyName)
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
            dataView.Font = new System.Drawing.Font("Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            formsPlot.Dock = DockStyle.Fill;
            tabControl.TabPages[0].Controls.Add(formsPlot);
            formsPlot.MouseMove += formsPlot_MouseMoved;
            labelTip.BackColor = System.Drawing.Color.White;
            labelTip.Font = new System.Drawing.Font("Times New Roman", 10f);
            labelTip.ForeColor = System.Drawing.Color.Black;
            labelTip.Size = new System.Drawing.Size(200, 40);
            formsPlot.Controls.Add(labelTip);
            formsPlot.Cursor = Cursors.Cross;
            MenuItem coordinatesVisibility = new MenuItem("Toggle Coordinates");
            formsPlot.Menu.Add("Toggle Coordinates", CoordinatesVisibility_Click);
        }

        private void CoordinatesVisibility_Click(IPlotControl control)
        {
            labelTip.Visible = !coordinatesVisible;
            coordinatesVisible = !coordinatesVisible;
        }

        private void formsPlot_MouseMoved(object sender, MouseEventArgs e)
        {
            if (source.Count > 0 && coordinatesVisible)
            {
                Pixel mousePixel = new Pixel(e.X, e.Y);
                Coordinates mouseCoordinates = formsPlot.Plot.GetCoordinates(mousePixel);
                x_value = DateTime.FromOADate(mouseCoordinates.X);
                string x_Display = x_value.ToString("HH:mm:ss dd/MM/yyyy");
                y_value = mouseCoordinates.Y;
                labelTip.Text = $"Time = {x_Display}\nValue = {y_value}";
                labelTip.Location = new System.Drawing.Point(e.X + 20, e.Y - 20);
                labelTip.BringToFront();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
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
                    formsPlot.Plot.SavePng(SFD.FileName, 1920, 1080);
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
                    float totalYOffset = 0;
                    formsPlot.Plot.SavePng("chartImage.png", 1920, 1080);
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                    PdfWriter.GetInstance(pdfDoc, new FileStream(SFD.FileName, FileMode.Create));
                    pdfDoc.Open();
                    if (!String.IsNullOrEmpty(Properties.Settings.Default.chartLogo))
                    {
                        if (File.Exists(Properties.Settings.Default.chartLogo))
                        {
                            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromFile(Properties.Settings.Default.chartLogo), System.Drawing.Imaging.ImageFormat.Jpeg);

                            float width = 150f; // Replace with your desired width
                            float height = 100f; // Replace with your desired height

                            // Set absolute position and scale to fit within the specified rectangle
                            img.SetAbsolutePosition(50f, pdfDoc.PageSize.Height - 90f);
                            img.ScaleToFit(width, height);

                            // Add the image to the PDF document
                            pdfDoc.Add(img);
                            totalYOffset += 10;
                        }
                    }

                    if (Properties.Settings.Default.chartStat)
                    {
                        BaseFont headerFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                        iTextSharp.text.Font chf = new iTextSharp.text.Font(headerFont, 10, 1, BaseColor.BLACK);

                        PdfPTable statTable = new PdfPTable(4);
                        statTable.WidthPercentage = 50;

                        foreach (DataGridViewColumn col in dataGridView1.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, chf));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = BaseColor.WHITE;
                            statTable.AddCell(cell);
                        }

                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            foreach (DataGridViewCell rowCell in row.Cells)
                                if (rowCell.Value != null)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(rowCell.Value.ToString(), chf));
                                    cell.BackgroundColor = BaseColor.WHITE;
                                    statTable.AddCell(cell);
                                }
                                else
                                {
                                    statTable.AddCell(new Phrase());
                                }
                        }

                        statTable.HeaderRows = 1;
                        // Add the table to the PDF document
                        pdfDoc.Add(statTable);
                    }

                    Image chartImage = Image.GetInstance("chartImage.png");
                    // Scale the image to fit the entire page
                    chartImage.ScaleAbsolute(pdfDoc.PageSize.Width - 20f, pdfDoc.PageSize.Height - 150f);

                    // Position the image at the top-left corner
                    chartImage.SetAbsolutePosition(10, 20);

                    pdfDoc.Add(chartImage);
                    pdfDoc.Close();
                    MessageBox.Show("Graph PDF exported");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Couldn't Export PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chartSetup ChartSetup = new chartSetup();
            ChartSetup.ShowDialog();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options options = new Options();
            Options.ParentForm = this;
            options.ShowDialog();
        }

        public void temp(string title, string xLabel, string yLabel, decimal markerSize, bool markerVisible, string yMax, string yMin, bool autoScale)
        {
            foreach (var signal in source)
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

        public void change(string title, string x, string y, decimal markerSize, bool markerVisible, string yMax, string yMin, bool autoScale)
        {
            try
            {
                temp(title, x, y, markerSize, markerVisible, yMax, yMin, autoScale);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Decorating Graph", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void openFile()
        {
            try
            {
                if (helpToolStripMenuItem.Enabled == true)
                {
                    source.Clear();
                    formsPlot.Plot.Clear();
                }

                helpToolStripMenuItem.Enabled = true;
                var enc = Encoding.UTF8;
                content = File.ReadAllLines(file, enc);

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
                dtCopy = dt;

                DateTime[] dateTimes = dt.AsEnumerable().Select(row => row.Field<string>(0)).Where(str => DateTime.TryParse(str, out _)).Select(str => DateTime.Parse(str)).ToArray();
                double[] dateTimesOADate = dateTimes.Select(dt => dt.ToOADate()).ToArray();
                for (int i = 1; i < headers.Length; i++)
                {
                    double[] data = dt.AsEnumerable().Select(row => row.Field<string>(i)).Where(strValue => float.TryParse(strValue, out _)).Select(strValue => double.Parse(strValue)).ToArray();
                    if (dateTimesOADate.Length == data.Length)
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
                formsPlot.Plot.Axes.SetLimitsX(dateTimesOADate[0], dateTimesOADate[dateTimesOADate.Length - 1]);

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
                fileStatistics(dt);
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
                    DateTime[] dateTimes = dtCopy.AsEnumerable().Select(row => row.Field<string>(0)).Where(str => DateTime.TryParse(str, out _)).Select(str => DateTime.Parse(str)).ToArray();
                    // Find the index of the row with a datetime value equal to or greater than filterStart
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dateTimes[i] >= startDateTime)
                        {
                            startIndex = i;
                            break;
                        }
                    }

                    // Find the index of the row with a datetime value equal to or greater than filterEnd
                    for (int i = startIndex; i < dt.Rows.Count; i++)
                    {
                        if (dateTimes[i] > endDateTime)
                        {
                            break;
                        }
                        endIndex = i;
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

            if (helpToolStripMenuItem.Enabled == true)
            {
                source.Clear();
                formsPlot.Plot.Clear();
            }

            helpToolStripMenuItem.Enabled = true;
            fileStatistics(filteredDataTable);
            DateTime[] dateTimes = filteredDataTable.AsEnumerable().Select(row => row.Field<string>(0)).Where(str => DateTime.TryParse(str, out _)).Select(str => DateTime.Parse(str)).ToArray();
            double[] dateTimesOADate = dateTimes.Select(dt => dt.ToOADate()).ToArray();
            dtCopy = filteredDataTable;
            List<string> headers = new List<string>();

            for (int c = 0; c < filteredDataTable.Columns.Count; c++)
            {
                headers.Add(filteredDataTable.Columns[c].ColumnName);
            }

            for (int i = 1; i < headers.Count; i++)
            {
                double[] data = filteredDataTable.AsEnumerable().Select(row => row.Field<string>(i)).Where(strValue => float.TryParse(strValue, out _)).Select(strValue => double.Parse(strValue)).ToArray();
                if (dateTimesOADate.Length == data.Length)
                {
                    var signal = formsPlot.Plot.Add.Signal(data);

                    signal.Label = headers[i];
                    signal.Data.XOffset = dateTimesOADate[0];
                    signal.Data.Period = dateTimesOADate[1] - dateTimesOADate[0];
                    signal.MaximumMarkerSize = 10;
                    if (selectedIndices.Count > 0)
                    {
                        signal.IsVisible = selectedIndices.Contains(i - 1);
                    }
                    source.Add(signal);
                }
            }
            formsPlot.Plot.Legend.IsVisible = true;
            formsPlot.Plot.Axes.DateTimeTicksBottom();
            formsPlot.Plot.Axes.AutoScale();
            formsPlot.Plot.Render();
            formsPlot.Refresh();
        }

        private void clearFilter_Click(object sender, EventArgs e)
        {
            dataView.DataSource = dt;
            dtCopy = dt;
            dataView.Refresh();
            source.Clear();
            formsPlot.Plot.Clear();
            DateTime[] dateTimes = dt.AsEnumerable().Select(row => row.Field<string>(0)).Where(str => DateTime.TryParse(str, out _)).Select(str => DateTime.Parse(str)).ToArray();
            double[] dateTimesOADate = dateTimes.Select(dt => dt.ToOADate()).ToArray();
            dtCopy = dt;
            List<string> headers = new List<string>();

            for (int c = 0; c < dt.Columns.Count; c++)
            {
                headers.Add(dt.Columns[c].ColumnName);
            }

            for (int i = 1; i < headers.Count; i++)
            {
                double[] data = dt.AsEnumerable().Select(row => row.Field<string>(i)).Where(strValue => float.TryParse(strValue, out _)).Select(strValue => double.Parse(strValue)).ToArray();
                if (dateTimesOADate.Length == data.Length)
                {
                    var signal = formsPlot.Plot.Add.Signal(data);

                    signal.Label = headers[i];
                    signal.Data.XOffset = dateTimesOADate[0];
                    signal.Data.Period = dateTimesOADate[1] - dateTimesOADate[0];
                    signal.MaximumMarkerSize = 10;
                    if (selectedIndices.Count > 0)
                    {
                        signal.IsVisible = selectedIndices.Contains(i - 1);
                    }
                    source.Add(signal);
                }
            }
            formsPlot.Plot.Legend.IsVisible = true;
            formsPlot.Plot.Axes.DateTimeTicksBottom();
            formsPlot.Plot.Axes.AutoScale();
            formsPlot.Plot.Render();
            formsPlot.Refresh();
            fileStatistics(dt);
        }

        private void exportPDFButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.UseWaitCursor = true;
                ExportPDF();
                this.UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ExportPDF()
        {
            try
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
                    Font chf = new Font(headerFont, 10, 1, BaseColor.BLACK);

                    doc.Open();

                    if (!String.IsNullOrEmpty(Properties.Settings.Default.Logo))
                    {
                        if (File.Exists(Properties.Settings.Default.Logo))
                        {
                            Image img = Image.GetInstance(System.Drawing.Image.FromFile(Properties.Settings.Default.Logo), System.Drawing.Imaging.ImageFormat.Jpeg);

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

                    if (Properties.Settings.Default.tableStat)
                    {
                        PdfPTable statTable = new PdfPTable(4);
                        statTable.WidthPercentage = 50;

                        foreach (DataGridViewColumn col in dataGridView1.Columns)
                        {
                            PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, chf));
                            cell.HorizontalAlignment = Element.ALIGN_CENTER;
                            cell.BackgroundColor = BaseColor.WHITE;
                            statTable.AddCell(cell);
                        }

                        for (int i = 0; i < dataGridView1.Rows.Count; i++)
                        {
                            if (selectedIndices.Contains(i))
                            {
                                DataGridViewRow row = dataGridView1.Rows[i];
                                foreach (DataGridViewCell rowCell in row.Cells)
                                {
                                    if (rowCell.Value != null)
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase(rowCell.Value.ToString(), chf));
                                        cell.BackgroundColor = BaseColor.WHITE;
                                        statTable.AddCell(cell);
                                    }
                                    else
                                    {
                                        statTable.AddCell(new Phrase());
                                    }
                                }
                            }
                        }

                        statTable.HeaderRows = 1;
                        // Add the table to the PDF document
                        doc.Add(statTable);
                        doc.Add(new Paragraph(Environment.NewLine));
                    }

                    // Iterate through the dataGridView in segments of 13 channels
                    int startIndex = 0;

                    while (startIndex < dataView.Columns.Count)
                    {

                        int endIndex = Math.Min(startIndex + 6, dataView.Columns.Count - 1);
                        // Create a PdfPTable for the current segment
                        PdfPTable table = new PdfPTable(endIndex - startIndex + 1); // Number of columns

                        if(selectedIndices.Count > 0)
                        {
                            table = new PdfPTable(selectedIndices.Count + 1);
                        }

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
                            cell.BackgroundColor = BaseColor.WHITE;
                            table.AddCell(cell);
                        }


                        // Add header row with column names
                        for (int i = startIndex; i <= endIndex; i++)
                        {
                            if (selectedIndices.Contains(i - 1) || i == 0)
                            {
                                DataGridViewColumn col = dataView.Columns[i];
                                PdfPCell cell = new PdfPCell(new Phrase(col.HeaderText, chf));
                                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                                cell.BackgroundColor = BaseColor.WHITE;

                                table.AddCell(cell);
                            }
                        }

                        // Add data rows
                        foreach (DataGridViewRow row in dataView.Rows)
                        {
                            if (startIndex > 0)
                            {
                                if (row.Cells[0].Value != null)
                                {
                                    PdfPCell cell = new PdfPCell(new Phrase(row.Cells[0].Value.ToString(), chf));
                                    cell.BackgroundColor = BaseColor.WHITE;
                                    table.AddCell(cell);
                                }
                                else
                                {
                                    table.AddCell(new Phrase());
                                }
                            }
                            for (int i = startIndex; i <= endIndex; i++)
                            {
                                if (selectedIndices.Contains(i - 1) || i == 0)
                                {
                                    if (row.Cells[i].Value != null)
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase(row.Cells[i].Value.ToString(), chf));
                                        cell.BackgroundColor = BaseColor.WHITE;
                                        table.AddCell(cell);
                                    }
                                    else
                                    {
                                        table.AddCell(new Phrase());
                                    }
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
                    MessageBox.Show("The PDF file was exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                        MessageBox.Show(ex.Message, "Error Exporting CSV", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    MessageBox.Show("Data Exported Successfully", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        openFile();
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

                    if (startSelection < endSelection)
                    {


                        List<string> files = new List<string>();

                        foreach (var fileData in fileInformation.fileList)
                        {
                            DateTime fileStartTime = new DateTime(fileData.startTimeStamp);
                            DateTime fileEndTime = new DateTime(fileData.endTimeStamp);
                            // Check if fileData is within the specified range
                            if ((fileStartTime < endSelection && fileEndTime > startSelection) || (fileStartTime > startSelection && fileEndTime < endSelection))
                            {
                                string filePath = Path.Combine(FileLocator.mainLocation, Path.GetFileName(selector.selectedFolder), fileData.fileName + ".dat");
                                files.Add(filePath);
                            }

                            if (fileStartTime > endSelection)
                            {
                                break;
                            }
                        }

                        if (files.Count > 0)
                        {
                            var dataTables = files.Select(f => LoadCsv(f, startSelection, endSelection)).ToList();
                            dt = MergeDataTables(dataTables, startSelection, endSelection);
                            dataView.DataSource = dt;
                            dtCopy = dt;
                            LoadGraph();
                            filterStart.Value = startSelection;
                            filterStart.MinDate = startSelection;
                            filterStart.MaxDate = endSelection;
                            filterEnd.Value = endSelection;
                            filterEnd.MinDate = startSelection;
                            filterEnd.MaxDate = endSelection;
                            fileStatistics(dt);
                        }
                        else
                        {
                            MessageBox.Show("No data in this range", "Empty Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Start cannot be after end", "Improper Range", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        static DataTable LoadCsv(string filePath, DateTime startSelection, DateTime endSelection)
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

                    DateTime timeStamp = Convert.ToDateTime(rows[0]);
                    if (timeStamp > startSelection && timeStamp < endSelection)
                    {

                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < headers.Length; i++)
                        {
                            dataRow[i] = rows[i].Trim();
                        }

                        dataTable.Rows.Add(dataRow);
                    }
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
                source.Clear();
                formsPlot.Plot.Clear();
            }

            helpToolStripMenuItem.Enabled = true;

            DateTime[] dateTimes = dt.AsEnumerable().Select(row => row.Field<string>(0)).Where(str => DateTime.TryParse(str, out _)).Select(str => DateTime.Parse(str)).ToArray();
            double[] dateTimesOADate = dateTimes.Select(dt => dt.ToOADate()).ToArray();

            List<string> headers = new List<string>();

            for (int c = 0; c < dt.Columns.Count; c++)
            {
                headers.Add(dt.Columns[c].ColumnName);
            }

            for (int i = 1; i < headers.Count; i++)
            {
                double[] data = dt.AsEnumerable().Select(row => row.Field<string>(i)).Where(strValue => float.TryParse(strValue, out _)).Select(strValue => double.Parse(strValue)).ToArray();
                if (dateTimesOADate.Length == data.Length)
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
        }

        private void dataView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void fileStatistics(DataTable table)
        {
            try
            {

                if (dataGridView1.Rows.Count > 1)
                {
                    dataGridView1.Rows.Clear();
                }

                for (int i = 1; i < table.Columns.Count; i++)
                {
                    DataColumn column = table.Columns[i];
                    double min = double.MaxValue;
                    double max = double.MinValue;
                    double sum = 0;
                    int count = 0;

                    foreach (DataRow row in table.Rows)
                    {
                        if (!row.IsNull(column))
                        {
                            double value = Convert.ToDouble(row[column]);
                            if (value < min)
                                min = value;
                            if (value > max)
                                max = value;
                            sum += value;
                            count++;
                        }
                    }

                    double average = sum / count;

                    int rowIndex = dataGridView1.Rows.Add(); // Get the index of the newly added row
                    dataGridView1.Rows[rowIndex].Cells[0].Value = table.Columns[i].ColumnName;
                    dataGridView1.Rows[rowIndex].Cells[1].Value = min;
                    dataGridView1.Rows[rowIndex].Cells[2].Value = max;
                    dataGridView1.Rows[rowIndex].Cells[3].Value = average;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Getting Statistics", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            List<string> channelNames = new List<string>();

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                channelNames.Add(dt.Columns[i].ColumnName);
            }

            ParameterSelctor parameterSelector = new ParameterSelctor(channelNames);
            if (parameterSelector.ShowDialog() == DialogResult.OK)
            {
                dataView.DataSource = dtCopy;
                selectedIndices = parameterSelector.selectedChannelID;
                for (int i = 0; i < channelNames.Count; i++)
                {
                    dataView.Columns[i + 1].Visible = selectedIndices.Contains(i);
                    source[i].IsVisible = selectedIndices.Contains(i);
                    dataGridView1.Rows[i].Visible = selectedIndices.Contains(i);
                }
            }
        }

        private void splitContainer_SplitterMoved(object sender, SplitterEventArgs e)
        {

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

            PdfPCell cell = new PdfPCell(new Phrase("Page " + writer.PageNumber, new Font(iTextSharp.text.Font.FontFamily.HELVETICA, fontSize)));
            cell.Border = 0;
            table.AddCell(cell);

            table.WriteSelectedRows(0, -1, margin, document.Bottom + margin, writer.DirectContent);
        }
    }
}
