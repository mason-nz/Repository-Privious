using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.LETask
{
    //设备信息表
    public class AppDevice
    {

        //主键
        public int RecID { get; set; }

        //用户ID
        public int UserID { get; set; }

        //EMEI
        public string EMEI { get; set; }

        //EMSI
        public string EMSI { get; set; }

        //AndroidId
        public string AndroidId { get; set; }

        //网络环境
        public string Network { get; set; }

        //版本号
        public string AppVersion { get; set; }

        //渠道号
        public string Channel { get; set; }

        //操作系统版本号
        public string SystemVersion { get; set; }

        //手机型号
        public string PhoneModel { get; set; }

        //屏幕分辨率
        public string ScreenResolution { get; set; }

        //激活时间
        public DateTime ?ActivationTime { get; set; }

        //运营商
        public string Carrier { get; set; }

        //获取位置信息的权限（多个逗号分割，英文逗号）
        public string AllowLocationInfo { get; set; }

        //获取通知的权限（多个逗号分割，英文逗号）
        public string AllowNoticeInfo { get; set; }

        //用户安装的app和id（多个逗号分割，英文逗号）
        public string ReleatedInstallAppInfo { get; set; }

        //创建时间
        public DateTime CreateTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否开启消息通知设置
        /// </summary>
        public bool IsAllowMsgNotice { get; set; }
        /// <summary>
        /// 平台，1：android    2：ios
        /// </summary>
        public int Platform { get; set; }

    }
}