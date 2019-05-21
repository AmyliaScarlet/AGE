using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace AGEService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private ServiceProcessInstaller process;
        private ServiceInstaller service;
        public ProjectInstaller()
        {
            InitializeComponent();
            //process = new ServiceProcessInstaller();
            //process.Account = ServiceAccount.LocalSystem;
            //service = new ServiceInstaller();
            //service.ServiceName = "AGEWcfService";
            //Installers.Add(process);
            //Installers.Add(service);
        }
    
        private void serviceInstaller2_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
