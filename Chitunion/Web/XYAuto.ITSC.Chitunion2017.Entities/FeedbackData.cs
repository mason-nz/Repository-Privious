using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYAuto.ITSC.Chitunion2017.Entities
{
    /// <summary>
    /// 2017-02-25 张立彬
    /// 反馈数据库model
    /// </summary
    public class FeedbackData
    {
        /// <summary>
        /// 反馈ID
        /// </summary>
        public int FeedbackID { get; set; }
        /// <summary>
        /// 媒体类型
        /// </summary>
        public int MediaType { get; set; }
        /// <summary>
        /// 子订单ID
        /// </summary>
        public string SubOrderID { get; set; }
        /// <summary>
        /// 广告位ID
        /// </summary>
        public int ADDetailID { get; set; }
        /// <summary>
        /// 阅读数/观看数/总观看数
        /// </summary>
        public int ReadCount { get; set; }
        /// <summary>
        ///转发数/曝光数/峰值
        /// </summary>
        public int TransmitCount { get; set; }
        /// <summary>
        /// 点赞数/提及数
        /// </summary>
        public int ClickCount { get; set; }
        /// </summary>
        /// 评论数
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 点击数
        /// </summary>
        public int LinkCount { get; set; }
        /// <summary>
        ///PV数
        /// </summary>
        public int PVCount { get; set; }
        /// <summary>
        /// UV数
        /// </summary>
        public int UVCount { get; set; }
        /// <summary>
        /// 订单数
        /// </summary>
        public int OrderCount { get; set; }
        /// <summary>
        /// 虚拟礼物价值
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        ///点击率
        /// </summary>
        public decimal ClickRate { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 反馈开始日
        /// </summary>
        public string FeedbackBeginDate { get; set; }
        /// <summary>
        /// 反馈结束日期
        /// </summary>
        public string FeedbackEndDate { get; set; }
        /// <summary>
        ///创建人ID
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 送达数（微信）
        /// </summary>
        public int DeliveredCount { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }

        public List<string> FilePathList { get; set; }
        /// <summary>
        /// 路径和名称集合
        /// </summary>
        public List<FeedBackFile> FileInfoList { get; set; }
    }



    public class FeedBackFile
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }

    public class SelectFeedbackData
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 广告位ID
        /// </summary>
        public int ADDetailID { get; set; }
        public List<FeedbackData> DataList = new List<FeedbackData>();

    }


}
