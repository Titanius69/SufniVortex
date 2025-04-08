using System;
using System.Drawing.Text;
using System.Windows.Forms;

namespace SufniVortex
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ComplexBrowserForm());
        }
    }
}
