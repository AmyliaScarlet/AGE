using System.Threading;
using System.Windows;

namespace AGE
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        static volatile int s_nMainThreadID = Thread.CurrentThread.ManagedThreadId;


        //这个属性表示当前执行线程是否在主线程中运行
        public static bool IsRunInMainThread { get { return Thread.CurrentThread.ManagedThreadId == s_nMainThreadID; } }
    }
}
