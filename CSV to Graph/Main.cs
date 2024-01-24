using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LicenseActivation;
using Microsoft.Win32;
using Image = iTextSharp.text.Image;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Data.OleDb;
using System.Collections.Generic;

namespace CSV_Graph
{
    public partial class Main : Form
    {

        string file;

        int numberOfZoom = 0;

        string sos = "";
        string eos = "";

        public static bool licenseActivated = false;

        int markerInterval = 200;
        int markerSize = 10;

        TextAnnotation RA = new TextAnnotation();
        TextAnnotation RA1 = new TextAnnotation();
        TextAnnotation RA2 = new TextAnnotation();

        DataTable dt = new DataTable();

        string[] content = new string[0];

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
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = true;
            chart1.MouseWheel += Chart1_MouseWheel;
        }

        private void Chart1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;

            var xMin = xAxis.ScaleView.ViewMinimum;
            var xMax = xAxis.ScaleView.ViewMaximum;
            var yMin = yAxis.ScaleView.ViewMinimum;
            var yMax = yAxis.ScaleView.ViewMaximum;

            int IntervalX = 3;
            int IntervalY = 3;
            try
            {
                if (e.Delta < 0 && numberOfZoom > 0) // Scrolled down.
                {
                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - IntervalX * 2 / Math.Pow(2, numberOfZoom);
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + IntervalX * 2 / Math.Pow(2, numberOfZoom);
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - IntervalY * 2 / Math.Pow(2, numberOfZoom);
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + IntervalY * 2 / Math.Pow(2, numberOfZoom);

                    if (posXStart < 0) posXStart = 0;
                    if (posYStart < 0) posYStart = 0;
                    if (posYFinish > yAxis.Maximum) posYFinish = yAxis.Maximum;
                    if (posXFinish > xAxis.Maximum) posYFinish = xAxis.Maximum;
                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                    numberOfZoom--;
                }
                else if (e.Delta < 0 && numberOfZoom == 0) //Last scrolled dowm
                {
                    yAxis.ScaleView.ZoomReset();
                    xAxis.ScaleView.ZoomReset();
                }
                else if (e.Delta > 0) // Scrolled up.
                {

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - IntervalX / Math.Pow(2, numberOfZoom);
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + IntervalX / Math.Pow(2, numberOfZoom);
                    var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - IntervalY / Math.Pow(2, numberOfZoom);
                    var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + IntervalY / Math.Pow(2, numberOfZoom);

                    xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    yAxis.ScaleView.Zoom(posYStart, posYFinish);
                    numberOfZoom++;
                }

                if (numberOfZoom < 0) numberOfZoom = 0;
            }
            catch { }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void displ(bool vis)
        {
            label1.Visible = vis;
        }

        public void LoadExcel()
        {

            if (helpToolStripMenuItem.Enabled == true)
            {
                chart1.Series.Clear();
            }

            helpToolStripMenuItem.Enabled = true;

            // Connection string for Excel
            string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={OFD.FileName};Extended Properties='Excel 12.0 Xml;HDR=YES';";

            // Get the name of the first sheet
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();
                DataTable sheetNames = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                string firstSheetName = sheetNames.Rows[0]["TABLE_NAME"].ToString();
                connection.Close();

                // Query to select all data from the first sheet
                string query = $"SELECT * FROM [{firstSheetName}]";

                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    connection.Open();

                    // Execute the command
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        dt = new DataTable();
                        adapter.Fill(dt);
                        dataView.DataSource = dt;
                    }
                }
            }


            chart1.ChartAreas.SuspendUpdates();
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                var series = new Series
                {
                    Name = dt.Columns[i].ColumnName,
                    IsVisibleInLegend = true,
                    ChartType = SeriesChartType.Spline,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 10,
                    MarkerStep = 200,
                    BorderWidth = 3,
                };

                // Add the series to the Chart control
                chart1.Series.Add(series);
            }

            chart1.Series.SuspendUpdates();
            for (int ik = 0; ik < dt.Columns.Count - 1; ik++)
            {
                chart1.Series[ik].Points.DataBindXY(dt.DefaultView, dt.Columns[0].ColumnName, dt.DefaultView, dt.Columns[ik + 1].ColumnName);
            }
            chart1.Series.ResumeUpdates();
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm dd-MM-yyyy";

            chart1.Annotations.Clear();

            sos = dt.Rows[0][0].ToString();
            eos = dt.Rows[dt.Rows.Count - 1][0].ToString();


            RA = new TextAnnotation();
            RA.Alignment = System.Drawing.ContentAlignment.TopRight;
            RA.ForeColor = System.Drawing.Color.Black;
            RA.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            chart1.Annotations.Add(RA);

            RA1 = new TextAnnotation();
            RA1.ForeColor = System.Drawing.Color.Black;
            RA1.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA1.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA1);

            RA2 = new TextAnnotation();
            RA2.ForeColor = System.Drawing.Color.Black;
            RA2.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA2.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA2);

            RA.Text = "Start: " + sos;
            RA1.Text = "End: " + eos;
            string timespanB = (DateTime.Parse(eos) - DateTime.Parse(sos)).ToString();
            RA2.Text = "Duration: " + timespanB + " (hrs:min:sec)";
            Ra2Pos();
            Ra1Pos();
            RaPos();

            chart1.ChartAreas.ResumeUpdates();
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
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else { Environment.Exit(0); }
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
                    chart1.SaveImage(SFD.FileName, ChartImageFormat.Png);
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
                    chart1.SaveImage("chartImage.png", ChartImageFormat.Png);
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
            try
            {
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    System.Drawing.Printing.PrinterSettings settings;
                    settings = printDialog1.PrinterSettings;
                    printDocument1 = chart1.Printing.PrintDocument;
                    printDocument1.DefaultPageSettings.Landscape = true;
                    System.Drawing.Printing.Margins margins = new System.Drawing.Printing.Margins(10, 10, 10, 10);
                    printDocument1.DefaultPageSettings.Margins = margins;
                    printDocument1.PrintController = new System.Drawing.Printing.StandardPrintController();
                    printDocument1.Print();
                    System.Windows.MessageBox.Show("Graph Printed");
                }
                printDocument1.Dispose();
            }
            catch
            {
                System.Windows.MessageBox.Show("Couldn't perform the requested action!");
            }
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

        public void temp(string t, string f, string x, string y, decimal m, decimal msp, bool mvis, string ymax, string ymin)
        {
            markerInterval = (int)msp;


            if (!String.IsNullOrWhiteSpace(t))
            {
                var cht = new Title();
                cht.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
                cht.Text = t;
                cht.Name = "Title";
                chart1.Titles.Add(cht);
            }

            if (!String.IsNullOrWhiteSpace(f))
            {
                var cht = new Title();
                cht.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
                cht.Text = f;
                cht.Name = "Footer";
                cht.Docking = Docking.Bottom;
                chart1.Titles.Add(cht);
            }

            if (!String.IsNullOrWhiteSpace(x))
            {
                chart1.ChartAreas[0].AxisX.Title = x;
            }

            if (!String.IsNullOrWhiteSpace(y))
            {
                chart1.ChartAreas[0].AxisY.Title = y;
            }

            if (mvis)
            {
                foreach (var s in chart1.Series)
                {
                    s.MarkerSize = Convert.ToInt32(m);
                }
            }
            else
            {
                foreach (var s in chart1.Series)
                {
                    s.MarkerStyle = MarkerStyle.None;
                }
            }

            if (true)
            {
                foreach (var s in chart1.Series)
                {
                    s.MarkerStep = Convert.ToInt32(msp);
                }
            }

            if (!String.IsNullOrWhiteSpace(ymax) && !String.IsNullOrWhiteSpace(ymin))
            {
                if (int.Parse(ymax) > int.Parse(ymin))
                {
                    chart1.ChartAreas[0].AxisY.Maximum = int.Parse(ymax);
                    chart1.ChartAreas[0].AxisY.Minimum = int.Parse(ymin);
                }
                else
                {
                    System.Windows.MessageBox.Show("Maximum value less than minimum! Could not perform this action");
                    chart1.ChartAreas[0].AxisY.Maximum = Double.NaN; // sets the Maximum to NaN
                    chart1.ChartAreas[0].AxisY.Minimum = Double.NaN; // sets the Minimum to NaN
                    chart1.ChartAreas[0].RecalculateAxesScale();
                }
            }
            else
            {
                chart1.ChartAreas[0].AxisY.Maximum = Double.NaN; // sets the Maximum to NaN
                chart1.ChartAreas[0].AxisY.Minimum = Double.NaN; // sets the Minimum to NaN
                chart1.ChartAreas[0].RecalculateAxesScale();
            }
        }

        public void change(string t, string f, string x, string y, decimal m, decimal msp, bool mvis, string ymax, string ymin)
        {
            try
            {
                temp(t, f, x, y, m, msp, mvis, ymax, ymin);
            }
            catch
            {
                System.Windows.MessageBox.Show("There was some error performing the action");
            }
        }

        public void openf()
        {
            try
            {
                if (helpToolStripMenuItem.Enabled == true)
                {
                    chart1.Series.Clear();
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

                for (int i = 0; i < headers.Length; i++)
                {
                    dt.Columns.Add(headers[i].ToLower(), typeof(string));
                    if (i > 0)
                    {
                        var series = new Series
                        {
                            Name = headers[i],
                            IsVisibleInLegend = true,
                            ChartType = SeriesChartType.Spline,
                            MarkerStyle = MarkerStyle.Circle,
                            MarkerSize = 10,
                            MarkerStep = (int)(content.Length - 1) / 10,
                            BorderWidth = 3,
                        };

                        chart1.Series.Add(series);
                    }
                }

                DataRow Row;
                for (int i = dataStartingIndex + 1; i < content.Length; i++)
                {
                    headers = content[i].Split(',');
                    Row = dt.NewRow();
                    for (int f = 0; f < headers.Length; f++)
                    {
                        Row[f] = headers[f];
                    }
                    dt.Rows.Add(Row);
                }

                dataView.DataSource = dt;

                sos = content[dataStartingIndex + 1].Split(',')[0];
                eos = content[content.Length - 1 - dataStartingIndex].Split(',')[0];
                DateTime startTS = new DateTime();
                DateTime endTS = new DateTime();
                string timespanB = "";
                try
                {
                    if (DateTime.TryParse(sos, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out startTS) && DateTime.TryParse(eos, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out endTS))
                    {
                        filterStart.MinDate = startTS;
                        filterStart.MaxDate = endTS;
                        filterStart.Value = startTS;
                        filterEnd.MinDate = startTS;
                        filterEnd.MaxDate = endTS;
                        filterEnd.Value = endTS;
                        timespanB = (endTS - startTS).ToString();
                    }
                }
                catch (Exception Ex)
                {
                    System.Windows.Forms.MessageBox.Show(Ex.ToString(), "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                chart1.Series.SuspendUpdates();
                for (int ik = 0; ik < len; ik++)
                {
                    chart1.Series[ik].Points.DataBindXY(dt.DefaultView, tn, dt.DefaultView, content[dataStartingIndex].Split(',')[(ik + 1)]);
                }
                chart1.Series.ResumeUpdates();

                chart1.Annotations.Clear();

                RA = new TextAnnotation();
                RA.Alignment = System.Drawing.ContentAlignment.TopRight;
                RA.ForeColor = System.Drawing.Color.Black;
                RA.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
                chart1.Annotations.Add(RA);

                RA1 = new TextAnnotation();
                RA1.ForeColor = System.Drawing.Color.Black;
                RA1.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
                RA1.Alignment = System.Drawing.ContentAlignment.TopRight;
                chart1.Annotations.Add(RA1);

                RA2 = new TextAnnotation();
                RA2.ForeColor = System.Drawing.Color.Black;
                RA2.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
                RA2.Alignment = System.Drawing.ContentAlignment.TopRight;
                chart1.Annotations.Add(RA2);

                RA.Text = "Start: " + sos;
                RA1.Text = "End: " + eos;
                RA2.Text = "Duration: " + timespanB + " (hrs:min:sec)";
                Ra2Pos();
                Ra1Pos();
                RaPos();

                if (list != null)
                {
                    string newFileLocation = Path.Combine(FileLocator.mainLocation, DeviceID.ToString(), Path.GetFileNameWithoutExtension(file) + ".dat");
                    using (File.Create(Path.Combine(FileLocator.mainLocation, DeviceID.ToString(), file + ".dat"))) { }
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

        void RaPos()
        {
            if (RA == null) return;
            RA.X = 0.5;
            RA.Y = 0.5;
            RA.Width = 15;
            RA.Height = 4;
        }

        void Ra1Pos()
        {
            if (RA1 == null) return;
            RA1.X = 15;
            RA1.Y = 0.5;
            RA1.Width = 15;
            RA1.Height = 4;
        }

        void Ra2Pos()
        {
            if (RA2 == null) return;
            RA2.X = 30;
            RA2.Y = 0.5;
            RA2.Width = 15;
            RA2.Height = 4;
        }

        private void chart1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var result = chart1.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.LegendItem)
            {
                if (result.Series.Color == System.Drawing.Color.Transparent)
                {
                    result.Series.Color = System.Drawing.Color.Empty;
                }
                else
                {
                    result.Series.Color = System.Drawing.Color.Transparent;
                }
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void filterButton_Click(object sender, EventArgs e)
        {

            try
            {

                if (dt != null)
                {
                    var startIndex = from row in dt.AsEnumerable()
                                     where row.Field<string>(0) == filterStart.Value.ToString("hh:mm:ss tt dd-MM-yyyy")
                                     select dt.Rows.IndexOf(row);
                    var endIndex = from row in dt.AsEnumerable()
                                   where row.Field<string>(0) == filterEnd.Value.ToString("hh:mm:ss tt dd-MM-yyyy")
                                   select dt.Rows.IndexOf(row);

                    DisplayRowsInRange(startIndex.First(), endIndex.First());
                }
            }
            catch (Exception frog)
            {
                System.Windows.MessageBox.Show(frog.Message);
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

            chart1.Series.SuspendUpdates();
            for (int ik = 0; ik < filteredDataTable.Columns.Count - 1; ik++)
            {
                chart1.Series[ik].Points.DataBindXY(filteredDataTable.DefaultView, filteredDataTable.Columns[ik].ColumnName, filteredDataTable.DefaultView, content[0].Split(',')[(ik + 1)]);
            }
            chart1.Series.ResumeUpdates();

            chart1.Annotations.Clear();

            RA = new TextAnnotation();
            RA.Alignment = System.Drawing.ContentAlignment.TopRight;
            RA.ForeColor = System.Drawing.Color.Black;
            RA.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            chart1.Annotations.Add(RA);

            RA1 = new TextAnnotation();
            RA1.ForeColor = System.Drawing.Color.Black;
            RA1.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA1.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA1);

            RA2 = new TextAnnotation();
            RA2.ForeColor = System.Drawing.Color.Black;
            RA2.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA2.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA2);

            RA.Text = "Start: " + dt.Rows[startRow][0].ToString();
            RA1.Text = "End: " + dt.Rows[endRow][0].ToString();
            string timespanB = (DateTime.Parse(dt.Rows[endRow][0].ToString()) - DateTime.Parse(dt.Rows[startRow][0].ToString())).ToString();
            RA2.Text = "Duration: " + timespanB + " (hrs:min:sec)";
            Ra2Pos();
            Ra1Pos();
            RaPos();
        }

        private void clearFilter_Click(object sender, EventArgs e)
        {
            dataView.DataSource = dt;

            chart1.Series.SuspendUpdates();
            for (int ik = 0; ik < dt.Columns.Count - 1; ik++)
            {
                chart1.Series[ik].Points.DataBindXY(dt.DefaultView, dt.Columns[ik].ColumnName, dt.DefaultView, content[0].Split(',')[(ik + 1)]);
            }
            chart1.Series.ResumeUpdates();

            chart1.Annotations.Clear();

            RA = new TextAnnotation();
            RA.Alignment = System.Drawing.ContentAlignment.TopRight;
            RA.ForeColor = System.Drawing.Color.Black;
            RA.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            chart1.Annotations.Add(RA);

            RA1 = new TextAnnotation();
            RA1.ForeColor = System.Drawing.Color.Black;
            RA1.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA1.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA1);

            RA2 = new TextAnnotation();
            RA2.ForeColor = System.Drawing.Color.Black;
            RA2.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA2.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA2);

            RA.Text = "Start: " + sos;
            RA1.Text = "End: " + eos;
            string timespanB = (DateTime.Parse(eos) - DateTime.Parse(sos)).ToString();
            RA2.Text = "Duration: " + timespanB + " (hrs:min:sec)";
            Ra2Pos();
            Ra1Pos();
            RaPos();
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
                    if (Path.GetExtension(OFD.FileName).ToLower() == ".csv")
                    {
                        openf();
                    }
                    else
                    {
                        LoadExcel();
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
                chart1.Series.Clear();
            }

            helpToolStripMenuItem.Enabled = true;
            chart1.ChartAreas.SuspendUpdates();
            for (int i = 1; i < dt.Columns.Count; i++)
            {
                var series = new Series
                {
                    Name = dt.Columns[i].ColumnName,
                    IsVisibleInLegend = true,
                    ChartType = SeriesChartType.Spline,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 10,
                    MarkerStep = (int)(dt.Rows.Count - 1) / 10,
                    BorderWidth = 3,
                };

                // Add the series to the Chart control
                chart1.Series.Add(series);
            }

            chart1.Series.SuspendUpdates();
            for (int ik = 0; ik < dt.Columns.Count - 1; ik++)
            {
                chart1.Series[ik].Points.DataBindXY(dt.DefaultView, dt.Columns[0].ColumnName, dt.DefaultView, dt.Columns[ik + 1].ColumnName);
            }
            chart1.Series.ResumeUpdates();
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm dd-MM-yyyy";

            chart1.Annotations.Clear();

            sos = dt.Rows[0][0].ToString();
            eos = dt.Rows[dt.Rows.Count - 1][0].ToString();


            RA = new TextAnnotation();
            RA.Alignment = System.Drawing.ContentAlignment.TopRight;
            RA.ForeColor = System.Drawing.Color.Black;
            RA.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            chart1.Annotations.Add(RA);

            RA1 = new TextAnnotation();
            RA1.ForeColor = System.Drawing.Color.Black;
            RA1.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA1.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA1);

            RA2 = new TextAnnotation();
            RA2.ForeColor = System.Drawing.Color.Black;
            RA2.Font = new System.Drawing.Font(label1.Font.Name, 10, label1.Font.Style, label1.Font.Unit);
            RA2.Alignment = System.Drawing.ContentAlignment.TopRight;
            chart1.Annotations.Add(RA2);

            RA.Text = "Start: " + sos;
            RA1.Text = "End: " + eos;
            string timespanB = (DateTime.Parse(eos) - DateTime.Parse(sos)).ToString();
            RA2.Text = "Duration: " + timespanB + " (hrs:min:sec)";
            Ra2Pos();
            Ra1Pos();
            RaPos();

            chart1.ChartAreas.ResumeUpdates();
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
