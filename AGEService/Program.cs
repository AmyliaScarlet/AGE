using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WcfService;

namespace AGEService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            Process[] vProcesses = Process.GetProcesses();
            foreach (Process vProcess in vProcesses)
            {
                if (vProcess.ProcessName.Equals("age", StringComparison.OrdinalIgnoreCase))
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                        new AGEService()
                    };
                    ServiceBase.Run(ServicesToRun);

                    //服务的地址
                    //Uri baseAddress = new Uri("http://localhost:8078/GettingStarted/");
                    //承载服务的宿主
                    //ServiceHost selfHost = new ServiceHost(typeof(AGEWcfService), baseAddress);
                }
            }
            
        }
    }
}
