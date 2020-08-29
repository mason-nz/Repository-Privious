using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ProjectManage
{
    public partial class AutoCallManageList : PageBase
    {
        public int PageSize = BLL.PageCommon.Instance.PageSize = 20;
        public int RecordCount;
        private int userID = 0;//
        public int GroupLength = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            userID = BLL.Util.GetLoginUserID();
            BindData();
        }
        /*
         *       name: name,
                statuss: status,
                acStatus: acStatus,
                group: group,
                category: category,
         */
        #region 属性
        private string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }
        private string Requeststatus
        {
            get { return HttpContext.Current.Request["status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status"].ToString()); }
        }
        private string RequestacStatus
        {
            get { return HttpContext.Current.Request["acStatus"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["acStatus"].ToString()); }
        }
        private string Requestgroup
        {
            get { return HttpContext.Current.Request["group"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["group"].ToString()); }
        }
        private string Requestcategory
        {
            get { return HttpContext.Current.Request["category"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["category"].ToString()); }
        }

        #endregion


        //绑定数据
        public void BindData()
        {
            Entities.QueryProjectInfo query = new Entities.QueryProjectInfo();



            DataTable dt = BLL.AutoCall_ProjectInfo.Instance.GetAutoCallProjectInfo(StringHelper.SqlFilter(RequestName), Requestgroup, Requestcategory, Requeststatus, RequestacStatus, userID, BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        public string GetStatus(string strStatus)
        {
            switch (strStatus)
            {
                case "0":
                    return "未开始";
                    break;
                case "1":
                    return "进行中";
                    break;
                case "2":
                    return "已结束";
                    break;
                default:
                    return "";
                    break;
            }
        }

        public string GetACStatus(string strACStatus)
        {
            int nStatus = 0;
            if (int.TryParse(strACStatus, out nStatus))
            {
                return BLL.Util.GetEnumOptText(typeof(ProjectACStatus), nStatus);
            }
            else
            {
                return "";
            }

        }

        //得到操作链接
        public string getOperLink(string status, string ACStatus, string strProjectID, string proName, string strCDID, string strSkillID)
        {
            int nAcStatus = -1;
            int.TryParse(ACStatus, out nAcStatus);

            string operLinkStr = string.Empty;
            if (status == "0")
            {
                if (nAcStatus == (int)ProjectACStatus.P00_未开始)
                {
                    //return string.Format("<a href=\"/ProjectManage/PopPanel/AutoCallConfig.aspx?act=edit&pjid={0}\" target='_blank'  name='a_edit'>编辑</a>&nbsp;", strProjectID);
                    //return String.Format("<a href='javascript:editProject(\"{0}\")'>编辑</a>", strProjectID);
                    return String.Format("<a href='javascript:editProject(\"{0}\",\"{1}\",\"{2}\",\"{3}\")'>编辑</a>&nbsp;", strProjectID, proName, strCDID, strSkillID);
                }
            }
            else if (status == "1")
            {
                if (nAcStatus == (int)ProjectACStatus.P00_未开始)
                {
                    //operLinkStr = string.Format("<a href=\"/ProjectManage/PopPanel/AutoCallConfig.aspx?act=edit&pjid={0}\" target='_blank'   name='a_edit'>编辑</a>&nbsp;", strProjectID);

                    operLinkStr += String.Format("<a href='javascript:editProject(\"{0}\",\"{1}\",\"{2}\",\"{3}\")'>编辑</a>&nbsp;", strProjectID, proName, strCDID, strSkillID);
                    operLinkStr += String.Format("<a href='javascript:startProject(\"{0}\")'>开始</a>&nbsp;", strProjectID);
                }
                else if (nAcStatus == (int)ProjectACStatus.P01_进行中)
                {
                    operLinkStr += String.Format("<a href='javascript:HoldProject(\"{0}\")'>暂停</a>&nbsp;", strProjectID);
                    operLinkStr += String.Format("<a href='javascript:endProject(\"{0}\")'>结束</a>", strProjectID);
                }
                else if (nAcStatus == (int)ProjectACStatus.P02_暂停中)
                {
                    operLinkStr += String.Format("<a href='javascript:startProject(\"{0}\",\"{1}\")'>开始&nbsp;</a>", strProjectID, ACStatus);
                    operLinkStr += String.Format("<a href='javascript:endProject(\"{0}\")'>结束</a>", strProjectID);
                }
            }
            else
            {
                return "";
            }
            //rightStr = "<a href='javascript:deleteTemplate(" + recID + ")'   name='a_delete'>删除</a>&nbsp;";
            return operLinkStr;
        }
    }
}