using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XYAuto.BUOC.IP2017.Entities.DTO;
using XYAuto.BUOC.IP2017.Entities.Examine;
using XYAuto.BUOC.IP2017.Entities.Examine.CarBrandInfo;
using XYAuto.BUOC.IP2017.Entities.Examine.MeidaInfo;
using XYAuto.BUOC.IP2017.Entities.Result;
using static XYAuto.BUOC.IP2017.Entities.ENUM.ENUM;

namespace XYAuto.BUOC.IP2017.BLL.ResultLabel
{
    public class ResultLabel
    {
        public static readonly ResultLabel Instance = new ResultLabel();

        /// <summary>
        /// zlb 2017-10-26
        /// 查询结果媒体标签
        /// </summary>
        /// <param name="MediaResultID">结果id</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryResultMediaLabel(int MediaResultID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            LabelStatistics lbStatic = new LabelStatistics();
            dicAll.Add("MediaType", "");
            dicAll.Add("Name", "");
            dicAll.Add("Number", "");
            dicAll.Add("HeadImg", "");
            dicAll.Add("HomeUrl", "");
            dicAll.Add("UpdateInfo", new Dictionary<string, object>());
            dicAll.Add("Category", lbStatic.listCategory);
            dicAll.Add("MarketScene", lbStatic.listMarketScene);
            dicAll.Add("DistributeScene", lbStatic.listDistributeScene);
            dicAll.Add("IPLabel", lbStatic.listIPLabel);
            DataTable dt = Dal.Result.ResultLabel.Instance.QueryResultMediaInfo(MediaResultID);
            AuditedMedia mediaInfo = Util.DataTableToEntity<AuditedMedia>(dt);
            if (mediaInfo != null)
            {
                dicAll["MediaType"] = mediaInfo.MediaType;
                dicAll["Name"] = mediaInfo.Name;
                dicAll["Number"] = mediaInfo.Number;
                dicAll["HeadImg"] = mediaInfo.HeadImg;
                dicAll["HomeUrl"] = mediaInfo.HomeUrl;
                if (!string.IsNullOrWhiteSpace(mediaInfo.SubmitMan))
                {
                    Dictionary<string, object> dicOperate = new Dictionary<string, object>();
                    dicOperate.Add("UserName", mediaInfo.SubmitMan);
                    dicOperate.Add("CreateTime", mediaInfo.SubmitTime);
                    dicAll["UpdateInfo"] = dicOperate;
                }

                ExamineLabel.BasicLabel.Instance.AssembleOldLabel(MediaResultID, lbStatic);
            }
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-26
        /// 查询结果品牌标签
        /// </summary>
        /// <param name="MediaResultID">结果id</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryResultBrandLabel(int MediaResultID)
        {
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            LabelStatistics lbStatic = new LabelStatistics();
            dicAll.Add("BrandID", "");
            dicAll.Add("SerialID", "");
            dicAll.Add("SerialName", "");
            dicAll.Add("BrandName", "");
            dicAll.Add("UpdateInfo", new Dictionary<string, object>());
            dicAll.Add("Category", lbStatic.listCategory);
            dicAll.Add("MarketScene", lbStatic.listMarketScene);
            dicAll.Add("DistributeScene", lbStatic.listDistributeScene);
            dicAll.Add("IPLabel", lbStatic.listIPLabel);
            DataTable dt = Dal.Result.ResultLabel.Instance.QueryResultBrandInfo(MediaResultID);
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
                    dicAll["UpdateInfo"] = dicOperate;
                }
                ExamineLabel.BasicLabel.Instance.AssembleOldLabel(MediaResultID, lbStatic);
            }


            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询结果媒体列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryResultMediaList(ReqsAuditMediaDto ReqDto)
        {
            if (ReqDto == null)
            {
                return null;
            }
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            DataTable dt = Dal.Result.ResultLabel.Instance.QueryResultMediaList(ReqDto);
            List<AuditedMedia> mediaList = Util.DataTableToList<AuditedMedia>(dt);
            if (mediaList != null && mediaList.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", mediaList);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-10-20
        /// 查询结果品牌列表
        /// </summary>
        /// <param name="ReqDto">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, object> QueryResultBrandList(ReqsAuditBrandDto ReqDto)
        {
            if (ReqDto == null)
            {
                return null;
            }
            Dictionary<string, object> dicAll = new Dictionary<string, object>();
            dicAll.Add("TotalCount", 0);
            DataTable dt = Dal.Result.ResultLabel.Instance.QueryResultBrandList(ReqDto);
            List<AuditedBrand> brandList = Util.DataTableToList<AuditedBrand>(dt);
            if (brandList != null && brandList.Count > 0)
            {
                dicAll["TotalCount"] = dt.Columns["TotalCount"].Expression;
            }
            dicAll.Add("List", brandList);
            return dicAll;
        }
        /// <summary>
        /// zlb 2017-11-03
        /// 插入车型品牌结果表
        /// </summary>
        /// <param name="BrandID"></param>
        /// <param name="SerialID"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public int InsertResult(int TaskType, int BrandID, int SerialID, int MediaType, string Number, string MediaName, string HeadImg, string HomeUrl, int UserId)
        {
            return Dal.Result.ResultLabel.Instance.InsertResult(TaskType, BrandID, SerialID, MediaType, Number, MediaName, HeadImg, HomeUrl, UserId);
        }
        public string DeleteMediaLabel(ReqResultIdDto Dto)
        {
            if (Dto == null)
            {
                return "参数错误";
            }
            DataTable dt = Dal.Result.ResultLabel.Instance.QueryResultInfo(Dto.MediaResultID);
            if (dt == null || dt.Rows.Count <= 0)
            {
                return "参数不存在";

            }
            int deleteResult = Dal.Result.ResultLabel.Instance.UpdateResultStatus(Dto.MediaResultID, -1);
            if (deleteResult <= 0)
            {
                return "删除失败，请重试";
            }

            string strWhere = "";
            int TaskType = Convert.ToInt32(dt.Rows[0]["TaskType"]);
            if (TaskType == (int)EnumTaskType.媒体)
            {
                int mediaType = Convert.ToInt32(dt.Rows[0]["MediaType"]);
                if (mediaType == (int)EnumMediaType.微信)
                {
                    strWhere += $" where MediaNumber='{dt.Rows[0]["Number"].ToString()}' AND  MediaType={mediaType}";
                }
                else if (mediaType == (int)EnumMediaType.头条 || mediaType == (int)EnumMediaType.搜狐)
                {
                    strWhere += $" where HomeUrl='{dt.Rows[0]["HomeUrl"].ToString()}' AND  MediaType={mediaType}";
                }
                else
                {
                    strWhere += $" where MediaName='{dt.Rows[0]["Name"].ToString()}' AND  MediaType={mediaType}";
                }
            }
            else
            {
                if (TaskType == (int)EnumTaskType.子品牌)
                {
                    strWhere += $" where BrandID={Convert.ToInt32(dt.Rows[0]["BrandID"])}";
                }
                else
                {
                    strWhere += $" where SerialID={Convert.ToInt32(dt.Rows[0]["SerialID"])}";
                }
            }
            int reuslt = Dal.ExamineLabel.ExamineLableOperate.Instance.UpdateBatchMediaStatus(strWhere, -1);
            if (reuslt <= 0)
            {
                BLL.Loger.Log4Net.Info("DeleteMediaLabel->MediaResultID：" + Dto.MediaResultID + "删除失败");
            }
            return "";
        }
    }
}
