using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.WebService.GSData
{

    public class GSDataResult
    {
        [JsonProperty(PropertyName = "returnCode")]
        public string ReturnCode { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "returnMsg")]
        public string ReturnMsg { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "feeCount")]
        public decimal FeeCount { get; set; } = 0;

        [JsonProperty(PropertyName = "returnData")]
        public object ReturnData { get; set; } = null;
    }


    public class GSDataGroupResult
    {
        [JsonProperty(PropertyName = "nicknameCount")]
        public int NicknameCount { get; set; }

        [JsonProperty(PropertyName = "groupCount")]
        public int GroupCount { get; set; }

        [JsonProperty(PropertyName = "groupList")]
        public List<GSDataGroupInfo> GroupList { get; set; }
    }


    public class GSDataGroupInfo
    {
        [JsonProperty(PropertyName = "groupid")]
        public int GroupID { get; set; }

        [JsonProperty(PropertyName = "groupname")]
        public string GroupName { get; set; }

        [JsonProperty(PropertyName = "count")]
        public int Count { get; set; }
    }
}
