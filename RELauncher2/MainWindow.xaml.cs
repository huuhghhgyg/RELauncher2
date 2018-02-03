using System;
using System.Linq;
using System.Windows;
using KMCCC.Launcher;
using MahApps.Metro.Controls;
using KMCCC.Authentication;
using System.Diagnostics;
using System.Configuration;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using System.Windows.Media.Imaging;
using System.Reflection;
using GetBingWallpaper;
using System.Threading.Tasks;
using System.Threading;
using System.Net;

namespace RELauncher2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadingGrid.Visibility = Visibility.Visible;
        }

        public event ResolveEventHandler AssemblyResolve;

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //获取加载失败的程序集的全名
            var assName = new AssemblyName(args.Name).FullName;
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KMCCC.Pro.dll"))
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    return Assembly.Load(bytes);//加载资源文件中的dll,代替加载失败的程序集
                }
             throw new DllNotFoundException(assName);
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
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
            this.ShowMessageAsync("内存已被设置", "自动设置已经生效，最大内存已被设置为"+slider.Value, MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
        }

        private void setMemory()
        {
            double usedMemory = 0;
            usedMemory = Math.Round(Process.GetCurrentProcess().WorkingSet64 / 1024.0 / 1024.0 * 100);
            slider.Maximum = usedMemory;
            if (memAuto.IsChecked == true)
            {
                slider.Value = Math.Round(usedMemory * 0.6);
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
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["setSelect"].Value = comboBox1.SelectedIndex.ToString();
            cfa.Save();
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
            cfa.AppSettings.Settings["memMax"].Value = slider.Maximum.ToString();
            cfa.AppSettings.Settings["GameHeight"].Value = winHeight.Text;
            cfa.AppSettings.Settings["GameWidth"].Value = winWidth.Text;
            cfa.AppSettings.Settings["LaunchMode"].Value = launchMode;
            cfa.AppSettings.Settings["passwd"].Value = passwdText.Password;
            cfa.AppSettings.Settings["online"].Value = onlineMode.IsChecked.ToString();
            cfa.AppSettings.Settings["twitch"].Value = twitch.IsChecked.ToString();
            cfa.AppSettings.Settings["autoconnect"].Value = autoConnect.IsChecked.ToString();
            cfa.AppSettings.Settings["ip"].Value = ipBox.Text;
            cfa.AppSettings.Settings["port"].Value = portBox.Text;
            cfa.AppSettings.Settings["BingDaily"].Value = BingDaily.IsChecked.ToString();
            cfa.Save();
        }

        private void Load()//加载
        {
            if (!File.Exists(@"./RELauncher2.exe.Config"))
            {

            }
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
            slider.Maximum = int.Parse(ConfigurationManager.AppSettings["memMax"]);
            comboBox1.SelectedIndex= int.Parse(ConfigurationManager.AppSettings["setSelect"]);
            BingDaily.IsChecked = bool.Parse(ConfigurationManager.AppSettings["BingDaily"]);

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

            string url;
            if (BingDaily.IsChecked == true)
            {
                GetWallPaper WallPaperGetter = new GetWallPaper();
                WallPaperGetter.GetWallPaperUrl();
                url = "https://" + WallPaperGetter.WallPaperUrl;
                WallPaperGetter.DownloadFile();
                //Clipboard.SetDataObject(UrlBlock.Text);
                GetPictureFromURL(url, mainImage);
            }

            if (File.Exists(@"./bgi.jpg"))
            {
                string startPath = AppDomain.CurrentDomain.BaseDirectory;
                bgBox.Source = new BitmapImage(new Uri(startPath + "bgi.jpg"));
                mainImage.Source = new BitmapImage(new Uri(startPath + "bgi.jpg"));
            }
        }

        private async void GetPictureFromURL(string URL, System.Windows.Controls.Image image)
        {
            await Task.Run(() => Thread.Sleep(0));
            var request = WebRequest.Create(URL);
            int ErrorNum = 0, AllErrorNum = 0;

            RETRY:
            try
            {
                using (var response = await request.GetResponseAsync())
                using (var stream = response.GetResponseStream())
                {

                    //var imgBrush = new ImageBrush();
                    //var bitmap = new BitmapImage();
                    //bitmap.BeginInit();//开始设置属性
                    //bitmap.StreamSource = stream;
                    //bitmap.EndInit();//终止设置属性
                    //imgBrush.ImageSource = bitmap;
                    //grid.Background = imgBrush;

                    //image = new Image();

                    var fullFilePath = @URL;

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
                    bitmap.EndInit();

                    image.Source = bitmap;
                }
            }
            catch (System.IO.IOException)
            {
                if (AllErrorNum <= 3)//错误次数小于三次
                {
                    ErrorNum++;//记录
                    await Task.Delay(100);//停止100ms
                    goto RETRY;//重试
                }
            }
            await Task.Delay(800);
            LoadingGrid.Visibility = Visibility.Hidden;
        }

        private void butReloadMC_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            saveSettings();
        }

        private void reSet_Click(object sender, RoutedEventArgs e)
        {
            usrNameText.Text = "";
            javaText.Text = "";
            slider.Value = 1024;slider.Maximum = 4096;
            winHeight.Text = "720";
            winWidth.Text = "1280";
            mcLauncher.IsChecked = true;bmcl.IsChecked = false;
            passwdText.Password = "";
            onlineMode.IsChecked = false;twitch.IsChecked = false;
            autoConnect.IsChecked = false;
            ipBox.Text = "";portBox.Text = "";
            formName.Text = "RELauncher2";
            comboBox1.SelectedIndex = 0;
            BingDaily.IsChecked = true;
            saveSettings();
            this.ShowMessageAsync("重设完成", "所有内容均被重新设置，请重新打开启动器", MessageDialogStyle.Affirmative, new MetroDialogSettings() { AffirmativeButtonText = "确定" });
        }
    }
}
