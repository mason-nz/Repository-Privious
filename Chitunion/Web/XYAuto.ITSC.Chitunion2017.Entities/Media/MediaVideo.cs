using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    //媒体-视频信息
    public class MediaVideo
    {
        public MediaVideo()
        {
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
        /// Platform
        /// </summary>
        private int _platform = -1;

        public int Platform
        {
            get { return _platform; }
            set { _platform = value; }
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
        /// Sex
        /// </summary>
        private string _sex;

        public string Sex
        {
            get { return _sex; }
            set { _sex = value; }
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
        /// 行业分类枚举ID
        /// </summary>
        private int _categoryid = -1;

        public int CategoryID
        {
            get { return _categoryid; }
            set { _categoryid = value; }
        }

        /// <summary>
        /// Profession
        /// </summary>
        private int _profession = -1;

        public int Profession
        {
            get { return _profession; }
            set { _profession = value; }
        }

        /// <summary>
        /// 是否为微信认证
        /// </summary>
        private int _authtype;

        public int AuthType
        {
            get { return _authtype; }
            set { _authtype = value; }
        }

        /// <summary>
        /// 媒体级别（普通或名人）
        /// </summary>
        private int _leveltype = -1;

        public int LevelType
        {
            get { return _leveltype; }
            set { _leveltype = value; }
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

        private int _source = -1;

        public int Source
        {
            get { return _source; }
            set { _source = value; }
        }

        //ls
        /// <summary>
        /// 平台名称
        /// </summary>
        public string PlatformName { get; set; }

        /// <summary>
        /// 录入人
        /// </summary>
        public string TrueName { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public string LevelTypeName { get; set; }

        public string CategoryName { get; set; }
        public string AreaName { get; set; }
        public string ProfessionName { get; set; }
        public string OverlayName { get; set; }
        public int PubCount { get; set; }
        public string PubID { get; set; }
        public string UserName { get; set; }
        public bool CanAddToRecommend { get; set; }
        public bool IsRange { get; set; }
    }
}