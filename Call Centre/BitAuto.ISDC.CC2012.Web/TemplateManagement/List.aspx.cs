using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.Utils;

namespace BitAuto.ISDC.CC2012.Web.TemplateManagement
{
    public partial class List : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 属性
        private string RequestName
        {
            get { return HttpContext.Current.Request["name"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["name"].ToString()); }
        }
        private string RequestStatuss
        {
            get { return HttpContext.Current.Request["status"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["status"].ToString()); }
        }

        private string RequestGroup
        {
            get { return HttpContext.Current.Request["group"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["group"].ToString()); }
        }
        private string RequestCategory
        {
            get { return HttpContext.Current.Request["category"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["category"].ToString()); }
        }
        private string RequestCreater
        {
            get { return HttpContext.Current.Request["creater"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["creater"].ToString()); }
        }
        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["beginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["beginTime"].ToString()); }
        }
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["endTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["endTime"].ToString()); }
        }
        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;
        private int userID = 0;

        //功能权限
        bool right_edit;        //编辑权限
        bool right_add;         //新增模板权限
        bool right_delete;      //删除权限
        bool right_generate;    //生成模板权限
        bool right_enable;        //启用权限
        bool right_disable;    //停用权限

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD5102"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                right_edit = BLL.Util.CheckRight(userID, "SYS024MOD510201");
                right_add = BLL.Util.CheckRight(userID, "SYS024MOD510204");
                right_delete = BLL.Util.CheckRight(userID, "SYS024MOD510202");
                right_generate = BLL.Util.CheckRight(userID, "SYS024MOD510203");
                right_enable = BLL.Util.CheckRight(userID, "SYS024MOD510206");
                right_disable = BLL.Util.CheckRight(userID, "SYS024MOD510207");
                BindData();
            }
        }

        //绑定数据
        public void BindData()
        {
            Entities.QueryTPage query = new Entities.QueryTPage();

            if (RequestName != "")
            {
                query.TPName = StringHelper.SqlFilter(RequestName);
            }
            if (RequestStatuss != "")
            {
                query.Statuss = StringHelper.SqlFilter(RequestStatuss);
            }

            if (RequestGroup != "")
            {
                query.BGID = int.Parse(RequestGroup);
            }
            if (RequestCategory != "")
            {
                query.SCID = int.Parse(RequestCategory);
            }
            if (RequestCreater != "")
            {
                query.CreateUserID = int.Parse(RequestCreater);
            }
            if (RequestBeginTime != "")
            {
                query.BeginTime = StringHelper.SqlFilter(RequestBeginTime);
            }
            if (RequestEndTime != "")
            {
                query.EndTime = StringHelper.SqlFilter(RequestEndTime);
            }

            query.LoginID = BLL.Util.GetLoginUserID();
            //判断数据权限，数据权限如果为 2-全部，则查看所有数据
            //Entities.UserDataRigth model_userDataRight = BLL.UserDataRigth.Instance.GetUserDataRigth(userID);
            //if (model_userDataRight != null)
            //{
            //    if (model_userDataRight.RightType != 2)//数据权限不为 2-全部
            //    {
            //        query.LoginID = userID;
            //        //判断分组权限，如果权限是2-本组，则能看到本组人创建的信息；如果权限是1-本人，则只能看本人创建的信息 
            //        DataTable dt_userGroupDataRight = BLL.UserGroupDataRigth.Instance.GetUserGroupDataRigthByUserID(userID);
            //        string ownGroup = string.Empty;//权限是本组的 组串
            //        string oneSelf = string.Empty; //权限是本人的 组串
            //        for (int i = 0; i < dt_userGroupDataRight.Rows.Count; i++)
            //        {
            //            if (dt_userGroupDataRight.Rows[i]["RightType"].ToString() == "2")
            //            {
            //                ownGroup += dt_userGroupDataRight.Rows[i]["BGID"].ToString() + ",";
            //            }
            //            if (dt_userGroupDataRight.Rows[i]["RightType"].ToString() == "1")
            //            {
            //                oneSelf += dt_userGroupDataRight.Rows[i]["BGID"].ToString() + ",";
            //            }
            //        }
            //        query.OwnGroup = ownGroup.TrimEnd(',');
            //        query.OneSelf = oneSelf.TrimEnd(',');
            //    }
            //}


            int RecordCount = 0;

            DataTable dt = BLL.TPage.Instance.GetTPage(query, "CreateTime Desc", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, PageSize, BLL.PageCommon.Instance.PageIndex, 1);
        }

        //获取操作人
        public string getOperator(string createrID)
        {
            string _operator = string.Empty;
            int _createrID;
            if (int.TryParse(createrID, out _createrID))
            {
                _operator = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_createrID);
            }
            return _operator;
        }

        /// <summary>
        /// 根据statusID得到状态名
        /// </summary>
        /// <param name="ttIsData">是否有数据（是否有使用）；1-有；-2-无</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public string getStatusName(string status, string ttIsData)
        {
            string statusName = string.Empty;
            int _status = int.Parse(status);

            if (ttIsData == "1")
            {
                _status = 2;
            }

            switch (_status)
            {
                case 0: statusName = "未完成";
                    break;
                case 1: statusName = "已完成";
                    break;
                case 2: statusName = "已使用";
                    break;
                case -1: statusName = "删除";
                    break;
            }

            return statusName;
        }

        //得到操作链接
        public string getOperLink(string status, string recID, string name, string ttCode, string url, string ttIsData, string isUsed)
        {
            string operLinkStr = string.Empty;
            int _status = int.Parse(status);

            if (ttIsData == "1")
            {
                _status = 2;
            }

            switch (_status)
            {
                case 0:
                    operLinkStr += rightEdit(ttCode) + rightDelete(recID) + rightTemplate(recID, ttCode);
                    break;
                case 1: operLinkStr += rightDelete(recID) + loadExcel(url, recID, ttCode);
                    break;
                case 2: operLinkStr += loadExcel(url, recID, ttCode);
                    break;
            }
            switch (isUsed)
            {
                case "0": operLinkStr += rightEnable(recID);
                    break;
                case "1": operLinkStr += rightDisable(recID);
                    break;
                case "-1":
                    break;
            }

            return operLinkStr;
        }

        //下载
        private string loadExcel(string url, string recid, string ttcode)
        {
            string path = "/upload/" + BLL.Util.GetUploadProject(BLL.Util.ProjectTypePath.Template, "/");
            url = path + url;
            return "<a href='' onclick=\"DownLoadExcel('" + url + "','" + recid + "','" + ttcode + "',this)\">下载模板</a>&nbsp;";
        }

        //编辑按钮权限：有按钮权限 
        private string rightEdit(string ttCode)
        {
            string rightStr = string.Empty;

            if (right_edit)
            {
                rightStr = "<a href='/TemplateManagement/TemplateEdit.aspx?ttcode=" + ttCode + "' target='_blank' name='a_edit'>编辑</a>&nbsp;";
            }
            return rightStr;
        }

        //删除按钮权限：有功能权限 
        private string rightDelete(string recID)
        {
            string rightStr = string.Empty;

            if (right_delete)
            {
                rightStr = "<a href='javascript:deleteTemplate(" + recID + ")'   name='a_delete'>删除</a>&nbsp;";
            }
            return rightStr;
        }

        //生成模板：
        private string rightTemplate(string recID, string ttCode)
        {
            string rightStr = string.Empty;

            if (right_generate)
            {
                rightStr = "<a href='javascript:void(0);' onclick='javascript:GenerateTemplate(" + recID + ",\"" + ttCode + "\")' name='aNewTemplate'>生成模板</a>&nbsp;";
            }
            return rightStr;
        }

        //启用按钮权限：有功能权限 
        private string rightEnable(string recID)
        {
            string rightStr = string.Empty;

            if (right_enable)
            {
                rightStr = "<a href='javascript:enableTemplate(" + recID + ")'   name='a_enable'>启用</a>&nbsp;";
            }
            return rightStr;
        }

        //停用按钮权限：有功能权限 
        private string rightDisable(string recID)
        {
            string rightStr = string.Empty;

            if (right_disable)
            {
                rightStr = "<a href='javascript:disableTemplate(" + recID + ")'   name='a_disable'>停用</a>&nbsp;";
            }
            return rightStr;
        }

    }
}