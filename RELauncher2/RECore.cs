using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace RELauncher2
{
    public static class RECore
    {
        public static string version;
        public static bool needGuide = false;
        //public static Server.ServerInfo ServerCfg;
        public static bool IsServerDedicated;
        public static Dictionary<string, object> Language = new Dictionary<string, object>();
        public static Dictionary<string, ResourceDictionary> Color = new Dictionary<string, ResourceDictionary>();
        public static string BaseDirectory = Environment.CurrentDirectory + '\\';
        public static string DataDirectory = Environment.CurrentDirectory + '\\' + "MTMCL" + '\\';
        private readonly static string Cfgfile = BaseDirectory + "mtmcl_config.json";
        public static string DefaultBG = "pack://application:,,,/Resources/bg.png";
        //public static NotiIcon NIcon = new NotiIcon();
        public static MainWindow MainWindow = null;
        private static Application thisApplication = Application.Current;
        public static Dispatcher Dispatcher = Dispatcher.CurrentDispatcher;

        public static object Logger { get; private set; }
        public static object Config { get; internal set; }

        public static void Invoke(Delegate invoke, object[] argObjects = null)
        {
            Dispatcher.Invoke(invoke, argObjects);
        }
        private enum ColorScheme
        {
            Red, Green, Blue, Purple, Orange, Lime, Emerald, Teal, Cyan, Cobalt, Indigo, Violet, Pink, Magenta, Crimson, Amber, Yellow, Brown, Olive, Steel, Mauve, Taupe, Sienna
        }
        public static void Halt(int code = 0)
        {
            thisApplication.Shutdown(code);
        }

        public static void SingleInstance(Window window)
        {
            System.Threading.ThreadPool.RegisterWaitForSingleObject(App.ProgramStarted, OnAnotherProgramStarted, window, -1, false);
        }

        private static void OnAnotherProgramStarted(object state, bool timedout)
        {
            var window = state as Window;
            //NIcon.ShowBalloonTip(2000, LangManager.GetLangFromResource("MTMCLHiddenInfo"));
            if (window != null)
            {
                Dispatcher.Invoke(new Action(window.Show));
                Dispatcher.Invoke(new Action(() => { window.Activate(); }));
            }
        }
    }
}
