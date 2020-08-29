using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.BUOC.ChiTuData2017.Entities.DataCenter.Encapsulate
{
    public class EncapsulateDetailInfo
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
        /// 文章URL
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
        /// 物料封装类型名称
        /// </summary>		
        private string _materialname = string.Empty;
        public string MaterialName
        {
            get { return _materialname; }
            set { _materialname = value; }
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
        /// 渠道名称
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
        /// 账号
        /// </summary>		
        private string _accountname = string.Empty;
        public string AccountName
        {
            get { return _accountname; }
            set { _accountname = value; }
        }
        /// <summary>
        /// 账号分值（头部文章）
        /// </summary>		
        private decimal _accountscore;
        public decimal AccountScore
        {
            get { return _accountscore; }
            set { _accountscore = value; }
        }
        /// <summary>
        /// 文章分值（头部文章）
        /// </summary>		
        private decimal _articlescore;
        public decimal ArticleScore
        {
            get { return _articlescore; }
            set { _articlescore = value; }
        }
     
        /// <summary>
        /// 状态名称（可用、作废）
        /// </summary>		
        private string _conditionname = string.Empty;
        public string ConditionName
        {
            get { return _conditionname; }
            set { _conditionname = value; }
        }
        /// <summary>
        /// 原因
        /// </summary>		
        private string _reason = string.Empty;
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
       
    }
}
