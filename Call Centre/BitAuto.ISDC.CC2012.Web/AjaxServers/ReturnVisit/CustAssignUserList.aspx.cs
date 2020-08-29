using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ReturnVisit
{
    public partial class CustAssignUserList : PageBase
    {
        #region 属性
        /// <summary>
        /// 姓名
        /// </summary>
        public string TrueName
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["TrueName"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["TrueName"]);
            }
        }
        /// <summary>
        /// 所属分组
        /// </summary>
        public string BGID
        {
            set;
            get;
        }
        /// <summary>
        /// 操作方式（默认null）
        /// 新增：createuser, operuser, employee
        /// 强斐 2014年7月10日
        /// </summary>
        public string Action
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Action"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Action"]);
            }
        }
        /// <summary>
        /// 选择项显示的分组
        /// </summary>
        public string DisplayGroupID
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["DisplayGroupID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["DisplayGroupID"]);
            }
        }

        /// <summary>
        /// 分组下拉
        /// </summary>
        public string Groups
        {
            get
            {
                return string.IsNullOrEmpty(HttpContext.Current.Request["Groups"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["Groups"]);
            }
        }

        //add by lixiaomin  所属分组是否显示自己所在的分组
        //public bool ShowSelfGroup
        //{
        //    get
        //    {
        //        bool flag = false;
        //        if (Request["ShowSelfGroup"] != null)
        //        {
        //            try
        //            {
        //                flag = bool.Parse(HttpUtility.UrlDecode(Request["ShowSelfGroup"].ToString()));
        //            }
        //            catch
        //            {

        //            }

        //        }
        //        return flag;
        //    }
        //}
        #endregion

        public int GroupLength = 5;
        public int PageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            BGID = string.IsNullOrEmpty(HttpContext.Current.Request["BGID"]) == true ? "" : HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["BGID"]);
            if (!IsPostBack)
            {
                if (Action == "createuser")
                {
                    literal_title.Text = "选择创建人";
                }
                else if (Action == "operuser")
                {
                    literal_title.Text = "选择操作人";
                }
                else if (Action == "addblackdatauser")
                {
                    literal_title.Text = "选择添加人";
                }
                else if (Action == "addwhitedatauser")
                {
                    literal_title.Text = "选择添加人";
                }
                else if (Action == "workordersubmituser")
                {
                    literal_title.Text = "选择工单提交人";
                }
                else if (Action == "worderV2submituser")
                {
                    literal_title.Text = "选择工单提交人";
                }
                else
                {
                    literal_title.Text = "选择坐席";
                }

                ddlBussiGroupBind();
                BindData();
            }
        }

        /// 绑定处理人
        /// <summary>
        /// 绑定处理人
        /// </summary>
        private void ddlBussiGroupBind()
        {
            //特殊情况说明
            //1 DisplayGroupID 存在时，下拉框只显示DisplayGroupID对于的分组，没有请选择选项，查询时，必须存在DisplayGroupID条件

            //2 Groups 存在时，值是[19|智能平台运营支持部,27|二手车运营支持部],需要从Groups中取值

            // 3 其他情况下，取管辖分组

            //根据DisplayGroupID的值选择默认值

            DataTable bgdt = new DataTable();
            if (!string.IsNullOrEmpty(DisplayGroupID) && CommonFunction.ObjectToInteger(DisplayGroupID) > 0)
            {
                bgdt = BLL.BusinessGroup.Instance.GetBusinessGroupByBGID(CommonFunction.ObjectToInteger(DisplayGroupID));
            }
            else if (!string.IsNullOrEmpty(Groups))
            {
                bgdt = GetGroupsData(bgdt);
            }
            else
            {
                int userid = BLL.Util.GetLoginUserID();

                #region  ShowSelfGroup属性启用
                //if (ShowSelfGroup)
                //{
                //    //管辖分组+所属分组 默认第一个：所属分组
                //    bgdt = BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userid);
                //}
                //else
                //{
                //    //管辖分组
                //    bgdt = BLL.EmployeeSuper.Instance.GetCurrentUseBusinessGroup(userid);
                //}
                #endregion

                #region  ShowSelfGroup属性作废
                //管辖分组+所属分组 默认第一个：所属分组
                bgdt = BLL.EmployeeSuper.Instance.GetCurrentUserGroups(userid);
                #endregion
            }

            if (bgdt != null && bgdt.Rows.Count > 0)
            {
                //排序
                DataView dv = bgdt.DefaultView;
                dv.Sort = "Name ASC";
                bgdt = dv.ToTable();
            }
            if (bgdt == null)
            {
                return;
            }
            //添加请选择
            DataRow dr = bgdt.NewRow();
            dr["BGID"] = "-2";
            dr["Name"] = "请选择";
            bgdt.Rows.InsertAt(dr, 0);

            //绑定数据
            ddlBussiGroup.DataSource = bgdt;
            ddlBussiGroup.DataTextField = "Name";
            ddlBussiGroup.DataValueField = "BGID";
            ddlBussiGroup.DataBind();


            if (!string.IsNullOrEmpty(DisplayGroupID) && CommonFunction.ObjectToInteger(DisplayGroupID) > 0)
            {
                BGID = DisplayGroupID;
                ddlBussiGroup.Value = DisplayGroupID;
            }
            else if (string.IsNullOrEmpty(BGID))
            {
                BGID = "-2";
                ddlBussiGroup.Value = "-2";
            }
        }

        private DataTable GetGroupsData(DataTable bgdt)
        {
            string[] bgrow = Groups.Split(',');
            string bgids = "";
            for (int i = 0; i < bgrow.Length; i++)
            {
                if (!string.IsNullOrEmpty(bgrow[i].Split('|')[0]))
                {
                    bgids += bgrow[i].Split('|')[0] + ",";
                }
            }
            if (bgids.EndsWith(","))
            {
                bgids = bgids.Substring(0, bgids.Length - 1);
            }
            bgdt = BLL.BusinessGroup.Instance.GetBusinessGroupByBGIDS(bgids);
            return bgdt;
        }

        /// 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        private void BindData()
        {
            int RecordCount = 0;
            int userid = BLL.Util.GetLoginUserID();
            QueryEmployeeSuper query = new QueryEmployeeSuper();
            query.OnlyCCDepart = true;
            query.ContainLoginUserID = userid;
            //分组
            query.BGID = CommonFunction.ObjectToInteger(BGID, -2);

            //下拉框
            if (query.BGID <= 0)
            {
                string str = "";
                foreach (ListItem item in ddlBussiGroup.Items)
                {
                    str += item.Value + ",";
                }
                if (str.Length > 0)
                {
                    query.BGIDs = str.TrimEnd(',');
                }
                else
                {
                    query.BGIDs = null;
                }
            }
            else
            {
                query.BGIDs = null;
            }

            //用户名称
            if (!string.IsNullOrEmpty(TrueName))
            {
                query.TrueName = TrueName;
            }

            #region 特殊条件
            if (Action == "createuser")
            {
                //创建人-其他任务
                query.SelectUserIdSql = "SELECT DISTINCT CreateUserID as UserID INTO #tmp FROM OtherTaskInfo";
            }
            else if (Action == "operuser")
            {
                //操作人-其他任务
                query.SelectUserIdSql = "SELECT DISTINCT LastOptUserID as UserID INTO #tmp FROM OtherTaskInfo";
            }
            else if (Action == "addblackdatauser")
            {
                //添加入—黑名单
                query.SelectUserIdSql = "SELECT DISTINCT CreateUserId as UserID INTO #tmp FROM dbo.BlackWhiteList WHERE [type] = 0 and [Status]=0";
            }
            else if (Action == "addwhitedatauser")
            {
                //添加人—白名单
                query.SelectUserIdSql = "SELECT DISTINCT CreateUserId as UserID INTO #tmp FROM dbo.BlackWhiteList WHERE [type] = 1 and [Status]=0";
            }
            else if (Action == "employee")
            {
                //坐席-其他任务
                query.SelectUserIdSql = "SELECT DISTINCT UserID as UserID INTO #tmp FROM ProjectTask_Employee WHERE PTID like 'OTH%'";
            }
            else if (Action == "shensuemployee")
            {
                //质量管理-申诉统计
                query.SelectUserIdSql = "SELECT DISTINCT CreateUserID as UserID INTO #tmp FROM CallRecord_ORIG_Business_qs";
            }
            else if (Action == "workordersubmituser")
            {
                //工单记录提交人
                query.SelectUserIdSql = "SELECT DISTINCT CreateUserID as UserID INTO #tmp FROM WorkOrderInfo";
            }
            else if (Action == "worderV2submituser")
            {
                //工单记录提交人
                query.SelectUserIdSql = "SELECT DISTINCT CreateUserID as UserID INTO #tmp FROM WOrderInfo";
            }
            
            #endregion

            //按条件找人：条件-部门，角色-
            DataTable dt = BLL.EmployeeSuper.Instance.GetEmployeeSuper(query, "TrueName", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repterPersonlist.DataSource = dt;
            repterPersonlist.DataBind();
            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 201);
        }
    }
}