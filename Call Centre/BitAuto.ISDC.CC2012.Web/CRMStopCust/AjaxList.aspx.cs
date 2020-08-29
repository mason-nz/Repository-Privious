using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;

namespace BitAuto.ISDC.CC2012.Web.CRMStopCust
{
    public partial class AjaxList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        /// JSON字符串
        /// </summary>
        public string JsonStr
        {
            get
            {
                return HttpContext.Current.Request["JsonStr"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["JsonStr"].ToString());
            }

        }
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                BindData();
            }
        }

        //绑定数据
        public void BindData()
        {
            Entities.QueryStopCustApply query = new Entities.QueryStopCustApply();

            string errMsg = string.Empty;
            BLL.ConverToEntitie<Entities.QueryStopCustApply> conver = new BLL.ConverToEntitie<Entities.QueryStopCustApply>(query);
            errMsg = conver.Conver(JsonStr);

            if (errMsg != "")
            {
                return;
            }

            query.LoginID = userID;
            int RecordCount = 0;

            DataTable dt = BLL.StopCustApply.Instance.GetStopCustApply(query, " sca.ApplyTime desc ", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            AjaxPager.PageSize = 20;
            AjaxPager.InitPager(RecordCount);
        }

        public string GetTaskStatusName(string taskStatus)
        {
            int _status = CommonFunction.ObjectToInteger(taskStatus);
            return BLL.Util.GetEnumOptText(typeof(Entities.StopCustTaskStatus), _status);
        }
        public string GetStopStatusName(string stopstatus, string applytype)
        {
            int _status = CommonFunction.ObjectToInteger(stopstatus);
            string result = BLL.Util.GetEnumOptText(typeof(Entities.StopCustStopStatus), _status);
            //停用类型
            if (applytype == "1")
            {
                if (_status == 2)
                {
                    result = "待停用";
                }
                else if (_status == 3)
                {
                    result = "已停用";
                }
            }
            //启用类型
            else if (applytype == "2")
            {
                if (_status == 2)
                {
                    result = "待启用";
                }
                else if (_status == 3)
                {
                    result = "已启用";
                }
            }
            return result;
        }
        public string GetOperLink(string status, string taskid, string taskStatus, string assignUserID)
        {
            int _status = CommonFunction.ObjectToInteger(status);
            int _taskStatus = CommonFunction.ObjectToInteger(taskStatus);

            if (_taskStatus == (int)Entities.StopCustTaskStatus.NoProcess || _taskStatus == (int)Entities.StopCustTaskStatus.Processing)
            {
                if (int.Parse(assignUserID) == BLL.Util.GetLoginUserID())
                {
                    return "<a href='Edit.aspx?op=e&TaskID=" + taskid + "' target='_blank'>处理</a>";
                }
            }
            return "<a href='View.aspx?TaskID=" + taskid + "' target='_blank' >查看</a>";
        }
    }
}