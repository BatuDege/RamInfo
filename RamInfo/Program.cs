using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace RamInfo
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                currentProcess.PriorityClass = ProcessPriorityClass.High;
            }

            Application.Run(new RamForm());
        }
    }
}
