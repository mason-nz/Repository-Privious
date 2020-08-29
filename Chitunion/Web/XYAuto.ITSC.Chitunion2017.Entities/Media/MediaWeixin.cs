using System;
using System.Collections.Generic;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    //媒体-微信公众号信息
    public class MediaWeixin
    {
        public MediaWeixin()
        {
            PublishStatus = Entities.Constants.Constant.INT_INVALID_VALUE;
            AuditStatus = (int)PublishBasicStatusEnum.待审核;
            AuthType = Entities.Constants.Constant.INT_INVALID_VALUE;
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
        /// 微信号
        /// </summary>
        private string _number;

        public string Number
        {
            get { return _number; }
            set { _number = value; }
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
        /// 二维码URL地址
        /// </summary>
        private string _twocodeurl;

        public string TwoCodeURL
        {
            get { return _twocodeurl; }
            set { _twocodeurl = value; }
        }

        /// <summary>
        /// 粉丝数
        /// </summary>
        private int _fanscount;

        public int FansCount
        {
            get { return _fanscount; }
            set { _fanscount = value; }
        }

        /// <summary>
        /// 粉丝数截图
        /// </summary>
        private string _fanscounturl;

        public string FansCountURL
        {
            get { return _fanscounturl; }
            set { _fanscounturl = value; }
        }

        /// <summary>
        /// 粉丝男比例
        /// </summary>
        private decimal _fansmaleper;

        public decimal FansMalePer
        {
            get { return _fansmaleper; }
            set { _fansmaleper = value; }
        }

        /// <summary>
        /// 粉丝女比例
        /// </summary>
        private decimal _fansfemaleper;

        public decimal FansFemalePer
        {
            get { return _fansfemaleper; }
            set { _fansfemaleper = value; }
        }

        /// <summary>
        /// 行业分类枚举ID
        /// </summary>
        private int _categoryid = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int CategoryID
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        /// <summary>
        /// ProvinceID
        /// </summary>
        private int _provinceid = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }

        /// <summary>
        /// CityID
        /// </summary>
        private int _cityid = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
        }

        /// <summary>
        /// 描述、签名
        /// </summary>
        private string _sign;

        public string Sign
        {
            get { return _sign; }
            set { _sign = value; }
        }

        /// <summary>
        /// 媒体领域枚举
        /// </summary>
        private int _areaid = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int AreaID
        {
            get { return _areaid; }
            set { _areaid = value; }
        }

        /// <summary>
        /// 媒体级别（意见领袖或普通）
        /// </summary>
        private int _leveltype = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int LevelType
        {
            get { return _leveltype; }
            set { _leveltype = value; }
        }

        /// <summary>
        /// 是否为微信认证
        /// </summary>
        private bool _isauth;

        public bool IsAuth
        {
            get { return _isauth; }
            set { _isauth = value; }
        }

        /// <summary>
        /// 下单备注（枚举）
        /// </summary>
        private string _orderremark;

        public string OrderRemark
        {
            get { return _orderremark; }
            set { _orderremark = value; }
        }

        /// <summary>
        /// 是否预约
        /// </summary>
        private bool _isreserve;

        public bool IsReserve
        {
            get { return _isreserve; }
            set { _isreserve = value; }
        }

        /// <summary>
        /// 媒体状态
        /// </summary>
        private int _status;

        public int Status
        {
            get { return _status; }
            set { _status = value; }
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

        private int _source = Entities.Constants.Constant.INT_INVALID_VALUE;

        public int Source
        {
            get { return _source; }
            set { _source = value; }
        }

        public int AuthType { get; set; }//授权方式

        public int PublishStatus { get; set; }//发布状态

        public int AuditStatus { get; set; }//审核状态

        public string FansSexScaleUrl { get; set; }//男女粉丝比例截图
        public string FansAreaShotUrl { get; set; }//粉丝区域分布截图

        public string OrderRemarkStr { get; set; }
        public List<int> CommonlyClass { get; set; }
        public string CommonlyClassStr { get; set; }
        public string AreaMapping { get; set; }
        public List<FansAreaDto> FansArea { get; set; }

        public bool IsAreaMedia { get; set; }

        //额外返回字段 ls
        /// <summary>
        /// 媒体级别
        /// </summary>
        public string LevelTypeName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        public string TrueName { get; set; }

        public string UserName { get; set; }
        public string OrderRemarkName { get; set; }
        public string CategoryName { get; set; }
        public string AreaName { get; set; }
        public string MediaAreaName { get; set; }
        public string Mobile { get; set; }
        public string OverlayName { get; set; }
        public int PubCount { get; set; }
        public string PubID { get; set; }

        public bool CanAddToRecommend { get; set; }
        public bool IsRange { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }

        public string ADName { get; set; }
        public int WxID { get; set; }
    }

    public class FansAreaDto
    {
        public int ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public decimal UserScale { get; set; }
    }
}