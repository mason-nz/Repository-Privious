using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Util;

namespace BitAuto.ISDC.CC2012.Web.CustInfo.MoreInfo
{
    public partial class TaskRecord : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public int PageSize
        {
            get
            {
                return this.AjaxPager_TaskRecord.PageSize;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        CustInfoHelper ch = new CustInfoHelper();

        private void BindData()
        {
            //查询
            int totalCount = 0;
            Entities.QueryProjectTaskInfo query = new Entities.QueryProjectTaskInfo();
            query.CRMCustID = ch.CustID;
            DataTable table = BitAuto.ISDC.CC2012.BLL.ProjectTaskInfo.Instance.GetProjectTaskInfoForTaskRecord(query, "LastOptTime DESC", ch.CurrentPage, PageSize, out totalCount);

            //设置数据源
            if (table != null && table.Rows.Count > 0)
            {
                repeater_TaskRecord.DataSource = table;
            }
            //绑定列表数据
            repeater_TaskRecord.DataBind();
            AjaxPager_TaskRecord.InitPager(totalCount);
        }

        //状态
        public string statusName(string statusID, string source)
        {
            string name = string.Empty;

            if (source == "0" || source == "-1")
            {
                return statusID;
            }

            int _id;
            if (int.TryParse(statusID, out _id))
            {
                name = BLL.Util.GetEnumOptText(typeof(Entities.EnumProjectTaskStatus), _id);
            }

            return name;
        }

        //获取姓名
        public string getUserName(string userID)
        {
            string name = string.Empty;

            int _id;
            if (int.TryParse(userID, out _id))
            {
                name = BLL.Util.GetNameInHRLimitEID(_id);
            }
            return name;
        }

        /// <summary>
        /// 查看链接
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="carType"></param>
        /// <param name="source"></param>
        /// <param name="htmlstr"></param>
        /// <returns></returns>
        public string getview(string taskid, string carType, string source, string TaskStatus)
        {
            string html = "";

            if (TaskStatus == "180012")
            {
                return taskid;
            }

            if (taskid.Length > 3 && taskid.Substring(0, 3) == "OLD")
            {
                return taskid;
            }

            #region 查看URL

            string url = "";

            if (source == "0")//客户核实
            {
                url = "/CRMStopCust/Edit.aspx?TaskID=" + taskid;

            }
            else if (source == "-1")//其他任务
            {
                url = "/OtherTask/OtherTaskDealView.aspx?OtherTaskID=" + taskid;
            }
            else if (source == "2")
            {
                //CRM
                if (carType == "2")
                {
                    url += "/CustCheck/CrmCustCheck/SecondCarView.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=&Action=view";
                }
                else
                {
                    url += "/CustCheck/CrmCustCheck/View.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=&Action=view";
                }
            }
            else
            {
                //Excel
                if (carType == "2")
                {
                    url += "/CustCheck/NewCustCheck/SecondCarView.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=&Action=view";
                }
                else
                {
                    url += "/CustCheck/NewCustCheck/View.aspx?TID=" + taskid + "&Page=" + PagerHelper.GetCurrentPage() + "&PageSize=" + PagerHelper.GetPageSize() + "&QueryParams=&Action=view";
                }
            }
            #endregion

            html += " <a target='_blank' href='" + url + "'>" + taskid + "</a>";

            return html;
        }
    }
}