using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using BitAuto.Services.Organization.Remoting;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.BLL;
using BitAuto.ISDC.CC2012.Entities;
namespace BitAuto.ISDC.CC2012.Web.ExamOnline.ExamObject
{
    public partial class GetExamPaperList : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        #region 定义属性
        public int pageSize = 10;
        public int RecordCount;
        public int GroupLength = 8;
        //public int RecordCount = 0;
        #region 属性

        /// <summary>
        /// 试卷名称
        /// </summary>
        private string PaperName
        {
            get { return Request["PaperName"] == null ? "" : HttpUtility.UrlDecode(Request["PaperName"].ToString()); }
        }

        /// <summary>
        /// 分类
        /// </summary>
        private string Catage
        {
            get { return Request["Catage"] == null ? "" : HttpUtility.UrlDecode(Request["Catage"].ToString()); }
        }

        private string State
        {
            get { return "0,1"; }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        private string CreateUser
        {
            get { return Request["CreateUser"] == null ? "" : HttpUtility.UrlDecode(Request["CreateUser"].ToString()); }
        }

        private string BGID
        {
            get { return Request["BGID"] == null ? BLL.EmployeeSuper.Instance.GetEmployeeAgent(BLL.Util.GetLoginUserID()).Rows[0]["BGID"].ToString() : HttpUtility.UrlDecode(Request["BGID"].ToString()); }
        }
        #endregion
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ExamPaperBindData();
            if (!IsPostBack)
            {
                CateBind();

                rp_BGData.DataSource = BLL.EmployeeSuper.Instance.GetCurrentUserGroups(BLL.Util.GetLoginUserID());
                rp_BGData.DataBind();

                //LoadSelectedEmployees();
            }
        }
        #endregion

        #region 绑定已经选择的试卷
        public string LoadSelectedEmployees()
        {
            string selectedStr = "";
            int ExamPaperID = 0;
            if (Request.QueryString["ExamPaperID"] == null)
            {
                return "";
            }
            else
            {
                if (int.TryParse(Request.QueryString["ExamPaperID"].ToString(), out ExamPaperID))
                {
                    Entities.ExamPaper examPaper = new Entities.ExamPaper();
                    examPaper = BLL.ExamPaper.Instance.GetExamPaper(ExamPaperID);

                    selectedStr += "<tr id='tr_" + examPaper.EPID.ToString() + "'>"
                            + "<td><a href='javascript:delSelectedEmployee(" + examPaper.EPID.ToString() + ")'><img title='删除' src='/Images/close.png'></a></td>"
                            + "<td>" + examPaper.Name + "</td>"
                            + "<td><input id='hdn_" + examPaper.EPID.ToString() + "' type='hidden' value=" + examPaper.EPID.ToString()
                            + " />" + GetCatageName(examPaper.ECID.ToString()) + "</td>"
                            + "<td>" + getCreateUserName(examPaper.CreaetUserID.ToString()) + "</td>"
                            + "<td>" + DateTime.Parse(examPaper.CreateTime.ToString()).ToString("yyyy-MM-dd") + "</td></tr>";
                }
            }
            return selectedStr;
        }
        #endregion

        #region 绑定试卷分类
        public void CateBind()
        {
            DataTable dt = new DataTable();
            QueryExamCategory query = new QueryExamCategory();
            query.Type = 2;
            int total = 0;
            dt = BLL.ExamCategory.Instance.GetExamCategory(query, " CreateTime", 1, 10, out total);
            Rpt.DataSource = dt;
            Rpt.DataBind();
        }
        #endregion

        #region 绑定试卷列表数据
        private void ExamPaperBindData()
        {
            Entities.QueryExamPaper query = new Entities.QueryExamPaper();

            #region 条件

            int intVal = 0;
            DateTime dateVal = new DateTime();

            if (PaperName != string.Empty)
            {
                query.Name = PaperName;
            }
            if (Catage != string.Empty && int.TryParse(Catage, out intVal))
            {
                query.ECID = intVal;
            }
            if (Catage != "")
            {
                query.ECIDStr = Catage;
            }
            if (State != string.Empty)
            {
                query.Status = State;
            }
            if (CreateUser != string.Empty && CreateUser != "-1")
            {
                query.CreaetUserID = CreateUser;
            }
            if (!string.IsNullOrEmpty(BGID))
            {
                query.BGID = int.Parse(BGID);
            }
            #endregion

            DataTable dt = BLL.ExamPaper.Instance.GetExamPaper(query, "CreateTime DESC", BLL.PageCommon.Instance.PageIndex, pageSize, out RecordCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                Rpt_ExamPaper.DataSource = dt;
                Rpt_ExamPaper.DataBind();
                litPagerDown.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), GroupLength, RecordCount, pageSize, BLL.PageCommon.Instance.PageIndex, 1);
            }
        }
        #endregion

        #region 根据分类ID获取分类名称
        /// <summary>
        /// 根据分类ID获取分类名称
        /// </summary>
        /// <param name="PagerID"></param>
        /// <returns></returns>
        public string GetCatageName(string PagerID)
        {
            string name = "";
            int catageID = 0;
            if (int.TryParse(PagerID, out catageID))
            {
                Entities.ExamCategory model = BLL.ExamCategory.Instance.GetExamCategory(catageID);
                if (model != null)
                {
                    name = model.Name;
                }
            }
            return name;
        }
        #endregion

        #region 试卷创建人
        public string getCreateUserName(string userID)
        {
            string userName = string.Empty;
            int _userID;
            if (int.TryParse(userID, out _userID))
            {
                userName = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(_userID);
            }
            return userName;

        }
        #endregion
    }
}