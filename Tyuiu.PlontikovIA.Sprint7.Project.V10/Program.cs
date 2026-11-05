using System;
using System.Windows.Forms;
using Tyuiu.PlontikovPI.Sprint7.Project.V10;

namespace Tyuiu.PlontikovPI.Sprint7.Project.V10
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm_PI());
        }
    }
}