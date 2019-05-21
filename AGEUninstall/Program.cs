using System;
using System.Windows.Forms;

namespace AGEUninstall
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            String sSysRoot = System.Environment.SystemDirectory;

            if (MessageBox.Show("是否卸载?", "AGE卸载", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                System.Diagnostics.Process.Start(sSysRoot + "\\msiexec.exe ", "/x {4AEBF657-FB8A-4398-B353-CB7C7FE0CA2A} /qr");
            }
           

        }
    }
}
