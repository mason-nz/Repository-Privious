using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_DMS2014.Entities;
using BitAuto.DSC.IM_DMS2014.Entities.Constants;

namespace BitAuto.DSC.IM_DMS2014.Web.AjaxServers.ContentManage
{
    public partial class OnlineMessageExport : System.Web.UI.Page
    {
        private string MemberName
        {
            get
            {
                return HttpContext.Current.Request["MemberName"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["MemberName"].ToString());
            }
        }
        private string District
        {
            get
            {
                return HttpContext.Current.Request["District"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["District"].ToString());
            }
        }
        private string CityGroup
        {
            get
            {
                return HttpContext.Current.Request["CityGroup"] == null ? "" :
                HttpUtility.UrlDecode(HttpContext.Current.Request["CityGroup"].ToString());
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

        public int RecordCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
            }
        }

        private void BindData()
        {
            RecordCount = 0;
            QueryUserMessage query = new QueryUserMessage();
            query.MemberName = MemberName == "" ? Constant.STRING_INVALID_VALUE : MemberName;
            query.DistrictID = District == "" ? Constant.STRING_INVALID_VALUE : District;
            query.CityGroupID = CityGroup == "" ? Constant.STRING_INVALID_VALUE : CityGroup;
            query.LastModifyUserName = LastModifyUserName == "" ? Constant.STRING_INVALID_VALUE : LastModifyUserName;
            query.Status = MessageState == "" ? Constant.INT_INVALID_VALUE : int.Parse(MessageState);
            query.QueryStarttime = QueryStarttime == "" ? Constant.STRING_INVALID_VALUE :  Convert.ToDateTime(QueryStarttime).ToString();
            query.QueryEndTime = QueryEndTime == "" ? Constant.STRING_INVALID_VALUE : Convert.ToDateTime(QueryEndTime).AddDays(1).ToString();
            query.AgentID = BLL.Util.GetLoginUserID();


            DataTable dt = BLL.UserMessage.Instance.GetUserMessage(query, "a.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 100000000, out RecordCount);
          
            DataTable dtNew = new DataTable();
            string[] arr = new string[11] { "经销商名称", "类型", "姓名", "电话", "时间", "内容", "操作人", "操作时间", "备注" , "工单", "状态"};

            for (int i = 0; i < arr.Length; i++)
            {
                dtNew.Columns.Add(arr[i]);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtNew.NewRow();
                dr["经销商名称"] = dt.Rows[i]["MemberName"];
                dr["类型"] = dt.Rows[i]["MessageType"];
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
 
    }
}