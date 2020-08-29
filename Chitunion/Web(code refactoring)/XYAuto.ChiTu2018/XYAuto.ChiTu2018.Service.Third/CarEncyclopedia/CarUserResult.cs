using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.Service.Third.CarEncyclopedia
{
    /// <summary>
    /// 注释：CarUserResult
    /// 作者：zhanglb
    /// 日期：2018/6/12 18:31:27
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public class CarUserResult
    {
        public int Code { get; set; }


        public CarUserInfo Data { get; set; }

        public string Msg { get; set; }

        public int Sub_code { get; set; }
    }
    /// <summary>
    /// 注释：汽车大全用户（根据Token调用接口返回）
    /// 作者：zhanglb
    /// 日期：2018/6/12 18:30:20
    /// 版权所有：Copyright  2018 行圆汽车-分发业务中心
    /// </summary>
    public abstract class CarUserInfo
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
}
