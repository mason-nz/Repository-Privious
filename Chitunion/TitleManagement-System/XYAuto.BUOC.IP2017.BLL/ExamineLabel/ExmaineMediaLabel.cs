using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.Examine.MeidaInfo;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.BLL.ExamineLabel
{
    public class ExmaineMediaLabel
    {
        public static readonly ExmaineMediaLabel Instance = new ExmaineMediaLabel();
        /// <summary>
        /// zlb 2017-10-20
        /// 查询待审媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryPendingAuditMediaList(ReqsAuditMediaDto ReqDto)
        {
            if (ReqDto == null)
            {
                return null;
            }
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            DataTable dt = Dal.ExamineLabel.ExmaineMediaLabel.Instance.QueryPendingAuditMediaList(ReqDto);
            List<PendingAuditMedia> mediaList = Util.DataTableToList<PendingAuditMedia>(dt);
            if (mediaList != null && mediaList.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", mediaList);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询已审媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryAuditedMediaList(ReqsAuditMediaDto ReqDto)
        {
            if (ReqDto == null)
            {
                return null;
            }
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            DataTable dt = Dal.ExamineLabel.ExmaineMediaLabel.Instance.QueryAuditedMediaList(ReqDto);
            List<AuditedMedia> mediaList = Util.DataTableToList<AuditedMedia>(dt);
            if (mediaList != null && mediaList.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", mediaList);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-23
        ///根据审核批次ID查询媒体待审标签和已审标签
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryAuditMediaLabel(int BatchAuditID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            LabelStatistics lbStatic = new LabelStatistics();
            dicAll.Add("MediaType", "");
            dicAll.Add("Name", "");
            dicAll.Add("Number", "");
            dicAll.Add("HeadImg", "");
            dicAll.Add("HomeUrl", "");
            dicAll.Add("OperateInfo", new Dictionary<string, object>());
            dicAll.Add("AuditInfo", new Dictionary<string, object>());
            dicAll.Add("Category", lbStatic.listCategory);
            dicAll.Add("MarketScene", lbStatic.listMarketScene);
            dicAll.Add("DistributeScene", lbStatic.listDistributeScene);
            dicAll.Add("IPLabel", lbStatic.listIPLabel);
            DataTable dt = Dal.ExamineLabel.ExmaineMediaLabel.Instance.QueryMediaInfo(BatchAuditID);
            AuditedMedia mediaInfo = Util.DataTableToEntity<AuditedMedia>(dt);
            if (mediaInfo != null)
            {
                dicAll["MediaType"] = mediaInfo.MediaType;
                dicAll["Name"] = mediaInfo.Name;
                dicAll["Number"] = mediaInfo.Number;
                dicAll["HeadImg"] = mediaInfo.HeadImg;
                dicAll["HomeUrl"] = mediaInfo.HomeUrl;
                string articleIds = Dal.ExamineLabel.ExmaineMediaLabel.Instance.QueryMediaArticleIdList(BatchAuditID);
                dicAll.Add("ArticleIDs", articleIds);
                if (!string.IsNullOrWhiteSpace(mediaInfo.SubmitMan))
                {
                    Dictionary<string, object> dicOperate = new Dictionary<string, object>();
                    dicOperate.Add("UserName", mediaInfo.SubmitMan);
                    dicOperate.Add("CreateTime", mediaInfo.SubmitTime);
                    dicAll["OperateInfo"] = dicOperate;
                }
                if (!string.IsNullOrWhiteSpace(mediaInfo.ExamineMan))
                {
                    Dictionary<string, object> dicAudit = new Dictionary<string, object>();
                    dicAudit.Add("UserName", mediaInfo.ExamineMan);
                    dicAudit.Add("CreateTime", mediaInfo.ExamineTime);
                    dicAll["AuditInfo"] = dicAudit;
                }
                bool IpIsSame = BasicLabel.Instance.StatisticsLabel(BatchAuditID, lbStatic);
                dicAll.Add("IpIsSame", IpIsSame);
            }
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-24
        /// 根据批次ID查询媒体待审标签或最终结果标签
        /// </summary>
        /// <param name="BatchID">批次ID(审核或最终结果)</param>
        /// <param name="SelectType">查询类型（1审核 2修改）</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryPendingAuditMediaLabel(int BatchID, int SelectType)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("MediaType", "");
            dicAll.Add("Name", "");
            dicAll.Add("Number", "");
            dicAll.Add("HeadImg", "");
            dicAll.Add("HomeUrl", "");
            dicAll.Add("Category", new List<Dictionary<string, object>>());
            dicAll.Add("MarketScene", new List<Dictionary<string, object>>());
            dicAll.Add("DistributeScene", new List<Dictionary<string, object>>());
            dicAll.Add("IPLabel", new List<Dictionary<string, object>>());
            DataTable dt = null;
            DataSet dsPending = null;
            if (SelectType == 1)
            {
                string articleIds = Dal.ExamineLabel.ExmaineMediaLabel.Instance.QueryMediaArticleIdList(BatchID);
                dicAll.Add("ArticleIDs", articleIds);
                dt = Dal.ExamineLabel.ExmaineMediaLabel.Instance.QueryMediaInfo(BatchID);
                dsPending = Dal.ExamineLabel.BasicLabel.Instance.QueryPendingAuditLabelList(BatchID);
            }
            else
            {
                dt = Dal.Result.ResultLabel.Instance.QueryResultMediaInfo(BatchID);
                dsPending = Dal.ExamineLabel.BasicLabel.Instance.QueryResultLabelList(BatchID);
            }
            MediaInfo mediaInfo = Util.DataTableToEntity<MediaInfo>(dt);
            if (mediaInfo != null)
            {
                dicAll["MediaType"] = mediaInfo.MediaType;
                dicAll["Name"] = mediaInfo.Name;
                dicAll["Number"] = mediaInfo.Number;
                dicAll["HeadImg"] = mediaInfo.HeadImg;
                dicAll["HomeUrl"] = mediaInfo.HomeUrl;
                if (dsPending != null)
                {
                    BasicLabel.Instance.DivideLabel(dsPending, dicAll,SelectType);
                }
            }
            return dicAll;
        }


    }
}
