using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYAuto.ITSC.Chitunion2017.Entities.ChituMedia
{
    public class WeiXinLuceneModel
    {

        /// <summary>
        /// 主键
        /// </summary>		
        public int RecID { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>		
        public string WxNumber { get; set; }
        /// <summary>
        /// 原始ID
        /// </summary>		
        public string OriginalID { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>		
        public string NickName { get; set; }
        /// <summary>
        /// 公众号类型
        /// </summary>		
        public int ServiceType { get; set; }
        /// <summary>
        /// 是否认证
        /// </summary>		
        public int IsVerify { get; set; }
        /// <summary>
        /// 认证类型
        /// </summary>		
        public int VerifyType { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>		
        public string HeadImg { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>		
        public int FansCount { get; set; }
        /// <summary>
        /// 简介
        /// </summary>		
        public string Summary { get; set; }
        /// <summary>
        /// 全称\主体
        /// </summary>		
        public string FullName { get; set; }
        /// <summary>
        /// 描述、签名
        /// </summary>		
        public string Sign { get; set; }
        /// <summary>
        /// ReadNum
        /// </summary>		
        public int ReadNum { get; set; }
        /// <summary>
        /// 行业分类枚举ID
        /// </summary>		
        public int CategoryID { get; set; }
        /// <summary>
        /// IsOriginal
        /// </summary>		
        public int IsOriginal { get; set; }
        /// <summary>
        /// 关联LE_SmartSearch主键
        /// </summary>		
        public int SmartSearchID { get; set; }
        /// <summary>
        /// SourceID
        /// </summary>		
        public int SourceID { get; set; }
        /// <summary>
        /// CreateTime
        /// </summary>		
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// -1删除数据  0正常数据  1未审核数据
        /// </summary>		
        public int Status { get; set; }
        /// <summary>
        /// CreateUserID
        /// </summary>		
        public int CreateUserID { get; set; }
        /// <summary>
        /// TimestampSign
        /// </summary>		
        public DateTime TimestampSign { get; set; }
        public string ServiceTypeName { get; set; }

        public string VerifyTypeName { get; set; }

        public string CategoryName { get; set; }

        public string QrCodeUrl { get; set; }

        public int ProvinceID { get; set; }

        public int CityID { get; set; }
        /// <summary>
        /// 单图文发布价格
        /// </summary>
        public decimal P60018001 { get; set; }
        /// <summary>
        /// 单图文原创+发布
        /// </summary>
        public decimal P60018002 { get; set; }
        /// <summary>
        /// 多图文头条发布价格
        /// </summary>
        public decimal P60028001 { get; set; }
        /// <summary>
        /// 多图文头条发布+原创价格
        /// </summary>
        public decimal P60028002 { get; set; }
        /// <summary>
        /// 多图文第二条发布价格
        /// </summary>
        public decimal P60038001 { get; set; }
        /// <summary>
        /// 多图文第二条发布+原创价格
        /// </summary>
        public decimal P60038002 { get; set; }
        /// <summary>
        /// 多图文N条发布价格
        /// </summary>
        public decimal P60048001 { get; set; }


        /// <summary>
        /// 多图文N条发布+原创价格
        /// </summary>
        public decimal P60048002 { get; set; }

        /// <summary>
        /// 多图文N条发布+原创价格
        /// </summary>
        public decimal MaxPrice { get; set; }
        public decimal MinPrice { get; set; }

        public int TotalScores { get; set; }


        public string TagText { get; set; }
    }

    public class WeiXinListModel
    {
        ///// <summary>
        ///// 主键
        ///// </summary>		
        public int RecID { get; set; }
        /// <summary>
        /// 微信号
        /// </summary>		
        public string WxNumber { get; set; }
        ///// <summary>
        ///// 原始ID
        ///// </summary>		
        //private string _originalid;
        //public string OriginalID
        //{
        //    get { return _originalid; }
        //    set { _originalid = value; }
        //}
        /// <summary>
        /// 昵称
        /// </summary>		
        public string NickName { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>		
        public string HeadImg { get; set; }
        /// <summary>
        /// 粉丝数
        /// </summary>		
        public int FansCount { get; set; }
        /// <summary>
        /// 简介
        /// </summary>		
        public string Summary { get; set; }
        /// <summary>
        /// 全称\主体
        /// </summary>		
        public string FullName { get; set; }
        /// <summary>
        /// 描述、签名
        /// </summary>		
        public string Sign { get; set; }
        /// <summary>
        /// ReadNum
        /// </summary>		
        public int ReadNum { get; set; }

        /// <summary>
        /// IsOriginal
        /// </summary>		
        public int IsOriginal { get; set; }
        ///// <summary>
        ///// 关联LE_SmartSearch主键
        ///// </summary>		
        //private int _smartsearchid;
        //public int SmartSearchID
        //{
        //    get { return _smartsearchid; }
        //    set { _smartsearchid = value; }
        //}
        ///// <summary>
        ///// SourceID
        ///// </summary>		
        //private int _sourceid;
        //public int SourceID
        //{
        //    get { return _sourceid; }
        //    set { _sourceid = value; }
        //}
        /// <summary>
        /// CreateTime
        /// </summary>		
        //private DateTime _createtime;
        //public DateTime CreateTime
        //{
        //    get { return _createtime; }
        //    set { _createtime = value; }
        //}
        ///// <summary>
        ///// CreateUserID
        ///// </summary>		
        //private int _createuserid;
        //public int CreateUserID
        //{
        //    get { return _createuserid; }
        //    set { _createuserid = value; }
        //}
        /// <summary>
        /// TotalScores
        /// </summary>		
        //private int _totalscores;
        //public int TotalScores
        //{
        //    get { return _totalscores; }
        //    set { _totalscores = value; }
        //}

        public string CategoryName { get; set; }

        public string QrCodeUrl { get; set; }

        public List<PositionModel> Position
        {
            get; set;
        }

        public int TotalScores { get; set; }

        public decimal IndexScore { get; set; }

        public string TagText { get; set; }

    }
    public class LuceneQuery
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string LastTime { get; set; }

    }
}
