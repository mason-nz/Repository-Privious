using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;

namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject
{
    public partial class ExamProjectListAjax : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// <summary>
        /// 列表查询条件，所在分组ID
        /// </summary>
        private int RequestBGID
        {
            get { return BLL.Util.GetCurrentRequestInt("BGIDS"); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
                int userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD3605"))//	添加试题或为知识点管理
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
        }

        public string currentPageNum = "1";

        #region 数据列表绑定
        public void BindData()
        {
            string where = " ";

            string ObjName = HttpUtility.UrlDecode(Request["ObjName"].ToString());
            if (ObjName != "")
            {
                where += " and ExamInfo.Name like '%" + BitAuto.Utils.StringHelper.SqlFilter(ObjName) + "%'";
            }
            string Group = HttpUtility.UrlDecode(Request["Group"].ToString());
            if (Group != "")
            {
                where += " and ExamInfo.businessGroup like '%" + BitAuto.Utils.StringHelper.SqlFilter(Group) + "%'";
            }
            string StartTime = HttpUtility.UrlDecode(Request["StartTime"].ToString());
            if (StartTime != "")
            {
                where += " and ExamInfo.ExamStartTime > '" + StartTime + "'";
            }
            string EndTime = HttpUtility.UrlDecode(Request["EndTime"].ToString());
            if (EndTime != "")
            {
                where += " and ExamInfo.ExamEndTime < '" + EndTime + "'";
            }
            string IsMakeUp = HttpUtility.UrlDecode(Request["IsMakeUp"].ToString());
            if (IsMakeUp != "")
            {
                where += " and ExamInfo.IsmakeUp = " + BitAuto.Utils.StringHelper.SqlFilter(IsMakeUp);
            }
            string CreateUserID = HttpUtility.UrlDecode(Request["CreateUserID"].ToString());
            if (CreateUserID != "-1")
            {
                where += " and ExamInfo.CreaetUserID in (" + BLL.Util.SqlFilterByInCondition(CreateUserID) + ")";
            }
            string ECID = HttpUtility.UrlDecode(Request["Cate"].ToString());
            if (ECID != "")
            {
                where += " and ExamInfo.ECID in (" + BLL.Util.SqlFilterByInCondition(ECID) + ")";
            }
            string State = HttpUtility.UrlDecode(Request["State"].ToString());
            if (State != "")
            {
                where += " and (1=2";
                string[] stateArr = State.Split(',');

                foreach (string state in stateArr)
                {
                    switch (Convert.ToInt32(state))
                    {
                        case 1: where += " or ExamInfo.ExamStartTime > '" + DateTime.Now + "'"; break;
                        case 2: where += " or (ExamInfo.ExamStartTime < '" + DateTime.Now + "' and ExamInfo.Status<>1)"; break;
                        case 3: where += " or ExamInfo.Status=1"; break;
                    }
                }
                where += ")";
            }
            if (RequestBGID > 0)
            {
                where += " and ExamInfo.BGID = " + RequestBGID;    
            }
            
           
            int RecordCount = 0;
            DataTable dt = BLL.ExamInfo.Instance.GetExamInfo2(where, "ExamInfo.CreateTime DESC", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);

            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("State", typeof(string));
            dt.Columns.Add("Contral", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                row["UserName"] = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(row["CreaetUserID"]));

                if (row["Status"].ToString() == "1")
                {
                    row["State"] = "已完成";
                    row["Contral"] = "<a target='_blank' href='ExamProjectView.aspx?eiid="
                        + row["EIID"].ToString() + "'>查看</a>&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                else if (Convert.ToDateTime(row["ExamStartTime"]) > DateTime.Now)
                {
                    row["State"] = "未开始";//OpenAddPageInfo()
                    row["Contral"] = "<a target='_blank' href='ExamProjectView.aspx?eiid="
                        + row["EIID"].ToString() + "'>查看</a>&nbsp;&nbsp;&nbsp;&nbsp;"
                        + "<a target='_blank' href='/ExamOnline/ExamObject/ExamProjectEdit.aspx?id="
                        + row["EIID"].ToString() + "'>编辑</a>&nbsp;&nbsp;&nbsp;&nbsp;"
                        + "<a href='javascript:void(0)' onclick='javascript:delExamInfo(this,"
                        + row["EIID"].ToString() + ",-1)'>删除</a>";
                }
                else
                {
                    row["State"] = "进行中";
                    string editStr = "";
                    Entities.MakeUpExamInfo makeExamInfo = null;
                    if (row["IsMakeUp"].ToString() == "1")
                    {
                        makeExamInfo = BLL.MakeUpExamInfo.Instance.GetMakeUpExamInfoByEIID(Convert.ToInt32(row["EIID"]));

                    }
                    if ((makeExamInfo != null && makeExamInfo.MakeupExamEndTime > DateTime.Now) || makeExamInfo == null)
                    {
                        //没有补考或者补考还没有结束的，就可以编辑

                        editStr = "<a  target='_blank' href='/ExamOnline/ExamObject/ExamProjectEdit.aspx?id="
                                + row["EIID"].ToString() + "'>编辑</a>&nbsp;&nbsp;&nbsp;&nbsp;";
                    }

                    row["Contral"] = "<a href='javascript:void(0)' onclick='javascript:delExamInfo(this,"
                        + row["EIID"].ToString() + ",1)'>完成</a>&nbsp;&nbsp;&nbsp;&nbsp;"
                        + editStr
                        + "<a target='_blank' href='ExamProjectView.aspx?eiid="
                        + row["EIID"].ToString() + "'>查看</a>&nbsp;&nbsp;&nbsp;&nbsp;";
                }
            }
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 10, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
            currentPageNum = BLL.PageCommon.Instance.PageIndex.ToString();
        }
        #endregion
    }
}