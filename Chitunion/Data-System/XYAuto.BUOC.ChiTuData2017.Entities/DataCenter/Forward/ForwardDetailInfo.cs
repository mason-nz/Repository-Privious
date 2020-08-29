using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Forward
{
    public class ForwardDetailInfo
    {
       
        /// <summary>
        /// 物料ID
        /// </summary>		
        private int _materialid;
        public int MaterialId
        {
            get { return _materialid; }
            set { _materialid = value; }
        }
        /// <summary>
        /// 标题
        /// </summary>		
        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        /// <summary>
        /// 落地页URL
        /// </summary>		
        private string _url = string.Empty;
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
  
        /// <summary>
        /// 品牌名称
        /// </summary>		
        private string _brandnmae = string.Empty;
        public string BrandNmae
        {
            get { return _brandnmae; }
            set { _brandnmae = value; }
        }

        /// <summary>
        /// 车型名称
        /// </summary>		
        private string _serialname = string.Empty;
        public string SerialName
        {
            get { return _serialname; }
            set { _serialname = value; }
        }

        /// <summary>
        /// 分发渠道名称
        /// </summary>		
        private string _channelname = string.Empty;
        public string ChannelName
        {
            get { return _channelname; }
            set { _channelname = value; }
        }

        /// <summary>
        /// 物料场景名称
        /// </summary>		
        private string _scenename = string.Empty;
        public string SceneName
        {
            get { return _scenename; }
            set { _scenename = value; }
        }
  
        /// <summary>
        /// 物料类型名称
        /// </summary>		
        private string _materialtypename = string.Empty;
        public string MaterialTypeName
        {
            get { return _materialtypename; }
            set { _materialtypename = value; }
        }
        /// <summary>
        /// 封装时间
        /// </summary>		
        private DateTime _encapsulatetime;
        public DateTime EncapsulateTime
        {
            get { return _encapsulatetime; }
            set { _encapsulatetime = value; }
        }
        /// <summary>
        /// 分发时间
        /// </summary>		
        private DateTime _distributetime;
        public DateTime DistributeTime
        {
            get { return _distributetime; }
            set { _distributetime = value; }
        }
        /// <summary>
        /// 转发统计时间
        /// </summary>		
        private DateTime _forwardstatisticstime;
        public DateTime ForwardStatisticsTime
        {
            get { return _forwardstatisticstime; }
            set { _forwardstatisticstime = value; }
        }
        /// <summary>
        /// 转发次数
        /// </summary>		
        private int _materialforwardcount;
        public int MaterialForwardCount
        {
            get { return _materialforwardcount; }
            set { _materialforwardcount = value; }
        }
        /// <summary>
        /// 账号分值（头部）
        /// </summary>		
        private decimal _accountscore;
        public decimal AccountScore
        {
            get { return _accountscore; }
            set { _accountscore = value; }
        }
        /// <summary>
        /// 文章分值（头部）
        /// </summary>		
        private decimal _articlescore;
        public decimal ArticleScore
        {
            get { return _articlescore; }
            set { _articlescore = value; }
        }

    }
}
