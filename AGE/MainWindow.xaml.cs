using AGELibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AGE
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly double dDoubleAnimationDuration = 0.5; //load动画一轮耗时 s
        private readonly int nWaitBackgroundRunTime = 2000; //等待后台服务启动的时间 m
        private readonly int[] nRandomSeedScope = new int[] { 200, 800 }; //随机数产生范围

        

        System.Timers.Timer aTimer;

        private static SynchronizationContext s_SC = SynchronizationContext.Current; //主窗口类的静态成员

        private static List<string> listLoad;

        private static int index = 0;

        public MainWindow()
        {
            InitializeComponent();


            ChkProcess();

            RunAnima();

            Init();
        }

        /// <summary>
        /// 开始loading动画
        /// </summary>
        private void RunAnima()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 360;
            da.Duration = new Duration(TimeSpan.FromSeconds(dDoubleAnimationDuration));
            da.RepeatBehavior = RepeatBehavior.Forever;
            RotateTransform rt = new RotateTransform();
            rt.CenterX = 10;
            rt.CenterY = 10;
            iLoad.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        }

        /// <summary>
        /// 检测程序重复开启
        /// </summary>
        private void ChkProcess()
        {
            Process[] vProcesses = Process.GetProcesses();
            foreach (Process vProcess in vProcesses)
            {
                if (vProcess.ProcessName.Equals("agemain", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("AGE已经启动,请勿重复启动！");

                    Environment.Exit(0);
                }
            }
        }

        private void killMain()
        {
            Process[] p = Process.GetProcessesByName("agemain");
            foreach (Process ps in p)
            {
                ps.Kill();
            }
        }


        /// <summary>
        /// 启动主程序
        /// </summary>
        private void StartAGEMain()
        {

            if (File.Exists(Environment.CurrentDirectory + "\\AGEMain.exe"))
            {
                Process.Start(Environment.CurrentDirectory + "\\AGEMain.exe", "open"); //
            }
            else
            {
                MessageBox.Show("主程序已损坏！");
                Environment.Exit(0);
            }

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = nWaitBackgroundRunTime;
            timer.Elapsed += delegate
            {
                timer.Stop();

                //启动后台服务
                OpenBackGroundService();

                Environment.Exit(0);
            };
            timer.Start();
        }

        private void OpenBackGroundService()
        {
            string serviceFilePath = Environment.CurrentDirectory + "\\AGEService.exe";
            string serviceName = "AGEService";

            //安装服务
            if (AGEService.IsServiceExisted(serviceName)) AGEService.UninstallService(serviceFilePath);
            AGEService.InstallService(serviceFilePath);

            //启动服务
            if (AGEService.IsServiceExisted(serviceName)) AGEService.ServiceStart(serviceName);

        }

        /// <summary>
        /// 初始化loading
        /// </summary>
        private void Init()
        {
            lCText.Content = "© 2016-"+DateTime.Now.Year+" AmyliaScarlet All rights received";

            listLoad = new List<string>()
            {
                "正在初始化引擎...",
                "正在加载框架...",
                "正在启动服务...",
                "正在加载功能模块...",
                "正在初始化UI...",
                "正在准备...",
                "欢迎使用！"
            };
            LoadText();

        }



        private void LoadText()
        {
            SetText(listLoad[index]);
            index++;

            //实例化Timer类，设置间隔时间为10000毫秒； 
            aTimer = new System.Timers.Timer();
            //注册计时器的事件
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            //设置时间间隔，覆盖构造函数设置的间隔
            aTimer.Interval = new Random().Next(nRandomSeedScope[0], nRandomSeedScope[1]);
            //设置是执行一次（false）还是一直执行(true)，默认为true
            aTimer.AutoReset = true;
            //开始计时
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            SetText(listLoad[index]);
            if (index == listLoad.Count - 1)
            {
                aTimer.Stop();
                StartAGEMain();
            }
            else
            {
                aTimer.Interval = new Random().Next(nRandomSeedScope[0], nRandomSeedScope[1]);
                index++;
            }
        }


        /// <summary>
        /// 这个函数用于设置UI界面上的某个元素
        /// </summary>
        /// <param name="strText"></param>
        private void SetText(string strText)
        {
            if (!App.IsRunInMainThread)
            {
                s_SC.Post(oo => { SetText(strText); }, null); //可以使用Post也可以使用Send
                return;
            }
            lText.Content = strText;
        }

        /// <summary>
        /// 这个函数用于从UI界面的元素获取内容
        /// </summary>
        /// <returns></returns>
        private string GetText()
        {
            if (!App.IsRunInMainThread)
            {
                string str = null;
                s_SC.Send(oo => { str = GetText(); }, null);   //必须要使用Send
                return str;
            }
            return lText.Content.ToString();
        }

    }
}
