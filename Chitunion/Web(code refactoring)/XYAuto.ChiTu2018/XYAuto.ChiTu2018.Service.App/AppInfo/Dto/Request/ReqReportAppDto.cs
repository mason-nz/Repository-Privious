using System;
using XYAuto.ChiTu2018.Entities.Enum.LeTask;

namespace XYAuto.ChiTu2018.Service.App.AppInfo.Dto.Request
{
    public class ReqReportAppDto
    {
        //EMSI，安卓ID，手机型号，网络类型（wifi，3G，4G），屏幕分辨率，激活时间 （ 以 服 务 端 时 间 为 准 ）
        //，版本号，渠道号，运营商，系统版本，申请获取位置信息的权限，申请获取通知的权限，上传用户手机安装的app名
        //称和id
        public int UserId { get; set; }
        public string IMEI { get; set; }//EMEI
        public string IMSI { get; set; }//EMSI
        public string AndroidId { get; set; }
        public string Network { get; set; }
        public string AppVersion { get; set; }
        public string Channel { get; set; }
        public string SystemVersion { get; set; }
        public string PhoneModel { get; set; }
        /// <summary>
        /// 屏幕分辨率
        /// </summary>
        public string ScreenResolution { get; set; }
        /// <summary>
        /// 激活时间
        /// </summary>
        public DateTime ?ActivationTime { get; set; }
        /// <summary>
        /// 运营商：4：由于某种原因（未安装sim卡等）没有获取到或者没有运营商；123分别为中国移动，中国联通，中国电信
        /// </summary>
        public string Carrier { get; set; }

        /// <summary>
        /// 获取位置信息的权限
        /// </summary>
        public string AllowLocationInfo { get; set; }

        /// <summary>
        /// 获取通知的权限
        /// </summary>
        public string AllowNoticeInfo { get; set; }

        /// <summary>
        /// 用户安装的app和id
        /// </summary>
        public string ReleatedInstallAppInfo { get; set; }
        /// <summary>
        /// 是否开启消息通知设置
        /// </summary>
        public bool IsAllowMsgNotice { get; set; }    
        /// <summary>
        /// 平台，1：android    2：ios
        /// </summary>
        public int Platform { get; set; } = (int)PlatformTypeEnum.Android;
    }
}
