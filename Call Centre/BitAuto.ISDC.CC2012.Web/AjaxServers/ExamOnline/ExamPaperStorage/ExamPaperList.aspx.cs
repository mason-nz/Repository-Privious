using System;
using System.Web;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.ExamPaperStorage
{
    public partial class ExamPaperList : PageBase
    {

        #region 属性

        /// <summary>
        /// 所属分组
        /// </summary>
        private string BGID
        {
            get { return Request["bgid"] == null ? "" : HttpUtility.UrlDecode(Request["bgid"].ToString()); }
        }

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
        /// <summary>
        /// 分类ID串
        /// </summary>
        private string ECIDStr
        {
            get { return Request["ECIDStr"] == null ? "" : HttpUtility.UrlDecode(Request["ECIDStr"].ToString()); }
        }
        /// <summary>
        /// 创建开始时间
        /// </summary>
        private string BeginTime
        {
            get { return Request["BeginTime"] == null ? "" : HttpUtility.UrlDecode(Request["BeginTime"].ToString()); }
        }

        /// <summary>
        /// 创建结束时间
        /// </summary>
        private string EndTime
        {
            get { return Request["EndTime"] == null ? "" : HttpUtility.UrlDecode(Request["EndTime"].ToString()); }
        }

        private string State
        {
            get { return Request["State"] == null ? "" : HttpUtility.UrlDecode(Request["State"].ToString()); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        private string CreateUser
        {
            get { return Request["CreateUser"] == null ? "" : HttpUtility.UrlDecode(Request["CreateUser"].ToString()); }
        }

        #endregion

        public int PageSize = BLL.PageCommon.Instance.PageSize = 10;
        public int GroupLength = 8;
        public int RecordCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userID = BLL.Util.GetLoginUserID();

                if (!BLL.Util.CheckRight(userID, "SYS024MOD3604"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                else
                {
                    BindData();
                }
            }
        }

        private void BindData()
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
            if (Catage != string.Empty && Catage != "-1")
            {
                query.ECID = int.Parse(Catage);
            }
            if (ECIDStr != string.Empty)
            {
                query.ECIDStr = ECIDStr;
            }
            if (BeginTime != string.Empty && DateTime.TryParse(BeginTime, out dateVal))
            {
                query.CreateBeginTime = dateVal;
            }
            if (EndTime != string.Empty && DateTime.TryParse(EndTime, out dateVal))
            {
                query.CreateEndTime = dateVal;
            }
            if (State != string.Empty)
            {
                query.Status = State;
            }
            if (BGID != string.Empty)
            {
                int _bgid = 0;
                if (int.TryParse(BGID, out _bgid))
                {
                    query.BGID = _bgid;
                }
            }
            if (CreateUser != string.Empty && CreateUser != "-1")
            {
                query.CreaetUserID = CreateUser;
            }

            #endregion

            DataTable dt = BLL.ExamPaper.Instance.GetExamPaper(query, "CreateTime DESC", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            if (dt != null && dt.Rows.Count > 0)
            {
                repeaterTableList.DataSource = dt;
                repeaterTableList.DataBind();
                AjaxPager_Custs.PageSize = PageSize;
                AjaxPager_Custs.InitPager(RecordCount);
            }
        }

        /// <summary>
        /// 根据ID取状态名称
        /// </summary>
        /// <param name="stataID"></param>
        /// <returns></returns>
        public string GetStataByID(string stataID)
        {
            string stataName = "";
            int intval = 0;
            if (int.TryParse(stataID, out intval))
            {
                stataName = BLL.Util.GetEnumOptText(typeof(BitAuto.ISDC.CC2012.Entities.ExamPaperState), intval);
            }
            return stataName;
        }

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

        //创建人
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

        /// <summary>
        /// 获取操作按钮HTML
        /// </summary>
        /// <param name="status"></param>
        /// <param name="epid"></param>
        /// <returns></returns>
        public string GetButtonHtml(string status, string epid)
        {
            string html = "";
            if (status == "0" || status == "2")
            {
                html += "&nbsp;&nbsp;<a href='/ExamOnline/ExamPaperStorage/ExamPaperEdit.aspx?epid=" + epid + "' target='_blank'>编辑</a>";
                int int_epid;
                if (int.TryParse(epid.Trim(), out int_epid))
                {
                    if (BLL.ExamInfo.Instance.GetExamPaperUsedCount(int_epid) == 0)
                    {
                        html += "&nbsp;&nbsp;<a href='#'  name='delPager' epid= '" + epid + "' target='_blank'>删除</a>";
                    }
                }
                else
                {
                    html += "&nbsp;&nbsp;<a href='#'  name='delPager' epid= '" + epid + "' target='_blank'>删除</a>";
                }
                
            }
            else if (status == "1")
            {
                html += "&nbsp;&nbsp;<a href='/ExamOnline/ExamPaperStorage/ExamPaperView.aspx?epid=" + epid + "' target='_blank'>查看</a>";
            }

            //未使用，已使用的试卷可以产生PDF文件。
            if (status == "0" || status == "1")
            {
                html += "&nbsp;&nbsp;<a href='/ExamOnline/ExamPaperStorage/ExamPaperPDF.aspx?paper=1&epid=" + epid + "' target='_self'>导出PDF</a>";
            }
            return html;
        }
    }
}