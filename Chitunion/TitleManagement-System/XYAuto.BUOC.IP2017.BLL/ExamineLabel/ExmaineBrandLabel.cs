using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.Examine.CarBrandInfo;

namespace XYAuto.BUOC.IP2017.BLL.ExamineLabel
{
    public class ExmaineBrandLabel
    {
        public static readonly ExmaineBrandLabel Instance = new ExmaineBrandLabel();
        /// <summary>
        /// zlb 2017-10-20
        /// 查询待审品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryPendingAuditBrandList(ReqsAuditBrandDto ReqDto)
        {
            if (ReqDto == null)
            {
                return null;
            }
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            DataTable dt = Dal.ExamineLabel.ExmaineBrandLabel.Instance.QueryPendingAuditBrandList(ReqDto);
            List<PendingAuditBrand> brandList = Util.DataTableToList<PendingAuditBrand>(dt);
            if (brandList != null && brandList.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", brandList);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询已审品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryAuditedBrandList(ReqsAuditBrandDto ReqDto)
        {
            if (ReqDto == null)
            {
                return null;
            }
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            DataTable dt = Dal.ExamineLabel.ExmaineBrandLabel.Instance.QueryAuditedBrandList(ReqDto);
            List<AuditedBrand> brandList = Util.DataTableToList<AuditedBrand>(dt);
            if (brandList != null && brandList.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", brandList);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-23
        ///根据审核批次ID查询品牌待审标签和已审标签
        /// </summary>
        /// <param name="BatchAuditID">审核批次ID</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryAuditBrandLabel(int BatchAuditID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            LabelStatistics lbStatic = new LabelStatistics();
            dicAll.Add("BrandID", "");
            dicAll.Add("SerialID", "");
            dicAll.Add("SerialName", "");
            dicAll.Add("BrandName", "");
            dicAll.Add("OperateInfo", new Dictionary<string, object>());
            dicAll.Add("AuditInfo", new Dictionary<string, object>());
            dicAll.Add("Category", lbStatic.listCategory);
            dicAll.Add("MarketScene", lbStatic.listMarketScene);
            dicAll.Add("DistributeScene", lbStatic.listDistributeScene);
            dicAll.Add("IPLabel", lbStatic.listIPLabel);
            DataTable dt = Dal.ExamineLabel.ExmaineBrandLabel.Instance.QueryBrandInfo(BatchAuditID);
            if (dt != null && dt.Rows.Count > 0)
            {
                dicAll["BrandID"] = dt.Rows[0]["BrandID"].ToString();
                dicAll["SerialID"] = dt.Rows[0]["SerialID"].ToString();
                dicAll["SerialName"] = dt.Rows[0]["SerialName"].ToString();
                dicAll["BrandName"] = dt.Rows[0]["BrandName"].ToString();
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["SubmitMan"].ToString()))
                {
                    Dictionary<string, object> dicOperate = new Dictionary<string, object>();
                    dicOperate.Add("UserName", dt.Rows[0]["SubmitMan"].ToString());
                    dicOperate.Add("CreateTime", dt.Rows[0]["SubmitTime"].ToString());
                    dicAll["OperateInfo"] = dicOperate;
                }
                if (!string.IsNullOrWhiteSpace(dt.Rows[0]["ExamineMan"].ToString()))
                {
                    Dictionary<string, object> dicAudit = new Dictionary<string, object>();
                    dicAudit.Add("UserName", dt.Rows[0]["ExamineMan"].ToString());
                    dicAudit.Add("CreateTime", dt.Rows[0]["ExamineTime"].ToString());
                    dicAll["AuditInfo"] = dicAudit;
                }
                bool IpIsSame = BasicLabel.Instance.StatisticsLabel(BatchAuditID, lbStatic);
                dicAll.Add("IpIsSame", IpIsSame);
            }
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-24
        /// 根据批次ID查询品牌待审标签或最终结果标签
        /// </summary>
        /// <param name="BatchID">批次ID(审核或最终结果)</param>
        /// <param name="SelectType">查询类型（1审核 2修改）</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryPendingAuditBrandLabel(int BatchID, int SelectType)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            LabelList labelList = new LabelList();
            dicAll.Add("BrandID", "");
            dicAll.Add("SerialID", "");
            dicAll.Add("SerialName", "");
            dicAll.Add("BrandName", "");
            dicAll.Add("Category", labelList.listCategory);
            dicAll.Add("MarketScene", labelList.listMarketScene);
            dicAll.Add("DistributeScene", labelList.listDistributeScene);
            dicAll.Add("IPLabel", labelList.listIPLabel);
            DataTable dt = null;
            DataSet dsPending = null;
            if (SelectType == 1)
            {

                dt = Dal.ExamineLabel.ExmaineBrandLabel.Instance.QueryBrandInfo(BatchID);
                dsPending = Dal.ExamineLabel.BasicLabel.Instance.QueryPendingAuditLabelList(BatchID);
            }
            else
            {
                dt = Dal.Result.ResultLabel.Instance.QueryResultBrandInfo(BatchID);
                dsPending = Dal.ExamineLabel.BasicLabel.Instance.QueryResultLabelList(BatchID);
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                dicAll["BrandID"] = dt.Rows[0]["BrandID"].ToString();
                dicAll["SerialID"] = dt.Rows[0]["SerialID"].ToString();
                dicAll["SerialName"] = dt.Rows[0]["SerialName"].ToString();
                dicAll["BrandName"] = dt.Rows[0]["BrandName"].ToString();
                if (dsPending != null)
                {
                    BasicLabel.Instance.DivideLabel(dsPending, dicAll, SelectType);
                }
            }
            return dicAll;
        }

    }
}
