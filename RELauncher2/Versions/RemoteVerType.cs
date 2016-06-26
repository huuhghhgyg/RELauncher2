using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RELauncher2.Versions
{
    [DataContract]
    class RemoteVerType
    {
        [DataMember(IsRequired = true)]
        public string id;
        [DataMember(IsRequired = true)]
        public string time;
        [DataMember(IsRequired = true)]
        public string releaseTime;
        [DataMember(IsRequired = true)]
        public string type;
        [DataMember(IsRequired = false)]
        public string url;
        public RemoteVerType()
        {
            id = "";
            time = "";
            releaseTime = "";
            type = "";
            url = "";
        }
    }
}
