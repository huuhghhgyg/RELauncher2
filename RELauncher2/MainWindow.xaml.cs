using System;
using System.Linq;
using System.Windows;
using KMCCC.Launcher;
using MahApps.Metro.Controls;
using KMCCC.Authentication;
using System.Diagnostics;
using System.Configuration;
using MahApps.Metro.Controls.Dialogs;
using System.Timers;
using System.Threading;
using System.Net;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Versions.RawVersionListType;
using System.Threading.Tasks;
using System.Data;
using RELauncher2.Versions;

namespace RELauncher2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public Task TaskEx { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            var versions = App.Core.GetVersions().ToArray();
            comboBox1.ItemsSource = versions;//绑定数据源
            comboBox1.DisplayMemberPath = "Id";//设置comboBox显示的为版本Id
            Load();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Launch()
        {
            bool twitchEn = (bool)twitch.IsChecked;
            LaunchMode mcMode;
            var ver = (KMCCC.Launcher.Version)comboBox1.SelectedItem;
            LaunchResult resultCheck;
            if (mcLauncher.IsChecked == true)
            {
                mcMode = LaunchMode.MCLauncher;
            }
            else
            {
                mcMode = LaunchMode.BmclMode;
            }
            if (onlineMode.IsChecked == true)//在线
            {
                if (autoConnect.IsChecked == true)//在线+自动
                {
                    var result = App.Core.Launch(new LaunchOptions
                    {
                        Version = ver, //Ver为Versions里你要启动的版本名字
                        MaxMemory = (int)slider.Value, //最大内存，int类型
                        Authenticator = new YggdrasilLogin(usrNameText.Text, passwdText.Password, twitchEn), // 正版启动，最后一个为是否twitch登录
                        Mode = mcMode, //启动模式，这个我会在后面解释有哪几种
                        Server = new ServerInfo { Address = ipBox.Text, Port = Convert.ToUInt16(portBox.Text) }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                        Size = new WindowSize { Height = Convert.ToUInt16(winHeight.Text), Width = Convert.ToUInt16(winWidth.Text) } //设置窗口大小，可以不要
                    });
                    resultCheck = result;
                }
                else//在线
                {
                    var result = App.Core.Launch(new LaunchOptions
                    {
                        Version = ver, //Ver为Versions里你要启动的版本名字
                        MaxMemory = (int)slider.Value, //最大内存，int类型
                        Authenticator = new YggdrasilLogin(usrNameText.Text, passwdText.Password, twitchEn), // 正版启动，最后一个为是否twitch登录
                        Mode = mcMode, //启动模式，这个我会在后面解释有哪几种
                                       //Server = new ServerInfo { Address = "服务器IP地址", Port = "服务器端口" }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                        Size = new WindowSize { Height = Convert.ToUInt16(winHeight.Text), Width = Convert.ToUInt16(winWidth.Text) } //设置窗口大小，可以不要
                    });
                    resultCheck = result;
                }
            }
            else//离线
            {
                if (autoConnect.IsChecked == true)//离线+自动
                {
                    var result = App.Core.Launch(new LaunchOptions
                    {
                        Version = ver, //Ver为Versions里你要启动的版本名字
                        MaxMemory = (int)slider.Value, //最大内存，int类型
                        Authenticator = new OfflineAuthenticator(usrNameText.Text), //离线启动，ZhaiSoul那儿为你要设置的游戏名
                                                                                    //Authenticator = new YggdrasilLogin("邮箱", "密码", true), // 正版启动，最后一个为是否twitch登录
                        Mode = mcMode, //启动模式，这个我会在后面解释有哪几种
                                       //Server = new ServerInfo { Address = "服务器IP地址", Port = "服务器端口" }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                        Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
                    });
                    resultCheck = result;
                }
                else//离线
                {
                    var result = App.Core.Launch(new LaunchOptions
                    {
                        Version = ver, //Ver为Versions里你要启动的版本名字
                        MaxMemory = (int)slider.Value, //最大内存，int类型
                        Authenticator = new OfflineAuthenticator(usrNameText.Text), //离线启动，ZhaiSoul那儿为你要设置的游戏名
                                                                                    //Authenticator = new YggdrasilLogin("邮箱", "密码", true), // 正版启动，最后一个为是否twitch登录
                        Mode = mcMode, //启动模式，这个我会在后面解释有哪几种
                                       //Server = new ServerInfo { Address = "服务器IP地址", Port = "服务器端口" }, //设置启动游戏后，自动加入指定IP的服务器，可以不要
                        Size = new WindowSize { Height = 768, Width = 1280 } //设置窗口大小，可以不要
                    });
                    resultCheck = result;
                }
            }

            if (!resultCheck.Success)
            {
                //MessageBox.Show(result.ErrorMessage, result.ErrorType.ToString());
                switch (resultCheck.ErrorType)
                {
                    case ErrorType.NoJAVA:
                        this.ShowMessageAsync("错误", "你系统的Java有异常，可能你非正常途径删除过Java，请尝试重新安装Java\n详细信息：" + resultCheck.ErrorMessage,  MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                        break;
                    case ErrorType.AuthenticationFailed:
                        this.ShowMessageAsync("错误", "正版验证失败！请检查你的账号密码。账号错误\n详细信息："+ resultCheck.ErrorMessage, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                        break;
                    case ErrorType.UncompressingFailed:
                        this.ShowMessageAsync("可能的多开或文件损坏", "可能的多开或文件损坏，请确认文件完整且不要多开\n如果你不是多开游戏的话，请检查libraries文件夹是否完整\n详细信息：" + resultCheck.ErrorMessage,  MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                        break;
                    default:
                        this.ShowMessageAsync("启动错误，请将此窗口截图向开发者寻求帮助", resultCheck.ErrorMessage + "\n" +
                        (resultCheck.Exception == null ? string.Empty : resultCheck.Exception.StackTrace), MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
                        break;
                }
            }
            else
            {
                Close();
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            setMemory();
        }

        private void setMemory()
        {
            double usedMemory = 0;
            usedMemory = Math.Round(Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0 * 100);
            slider.Maximum = usedMemory;
            if (memAuto.IsChecked == true)
            {
                slider.Value = Math.Round(usedMemory * 0.8);
            }
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = formName.Text;
            
        }

        private void javaAuto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                javaText.Text = KMCCC.Tools.SystemTools.FindJava().Last();//textbox1显示我们找到的最后一个Java（也是最近安装的一个）
            }
            catch
            {
            }
        }

        private void autoConnect_Click(object sender, RoutedEventArgs e)
        {
            if (autoConnect.IsChecked == true)
            {
                ipBox.IsEnabled = true;
            }
            else
            {
                ipBox.IsEnabled = false;
            }
        }

        private void onlineMode_Click(object sender, RoutedEventArgs e)
        {
            if (onlineMode.IsChecked == true)
            {
                passwdText.IsEnabled = true;
            }
            else
            {
                passwdText.IsEnabled = false;
            }
        }

        private void LaunchBtn_Click(object sender, RoutedEventArgs e)
        {
            Launch();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            saveSettings();
        }
        
        private void saveSettings()
        {
            string launchMode;
            if (mcLauncher.IsChecked == true)
            {
                launchMode = "mcLauncher";
            }
            else
            {
                launchMode = "bmcl";
            }

            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["usrname"].Value = usrNameText.Text;
            cfa.AppSettings.Settings["javaPath"].Value = javaText.Text;
            cfa.AppSettings.Settings["memory"].Value = slider.Value.ToString();
            cfa.AppSettings.Settings["GameHeight"].Value = winHeight.Text;
            cfa.AppSettings.Settings["GameWidth"].Value = winWidth.Text;
            cfa.AppSettings.Settings["LaunchMode"].Value = launchMode;
            cfa.AppSettings.Settings["passwd"].Value = passwdText.Password;
            cfa.AppSettings.Settings["online"].Value = onlineMode.IsChecked.ToString();
            cfa.AppSettings.Settings["twitch"].Value = twitch.IsChecked.ToString();
            cfa.AppSettings.Settings["autoconnect"].Value = autoConnect.IsChecked.ToString();
            cfa.AppSettings.Settings["ip-"].Value = ipBox.Text;
            cfa.Save();
        }

        private void Load()//加载
        {
            string launchMode;
            usrNameText.Text = ConfigurationManager.AppSettings["usrname"];
            javaText.Text = ConfigurationManager.AppSettings["javaPath"];
            slider.Value= int.Parse(ConfigurationManager.AppSettings["memory"]);
            winHeight.Text = ConfigurationManager.AppSettings["GameHeight"];
            winWidth.Text = ConfigurationManager.AppSettings["GameWidth"];
            launchMode = ConfigurationManager.AppSettings["LaunchMode"];
            passwdText.Password = ConfigurationManager.AppSettings["passwd"];
            onlineMode.IsChecked = bool.Parse(ConfigurationManager.AppSettings["online"]);
            twitch.IsChecked = bool.Parse(ConfigurationManager.AppSettings["twitch"]);
            autoConnect.IsChecked = bool.Parse(ConfigurationManager.AppSettings["autoconnect"]);
            ipBox.Text = ConfigurationManager.AppSettings["ip"];
            portBox.Text = ConfigurationManager.AppSettings["port"];
            formName.Text= ConfigurationManager.AppSettings["title"];


            if (launchMode == "mcLauncher")
            {
                mcLauncher.IsChecked = true;
            }
            else
            {
                bmcl.IsChecked = true;
            }

            if (onlineMode.IsChecked!=true)
            {
                passwdText.IsEnabled = false;
            }
        }

        private void butReloadMC_Click(object sender, RoutedEventArgs e)
        {
            reloadMCVersion();
        }

        private void reloadMCVersion()
        {
            butReloadMC.IsEnabled = false;
            loadErrorGrid.Visibility = Visibility.Collapsed;
            progGrid.Visibility = Visibility.Visible;
            versionListView.DataContext = null;
            var rawJson = new DataContractJsonSerializer(typeof(RawVersionListType));//
            var getJson = (HttpWebRequest)WebRequest.Create(UrlReplacer.getVersionsUrl());//
            getJson.Timeout = 10000;
            getJson.ReadWriteTimeout = 10000;
            getJson.UserAgent = "MTMCL" + RECore.version;
            var thGet = new Thread(new ThreadStart(delegate
            {
                try
                {
                    Dispatcher.Invoke(new Action(async delegate
                    {
                        butReloadMC.Content = "RemoteVerGetting";//getting
                            await Task.Delay(TimeSpan.FromSeconds(1));
                    }));
                    var getJsonAns = (HttpWebResponse)getJson.GetResponse();
                    // ReSharper disable once AssignNullToNotNullAttribute
                    var remoteVersion = rawJson.ReadObject(getJsonAns.GetResponseStream()) as RawVersionListType;
                    var dt = new DataTable();
                    dt.Columns.Add("Ver");
                    dt.Columns.Add("RelTime", typeof(DateTime));
                    dt.Columns.Add("Type");
                    dt.Columns.Add("Url");
                    if (remoteVersion != null)
                        foreach (RemoteVerType rv in remoteVersion.getVersions())
                        {
                            dt.Rows.Add(new object[] { rv.id, DateTime.Parse(rv.releaseTime), rv.type, rv.url });
                        }
                    Dispatcher.Invoke(new Action(delegate
                    {
                        progGrid.Visibility = Visibility.Collapsed;//膜
                        butReloadMC.Content = "Reload";
                        butReloadMC.IsEnabled = true;
                        versionListView.DataContext = dt;
                        versionListView.Items.SortDescriptions.Add(new SortDescription("RelTime", ListSortDirection.Descending));
                    }));
                }
                catch (WebException ex)//easywrite
                {
                    Dispatcher.Invoke(new Action(delegate
                    {
                        progGrid.Visibility = Visibility.Collapsed;
                        loadErrorGrid.Visibility = Visibility.Visible;
                        butReloadMC.Content = "Reload";
                        butReloadMC.IsEnabled = true;
                    }));
                }
                catch (TimeoutException ex)//easywrite
                {
                    Dispatcher.Invoke(new Action(delegate
                    {
                        progGrid.Visibility = Visibility.Collapsed;
                        loadErrorGrid.Visibility = Visibility.Visible;
                        butReloadMC.Content = "Reload";
                        butReloadMC.IsEnabled = true;
                    }));
                }
                catch (Exception ex)//easywrite
                {
                    Dispatcher.Invoke(new Action(delegate
                    {
                        progGrid.Visibility = Visibility.Collapsed;
                        loadErrorGrid.Visibility = Visibility.Visible;
                        butReloadMC.Content = "Reload";
                        butReloadMC.IsEnabled = true;
                    }));
                }
            }));
            thGet.Start();
        }
    }
}
