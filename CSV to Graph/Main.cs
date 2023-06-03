using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using LicenseActivation;
using Microsoft.Win32;

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
        int markerSSize = 10;

        TextAnnotation RA = new TextAnnotation();
        TextAnnotation RA1 = new TextAnnotation();
        TextAnnotation RA2 = new TextAnnotation();

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
                        MessageBox.Show(ex1.Message);
                        return null;
                    }
                }
            }
            catch (Exception ex1)
            {
                MessageBox.Show(ex1.Message);
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
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                file = OFD.FileName;
                try
                {
                    Thread thread = new Thread(openf);
                    thread.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public void displ(bool vis)
        {
            label1.Visible = vis;
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
                    chart1.SaveImage(SFD.FileName, ChartImageFormat.Png);
                    MessageBox.Show("Graph image exported");
                }
            }
            catch
            {
                MessageBox.Show("Couldn't perform the requested action!");
            }
        }

        private void pdfToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SFD.Filter = "PDF (*.pdf)|*.pdf";
                if (SFD.ShowDialog() == DialogResult.OK)
                {
                    var chartimg = new MemoryStream();
                    chart1.SaveImage(chartimg, ChartImageFormat.Png);
                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
                    PdfWriter.GetInstance(pdfDoc, new FileStream(SFD.FileName, FileMode.Create));
                    pdfDoc.Open();
                    Image Chim = Image.GetInstance(chartimg.GetBuffer());
                    Chim.ScaleAbsoluteHeight(575);
                    Chim.ScaleAbsoluteWidth(750);
                    pdfDoc.Add(Chim);
                    pdfDoc.Close();
                    MessageBox.Show("Graph PDF exported");
                }
            }
            catch
            {
                MessageBox.Show("Couldn't perform the requested action!");
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
                    MessageBox.Show("Graph Printed");
                }
                printDocument1.Dispose();
            }
            catch
            {
                MessageBox.Show("Couldn't perform the requested action!");
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

            Invoke((MethodInvoker)delegate
            {
                displ(true);
            });

            if (!String.IsNullOrWhiteSpace(t))
            {
                var cht = new Title();
                cht.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
                cht.Text = t;
                cht.Name = "Title";
                Invoke((MethodInvoker)delegate
                {
                    chart1.Titles.Add(cht);
                });
            }

            if (!String.IsNullOrWhiteSpace(f))
            {
                var cht = new Title();
                cht.Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);
                cht.Text = f;
                cht.Name = "Footer";
                cht.Docking = Docking.Bottom;
                Invoke((MethodInvoker)delegate
                {
                    chart1.Titles.Add(cht);
                });
            }

            if (!String.IsNullOrWhiteSpace(x))
            {
                Invoke((MethodInvoker)delegate
                {
                    chart1.ChartAreas[0].AxisX.Title = x;
                });
            }

            if (!String.IsNullOrWhiteSpace(y))
            {
                Invoke((MethodInvoker)delegate
                {
                    chart1.ChartAreas[0].AxisY.Title = y;
                });
            }

            if (mvis)
            {
                Invoke((MethodInvoker)delegate
                {
                    foreach (var s in chart1.Series)
                    {
                        s.MarkerSize = Convert.ToInt32(m);
                    }
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    foreach (var s in chart1.Series)
                    {
                        s.MarkerStyle = MarkerStyle.None;
                    }
                });
            }

            if (true)
            {
                Invoke((MethodInvoker)delegate
                {
                    foreach (var s in chart1.Series)
                    {
                        s.MarkerStep = Convert.ToInt32(msp);
                    }
                });
            }

            if (!String.IsNullOrWhiteSpace(ymax) && !String.IsNullOrWhiteSpace(ymin))
            {
                if (int.Parse(ymax) > int.Parse(ymin))
                {
                    Invoke((MethodInvoker)delegate
                    {
                        chart1.ChartAreas[0].AxisY.Maximum = int.Parse(ymax);
                        chart1.ChartAreas[0].AxisY.Minimum = int.Parse(ymin);
                    });
                }
                else
                {
                    MessageBox.Show("Maximum value less than minimum! Could not perform this action");
                    Invoke((MethodInvoker)delegate
                    {
                        chart1.ChartAreas[0].AxisY.Maximum = Double.NaN; // sets the Maximum to NaN
                        chart1.ChartAreas[0].AxisY.Minimum = Double.NaN; // sets the Minimum to NaN
                        chart1.ChartAreas[0].RecalculateAxesScale();
                    });
                }
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    chart1.ChartAreas[0].AxisY.Maximum = Double.NaN; // sets the Maximum to NaN
                    chart1.ChartAreas[0].AxisY.Minimum = Double.NaN; // sets the Minimum to NaN
                    chart1.ChartAreas[0].RecalculateAxesScale();
                });
            }
            Invoke((MethodInvoker)delegate
            {
                displ(false);
            });
        }

        public void change(string t, string f, string x, string y, decimal m, decimal msp, bool mvis, string ymax, string ymin)
        {
            try
            {

                Thread thread = new Thread(() => temp(t, f, x, y, m, msp, mvis, ymax, ymin));
                thread.Start();
            }
            catch
            {
                MessageBox.Show("There was some error performing the action");
            }
        }

        public void openf()
        {
            try
            {
                Invoke((MethodInvoker)delegate
                {
                    displ(true);
                    if (helpToolStripMenuItem.Enabled == true)
                    {
                        chart1.Series.Clear();
                    }

                    helpToolStripMenuItem.Enabled = true;
                });

                string[] content = File.ReadAllLines(file);

                string[] headers = content[0].Split(',');

                string tn = headers[0];

                int len = headers.Length - 1;

                DataTable dt = new DataTable();

                for (int i = 0; i < headers.Length; i++)
                {
                    dt.Columns.Add(headers[i].ToLower(), typeof(string));
                    if (i > 0)
                    {
                        var series = new Series
                        {
                            Name = headers[i],
                            IsVisibleInLegend = true,
                            ChartType = SeriesChartType.Line,
                            MarkerStyle = MarkerStyle.Circle,
                            MarkerSize = 10,
                            MarkerStep = 200,
                            BorderWidth = 3
                        };

                        Invoke((MethodInvoker)delegate
                        {
                            chart1.Series.Add(series);
                        });
                    }
                }

                DataRow Row;
                for (int i = 1; i < content.Length; i++)
                {
                    headers = content[i].Split(',');
                    Row = dt.NewRow();
                    for (int f = 0; f < headers.Length; f++)
                    {
                        Row[f] = headers[f];
                    }
                    dt.Rows.Add(Row);
                }

                sos = content[1].Split(',')[0];
                eos = content[content.Length - 1].Split(',')[0];

                Invoke((MethodInvoker)delegate
                {
                    chart1.Series.SuspendUpdates();
                    for (int ik = 0; ik < len; ik++)
                    {
                        chart1.Series[ik].Points.DataBindXY(dt.DefaultView, tn, dt.DefaultView, content[0].Split(',')[(ik + 1)]);
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

                    displ(false);
                });
            }
            catch
            {
                MessageBox.Show("The file is being used by another application. Close and try again.");
                Invoke((MethodInvoker)delegate
                {
                    displ(false);
                });
            }
        }

        void RaPos()
        {
            if (RA == null) return;
            ElementPosition LP = chart1.Legends[0].Position;
            RA.X = 0.5;
            RA.Y = 0.5;
            RA.Width = 15;
            RA.Height = 4;
        }

        void Ra1Pos()
        {
            if (RA1 == null) return;
            ElementPosition LP = chart1.Legends[0].Position;
            RA1.X = 15;
            RA1.Y = 0.5;
            RA1.Width = 15;
            RA1.Height = 4;
        }

        void Ra2Pos()
        {
            if (RA2 == null) return;
            ElementPosition LP = chart1.Legends[0].Position;
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
    }
}
