using System;
using System.Globalization;
using System.Windows.Forms;

namespace CSV_Graph
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                CultureInfo culture = new CultureInfo("en-IN");
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Splash splash = new Splash();
                splash.Show();
                Application.Run();
            }
            catch { }
        }
    }
}
