using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Clue
{
    public class ClueDataInfo
    {


        /// <summary>
        /// 物料ID
        /// </summary>		
        private int _materialid;
        public int MaterialID
        {
            get { return _materialid; }
            set { _materialid = value; }
        }
        /// <summary>
        /// 文章标题
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
        /// 物料类型名称
        /// </summary>		
        private string _materialname = string.Empty;
        public string MaterialName
        {
            get { return _materialname; }
            set { _materialname = value; }
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
        /// 场景名称
        /// </summary>		
        private string _scenename = string.Empty;
        public string SceneName
        {
            get { return _scenename; }
            set { _scenename = value; }
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
        /// <summary>
        /// 询价数
        /// </summary>		
        private int _inquerycount;
        public int InqueryCount
        {
            get { return _inquerycount; }
            set { _inquerycount = value; }
        }
        /// <summary>
        /// 会话数
        /// </summary>		
        private int _sessioncount;
        public int SessionCount
        {
            get { return _sessioncount; }
            set { _sessioncount = value; }
        }
        /// <summary>
        /// 电话接通数
        /// </summary>		
        private int _telconnectcount;
        public int TelConnectCount
        {
            get { return _telconnectcount; }
            set { _telconnectcount = value; }
        }
        /// <summary>
        /// 线索统计生成时间
        /// </summary>		
        private DateTime _cluedate;
        public DateTime ClueDate
        {
            get { return _cluedate; }
            set { _cluedate = value; }
        }


    }
}
