using AGE;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AGEMain
{
    public partial class Form : System.Windows.Forms.Form
    {
        public delegate void Entrust();

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {



        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            

        }

        private void CallBack()
        {
            Close();
        }

        private void UninstallService(object obj)
        {
            string serviceFilePath = Environment.CurrentDirectory + "\\AGEService.exe";
            string serviceName = "AGEService";
            if (AGEService.IsServiceExisted(serviceName)) AGEService.UninstallService(serviceFilePath);

            Entrust callback = obj as Entrust; //强转为委托
            callback();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            btnExit.Enabled = false;

            loadControl.Visible = true;

            Entrust callback = new Entrust(CallBack); //委托之后要执行的方法
            Thread th = new Thread(new ParameterizedThreadStart(UninstallService)); //线程要执行的方法
            th.IsBackground = true;
            th.Start(callback);//启动线程 委托给线程

         

        }


    }
}
