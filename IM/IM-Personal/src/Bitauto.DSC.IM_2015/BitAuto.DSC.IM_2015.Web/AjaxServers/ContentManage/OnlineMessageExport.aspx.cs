using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.ContentManage
{
    public partial class OnlineMessageExport : System.Web.UI.Page
    {

        #region 属性
        private string UsertName
        {
            get
            {
                return HttpContext.Current.Request["UsertName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["UsertName"].ToString());
            }
        }
        private string MessageState
        {
            get
            {
                return HttpContext.Current.Request["MessageState"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MessageState"].ToString());
            }
        }
        private string QueryStarttime
        {
            get
            {
                return HttpContext.Current.Request["QueryStarttime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryStarttime"].ToString());
            }
        }
        private string QueryEndTime
        {
            get
            {
                return HttpContext.Current.Request["QueryEndTime"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["QueryEndTime"].ToString());
            }
        }
        private string TypeID
        {
            get
            {
                return HttpContext.Current.Request["TypeID"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["TypeID"].ToString());
            }
        }
        private string SourceType
        {
            get
            {
                return HttpContext.Current.Request["SourceType"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["SourceType"].ToString());
            }
        }
        private string LastModifyUserName
        {
            get
            {
                return HttpContext.Current.Request["LastModifyUserName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["LastModifyUserName"].ToString());
            }
        }

        public int RecordCount;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            RecordCount = 0;
            QueryUserMessage query = new QueryUserMessage();
            query.UsertName = UsertName == "" ? Constant.STRING_INVALID_VALUE : UsertName;
            query.Status = MessageState == "" ? Constant.INT_INVALID_VALUE : int.Parse(MessageState);
            query.TypeID = TypeID == "" ? Constant.INT_INVALID_VALUE : int.Parse(TypeID);
            query.SourceType = SourceType == "" ? Constant.INT_INVALID_VALUE : int.Parse(SourceType);
            query.QueryStarttime = QueryStarttime == "" ? Constant.STRING_INVALID_VALUE : Convert.ToDateTime(QueryStarttime).ToString();
            query.QueryEndTime = QueryEndTime == "" ? Constant.STRING_INVALID_VALUE : Convert.ToDateTime(QueryEndTime).AddDays(1).ToString();
            query.LastModifyUserName = LastModifyUserName == "" ? Constant.STRING_INVALID_VALUE : LastModifyUserName;
            query.UserID = BLL.Util.GetLoginUserID();

            DataTable dt = BLL.UserMessage.Instance.GetUserMessage(query, "um.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 100000000, out RecordCount);

            DataTable dtNew = new DataTable();
            string[] arr = new string[12] { "访客名称", "访客来源", "咨询类型", "姓名", "电话", "时间", "内容", "操作人", "操作时间", "备注", "工单", "状态" };

            for (int i = 0; i < arr.Length; i++)
            {
                dtNew.Columns.Add(arr[i]);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtNew.NewRow();
                dr["访客名称"] = dt.Rows[i]["VUserName"];
                dr["访客来源"] = GetSourceTypeName(dt.Rows[i]["SourceType"].ToString());
                dr["咨询类型"] = BLL.UserMessage.Instance.GetTypeName(dt.Rows[i]["TypeID"].ToString()); ;
                dr["姓名"] = dt.Rows[i]["UserName"];
                dr["电话"] = dt.Rows[i]["Phone"];
                dr["时间"] = dt.Rows[i]["CreateTime"];

                dr["内容"] = dt.Rows[i]["Content"];
                dr["操作人"] = dt.Rows[i]["TrueName"].ToString();
                dr["操作时间"] = dt.Rows[i]["LastModifyTime"].ToString();
                dr["备注"] = dt.Rows[i]["Remarks"];
                dr["工单"] = dt.Rows[i]["OrderID"];

                string strStatus = dt.Rows[i]["Status"].ToString();
                if (strStatus == "1")
                {
                    dr["状态"] = "新建";
                }
                else if (strStatus == "2")
                {
                    dr["状态"] = "处理中";
                }
                else if (strStatus == "3")
                {
                    dr["状态"] = "已完成";
                }

                dtNew.Rows.Add(dr);

            }

            BLL.Util.ExportToSCV("在线留言数据导出", dtNew, false);


        }
        //获取业务线来源名称
        public string GetSourceTypeName(string sourceTypeID)
        {
            var list = BLL.Util.GetAllSourceType(true);
            var sourceTypeInfo = list.FirstOrDefault(i => i.SourceTypeValue == sourceTypeID);
            return sourceTypeInfo == null ? "" : sourceTypeInfo.SourceTypeName;
        }
    }
}