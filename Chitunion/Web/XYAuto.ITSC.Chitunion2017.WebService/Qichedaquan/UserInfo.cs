using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.WebService.Qichedaquan
{
    /// <summary>
    /// 汽车大全用户（根据Token调用接口返回）
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 汽车大全用户Token
        /// </summary>
        public string User_token { get; set; }

        /// <summary>
        /// 汽车大全用户昵称
        /// </summary>
        public string Nick_name { get; set; }

        /// <summary>
        /// 汽车大全用户手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 汽车大全用户头像URL
        /// </summary>
        public string User_avatar { get; set; }

        /// <summary>
        /// 汽车大全用户性别（1为男性，2为女性，0代表未知）
        /// </summary>
        public int User_gender { get; set; }
    }

    //[JsonObject(MemberSerialization.OptOut)]
    public class UserResult
    {
        public int Code { get; set; }


        public UserInfo Data { get; set; }

        public string Msg { get; set; }
    
        public int Sub_code { get; set; }
    }
}
