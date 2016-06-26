using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RELauncher2
{
    public enum EnumGraphic
    {
        Fast = 0,
        Fancy = 1,
        BakaXL = 2,
        EffectiveFirst = 0,
        ExperienceFirst = 1,
        Duang = 2
    }
    [DataContract]
    public class Config : ICloneable
    {
        [DataMember]
        public EnumGraphic graphic { get; set; }
        [DataMember]
        public bool requiredGuide { get; set; }
        [DataMember]
        [JsonProperty("javapath")]
        public string Javaw { get; set; }
        [DataMember]
        [JsonProperty("minecraftpath")]
        public string MCPath { get; set; }
        [DataMember]
        [JsonProperty("javaXmx")]
        public double Javaxmx { get; set; }
        [DataMember]
        [JsonProperty("lastversion")]
        public string LastPlayVer { get; set; }
        [JsonProperty("lastlaunchmode")]
        public string LastLaunchMode { get; set; }
        [DataMember]
        [JsonProperty("jvmarg")]
        public string ExtraJvmArg { get; set; }
        [DataMember]
        [JsonProperty("language")]
        public string Lang { get; set; }
        [DataMember]
        [JsonProperty("background")]
        public string Background { get; set; }
        [DataMember]
        [JsonProperty("color-scheme")]
        public string ColorScheme { get; set; }
        [DataMember]
        [JsonProperty("download-source")]
        public static int DownloadSource { get; set; }
        [DataMember]
        public string GUID { get; set; }
        [DataMember]
        [JsonProperty("expand-task-gui")]
        public bool ExpandTaskGui { get; set; }
        [DataMember]
        [JsonProperty("check-update")]
        public bool CheckUpdate { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public string password { get; private set; }
        [DataMember]
        public string token;
        [DataMember]
        [JsonProperty("update-source")]
        public byte UpdateSource { get; set; }
        [DataMember]
        [JsonProperty("search-latest-update")]
        public bool SearchLatest { get; set; }
        [DataMember]
        [JsonProperty("server-info")]
        public ServerInfo Server { get; set; }
        [DataMember]
        [JsonProperty("saved-auths")]
        public Dictionary<string, SavedAuth> SavedAuths { get; set; }
        [DataMember]
        [JsonProperty("default-auth")]
        public string DefaultAuth { get; set; }
        [JsonProperty("reverse-color")]
        public bool reverseColor { get; set; }
        [JsonProperty("fail-update-last-time")]
        public bool failUpdateLastTime { get; set; }
        [DataContract]
        public class ServerInfo
        {
            [JsonProperty("title")]
            public string Title;
            [DataMember(Name = "server-name")]
            [JsonProperty("server-name")]
            public string ServerName;
            [DataMember(Name = "server-ip")]
            [JsonProperty("server-ip")]
            public string ServerIP;
            [DataMember(Name = "client-name")]
            [JsonProperty("client-name")]
            public string ClientPath;
            [DataMember(Name = "need-server-pack")]
            [JsonProperty("need-server-pack")]
            public bool NeedServerPack;
            [DataMember(Name = "server-pack-url", IsRequired = false)]
            [JsonProperty("server-pack-url")]
            public string ServerPackUrl;
            [DataMember(Name = "allow-self-download-client")]
            [JsonProperty("allow-self-download-client")]
            public bool AllowSelfDownloadClient;
            [DataMember(Name = "lock-background")]
            [JsonProperty("lock-background")]
            public bool LockBackground;
            [DataMember(Name = "background-path", IsRequired = false)]
            [JsonProperty("background-path")]
            public string BackgroundPath;
            [DataMember(Name = "auths", IsRequired = false)]
            [JsonProperty("auths")]
            public List<Auth> Auths;
            public class Auth
            {
                [DataMember(Name = "auth-name", IsRequired = true)]
                [JsonProperty("auth-name")]
                public string Name;
                [DataMember(Name = "auth-url", IsRequired = true)]
                [JsonProperty("auth-url")]
                public string Url;
            }
        }
        [DataContract]
        public class SavedAuth
        {
            [DataMember]
            [JsonProperty("auth-type")]
            public string AuthType { get; set; }
            [DataMember]
            [JsonProperty("display-name")]
            public string DisplayName { get; set; }
            [DataMember]
            [JsonProperty("access-token")]
            public string AccessToken { get; set; }
            [DataMember]
            [JsonProperty("uuid")]
            public string UUID { get; set; }
            [DataMember]
            [JsonProperty("properies")]
            public string Properies { get; set; }
            [DataMember]
            [JsonProperty("user-type")]
            public string UserType { get; set; }
        }
        public object Clone()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取注册表，寻找安装的java路径
        /// </summary>
        /// <returns>javaw.exe路径</returns>
        /// <summary>
        /// 获取系统物理内存大小
        /// </summary>
        /// <returns>系统物理内存大小，支持64bit,单位MB</returns>
    }

}
