using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using XYAuto.ITSC.Chitunion2017.Entities.Enum;

namespace XYAuto.ITSC.Chitunion2017.BLL.Media.Business.V1_4
{
    public class MediaCommonInfo
    {
        public static readonly MediaCommonInfo Instance = new MediaCommonInfo();
        /// <summary>
        /// 2017-06-05 zlb
        /// 添加下单备注 
        /// </summary>
        /// <param name="MediaRelation">MediaRelationType枚举</param>
        /// <param name="EnumType">枚举类型 45001：刊例，45002：微信，45003：App备注</param>
        /// <param name="RelationID">关联ID(刊例或媒体的ID)</param>
        /// <param name="ListRemarkID">下单备注ID集合</param> （不接竞品 = 40001, 不接微商 = 40003, 不接理财和保险 = 40004,不接养生和保健品 = 40005,不接医疗和美容 = 40007,不接招商加盟 = 40008,其他 = 40009）
        /// <param name="OtherContent">其他内容（为空则不添加）</param>
        /// <param name="ErrorDescribe">返回的错误描述</param>
        /// <returns>大于0插入成功</returns>
        public int InsertMediaRemark(MediaRelationType MediaRelation, int EnumType, int RelationID, List<int> ListRemarkID, string OtherContent, out string ErrorDescribe)
        {
            ErrorDescribe = VerificationInsert(EnumType, ListRemarkID);
            if (ErrorDescribe != "")
            {
                return -1;
            }
            string mediaTableName = "Publish_Remark";
            if (MediaRelation == MediaRelationType.BaseTable)
            {
                mediaTableName = "Media_Remark_Basic";
            }
            else
            {
                mediaTableName = "Publish_Remark";
            }
            int userID = Common.UserInfo.GetLoginUserID();
            int result = Dal.Media.MediaPCAPP.Instance.InsertMediaRemark(mediaTableName, EnumType, RelationID, ListRemarkID, OtherContent.Trim(), userID, DateTime.Now);
            if (result <= 0)
            {
                ErrorDescribe = "添加失败";
            }
            return result;
        }
        private string VerificationInsert(int EnumType, List<int> ListRemarkID)
        {
            if (!Enum.IsDefined(typeof(MediaRemarkTypeEnum), EnumType))
            {
                return "类型错误";
            }
            if (ListRemarkID.Count <= 0)
            {
                return "请填写下单备注";
            }
            for (int i = 0; i < ListRemarkID.Count; i++)
            {
                if (!Enum.IsDefined(typeof(MediaRemarkEnum), ListRemarkID[i]))
                {
                    return "备注类型错误";
                }
            }
            if (ListRemarkID.Contains((int)MediaRemarkEnum.其他))
            {
                ListRemarkID.Remove((int)MediaRemarkEnum.其他);
            }
            return "";
        }
        /// <summary>
        /// 2017-06-08 zlb
        /// 查询自己的拉黑或收藏媒体列表
        /// </summary>
        /// <param name="SelectType">1收藏 2拉黑</param>
        /// <param name="PageIndex">第几页</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public ListTotal SelectCollectionPullBlack(int SelectType, int PageIndex, int pageSize, out string ErrorMsg)
        {
            ErrorMsg = "";

            if (SelectType != 1 && SelectType != 2)
            {
                ErrorMsg = "查询参数错误";
                return null;
            }
            ListTotal lt = new ListTotal();
            int userID = Common.UserInfo.GetLoginUserID();
            DataTable dt = Dal.Media.MediaCollectionBlacklist.Instance.SelectCollectionPullBlack(SelectType, userID, PageIndex, pageSize);
            if (dt != null && dt.Rows.Count > 0)
            {
                lt.TotalCount = Convert.ToInt32(dt.Columns["TotalCount"].Expression);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    DataRow dw = dt.Rows[i];
                    dic.Add("TemplateID", dw["TemplateID"]);
                    dic.Add("MediaType", dw["MediaType"]);
                    dic.Add("MediaId", dw["MediaID"]);
                    dic.Add("Name", dw["Name"].ToString());
                    dic.Add("CreateUser", dw["CreateUser"].ToString());
                    dic.Add("HeadIconURL", dw["HeadIconURL"].ToString());
                    dic.Add("Status", dw["Status"]);
                    lt.listDetail.Add(dic);
                }
            }
            return lt;
        }

    }
}
