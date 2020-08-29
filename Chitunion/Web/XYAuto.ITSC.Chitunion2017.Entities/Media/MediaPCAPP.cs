using System;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    //媒体-媒体信息
    public class MediaPcApp : OtherRelevantInfo

    {
        public MediaPcApp()
        {
            this.AuditStatus = (int)MediaAuditStatusEnum.PendingAudit;
            this.CreateTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;
        }

        /// <summary>
        /// MediaID
        /// </summary>
        private int _mediaid;

        public int MediaID
        {
            get { return _mediaid; }
            set { _mediaid = value; }
        }

        /// <summary>
        /// 微信名称
        /// </summary>
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 头像的URL地址
        /// </summary>
        private string _headiconurl;

        public string HeadIconURL
        {
            get { return _headiconurl; }
            set { _headiconurl = value; }
        }

        /// <summary>
        /// 行业分类枚举ID
        /// </summary>
        private int _categoryid = -1;

        public int CategoryID
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        /// <summary>
        /// ProvinceID
        /// </summary>
        private int _provinceid = -1;

        public int ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }

        /// <summary>
        /// CityID
        /// </summary>
        private int _cityid = -1;

        public int CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

        /// <summary>
        /// 媒体级别（意见领袖或普通）
        /// </summary>
        private string _terminal;

        public string Terminal
        {
            get { return _terminal; }
            set { _terminal = value; }
        }

        /// <summary>
        /// 是否预约
        /// </summary>
        private int _dailylive;

        public int DailyLive
        {
            get { return _dailylive; }
            set { _dailylive = value; }
        }

        /// <summary>
        /// 媒体状态
        /// </summary>
        private int _dailyip;

        public int DailyIP
        {
            get { return _dailyip; }
            set { _dailyip = value; }
        }

        /// <summary>
        /// 网址
        /// </summary>
        private string _website = string.Empty;

        public string WebSite
        {
            get { return _website; }
            set { _website = value; }
        }

        /// <summary>
        /// 媒体介绍
        /// </summary>
        private string _remark = string.Empty;

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        /// <summary>
        /// CreateTime
        /// </summary>
        private DateTime _createtime;

        public DateTime CreateTime
        {
            get { return _createtime; }
            set { _createtime = value; }
        }

        /// <summary>
        /// CreateUserID
        /// </summary>
        private int _createuserid;

        public int CreateUserID
        {
            get { return _createuserid; }
            set { _createuserid = value; }
        }

        /// <summary>
        /// LastUpdateTime
        /// </summary>
        private DateTime _lastupdatetime;

        public DateTime LastUpdateTime
        {
            get { return _lastupdatetime; }
            set { _lastupdatetime = value; }
        }

        /// <summary>
        /// LastUpdateUserID
        /// </summary>
        private int _lastupdateuserid;

        public int LastUpdateUserID
        {
            get { return _lastupdateuserid; }
            set { _lastupdateuserid = value; }
        }

        private int _source = -1;

        public int Source
        {
            get { return _source; }
            set { _source = value; }
        }

        //ls
        /// <summary>
        /// 录入人
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 所在地
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 覆盖区域
        /// </summary>
        public string OverlayName { get; set; }

        /// <summary>
        /// 行业
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 广告位数量
        /// </summary>
        public int ADCount { get; set; }

        public string TerminalName { get; set; }
        public int PubCount { get; set; }
        public string PubID { get; set; }
        public string UserName { get; set; }

        public int Status { get; set; }
        public int AuditStatus { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public int BaseMediaID { get; set; }
        public int AdTemplateId { get; set; }
    }

    public class OtherRelevantInfo
    {
        public string OrderRemarkStr { get; set; }//下单备注
        public string AreaMapping { get; set; }//区域分布
        //public string CommonlyClass { get; set; }//常见分类

        public string CommonlyClassStr { get; set; }//常见分类
    }
}