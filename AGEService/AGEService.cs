using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using WcfService;

namespace AGEService
{
    public partial class AGEService : ServiceBase
    {
        string filePath = "F:\\AGELog\\AGEService.log";
        public ServiceHost serviceHost = null;

        public AGEService()
        {
            InitializeComponent();
        }
        private void Log(String str)
        {

            if (!File.Exists(filePath)) File.Create(filePath);

            StreamWriter srW = File.AppendText(filePath);
            srW.WriteLine("时间:" + DateTime.Now.ToString());
            srW.WriteLine(str);
            srW.WriteLine("");
            srW.Close();
        }

        protected override void OnStart(string[] args)
        {
            CanShutdown = true;
            AutoLog = true;

            //if (serviceHost != null)
            //{
            //    serviceHost.Close();
            //}
            //serviceHost = new ServiceHost(typeof(AGEWcfService));

            //serviceHost.Open();

            Log("信息:服务启动！");
        }
        

        protected override void OnShutdown()
        {
            base.OnShutdown();

        }
        protected override void OnCustomCommand(int command)
        {
            //var myBinding = new WSHttpBinding();
            //EndpointAddress myEndpoint = new EndpointAddress("http://localhost:8078/GettingStarted/");
            ////EndpointAddress myEndpoint = new EndpointAddress("http://localhost:8078/"); 
            //ChannelFactory<IAGEWcfService> myChannelFactory = new ChannelFactory<IAGEWcfService>(myBinding, myEndpoint);
            //var client = myChannelFactory.CreateChannel();
            //client.Log("command="+ command);
            base.OnCustomCommand(command);


        }
        protected override void OnStop()
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }

            Log("信息:服务停止！");
        }

    }


}
