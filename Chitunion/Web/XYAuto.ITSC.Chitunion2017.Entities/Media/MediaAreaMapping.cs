using System;

namespace XYAuto.ITSC.Chitunion2017.Entities.Media
{
    //媒体——覆盖区域信息
    public class MediaAreaMapping
    {
        public MediaAreaMapping()
        {
            this.RelateType = (int)Entities.Enum.MediaAreaMappingType.AreaMedia;
        }

        /// <summary>
        /// RecID
        /// </summary>
        private int _recid;

        public int RecID
        {
            get { return _recid; }
            set { _recid = value; }
        }

        /// <summary>
        /// 媒体分类（枚举）
        /// </summary>
        private int _mediatype;

        public int MediaType
        {
            get { return _mediatype; }
            set { _mediatype = value; }
        }

        /// <summary>
        /// 媒体ID
        /// </summary>
        private int _mediaid;

        public int MediaID
        {
            get { return _mediaid; }
            set { _mediaid = value; }
        }

        /// <summary>
        /// 省份ID（全国=0）
        /// </summary>
        private int _provinceid;

        public int ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }

        /// <summary>
        /// CityID
        /// </summary>
        private int _cityid;

        public int CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
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

        public int RelateType { get; set; }

        public string ProvinceName { get; set; }

        public string CityName { get; set; }
    }

    /// <summary>
    ///  这个地方不能加字段，这个类 不能动
    /// </summary>
    public class MediaAreaMappingTable
    {
        public MediaAreaMappingTable()
        {
            this.RelateType = (int)Entities.Enum.MediaAreaMappingType.AreaMedia;
        }

        /// <summary>
        /// RecID
        /// </summary>
        private int _recid;

        public int RecID
        {
            get { return _recid; }
            set { _recid = value; }
        }

        /// <summary>
        /// 媒体分类（枚举）
        /// </summary>
        private int _mediatype;

        public int MediaType
        {
            get { return _mediatype; }
            set { _mediatype = value; }
        }

        /// <summary>
        /// 媒体ID
        /// </summary>
        private int _mediaid;

        public int MediaID
        {
            get { return _mediaid; }
            set { _mediaid = value; }
        }

        /// <summary>
        /// 省份ID（全国=0）
        /// </summary>
        private int _provinceid;

        public int ProvinceID
        {
            get { return _provinceid; }
            set { _provinceid = value; }
        }

        /// <summary>
        /// CityID
        /// </summary>
        private int _cityid;

        public int CityID
        {
            get { return _cityid; }
            set { _cityid = value; }
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

        public int RelateType { get; set; }
    }
}