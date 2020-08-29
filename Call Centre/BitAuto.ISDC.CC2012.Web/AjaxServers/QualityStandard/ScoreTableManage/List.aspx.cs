using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.QualityScoring.ScoreTableManage
{
    public partial class List : PageBase
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

        public bool right_edit = false;
        public bool right_delete = false;
        public bool right_audit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                userID = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userID, "SYS024MOD6202")) 
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                right_edit = BLL.Util.CheckRight(userID, "SYS024BUT600201");
                right_delete = BLL.Util.CheckRight(userID, "SYS024BUT600203");
                right_audit = BLL.Util.CheckRight(userID, "SYS024BUT600202");

                BindData();
            }
        }

        private void BindData()
        {
            Entities.QueryQS_RulesTable query = new Entities.QueryQS_RulesTable();
            string errMsg = string.Empty;
            BLL.ConverToEntitie<Entities.QueryQS_RulesTable> conver = new BLL.ConverToEntitie<Entities.QueryQS_RulesTable>(query);
            errMsg = conver.Conver(JsonStr);

            if (errMsg != "")
            {
                return;
            }
            query.LoginID = userID;
            int RecordCount = 0;

            DataTable dt = BLL.QS_RulesTable.Instance.GetQS_RulesTable(query, " QS_RulesTable.CreateTime desc ", BLL.PageCommon.Instance.PageIndex, PageSize, out RecordCount);
            repeaterTableList.DataSource = dt;
            repeaterTableList.DataBind();

            AjaxPager.PageSize = 20;
            AjaxPager.InitPager(RecordCount);
        }

        public string oper(string status, string RTID, string createrID)
        {
            string operStr = string.Empty;
            int _status;
            int _rtid = 0;
            int _createrID = int.Parse(createrID);

            if (status != "" && !int.TryParse(RTID, out _rtid))
            {
                return operStr;
            }
            _status = int.Parse(status);
            switch (_status)
            {
                case (int)Entities.QSRulesTableStatus.Unfinished:
                    operStr = edit(_rtid, _createrID) + delete(_rtid, _createrID);
                    break;
                case (int)Entities.QSRulesTableStatus.Audit:
                    operStr = audit(_rtid);
                    break;
                case (int)Entities.QSRulesTableStatus.Finished:
                    break;
            }

            return operStr;
        }

        /// <summary>
        /// 增加使用状态参数，只有在 状态：未完成,或 状态：已完成 且 使用状态：未使用 两种情况下再根据功能点判断是否显示
        /// 编辑、删除链接
        /// </summary>
        /// <param name="status"></param>
        /// <param name="statusInUse"></param>
        /// <param name="RTID"></param>
        /// <param name="createrID"></param>
        /// <returns></returns>
        public string oper(string status, string statusInUse, string RTID, string createrID)
        {
            string operStr = string.Empty;
            int _status;
            int _rtid = 0;
            int _createrID = int.Parse(createrID);

            if (status != "" && !int.TryParse(RTID, out _rtid))
            {
                return operStr;
            }
            _status = int.Parse(status);
            switch (_status)
            {
                case (int)Entities.QSRulesTableStatus.Unfinished:
                    operStr = edit(_rtid, _createrID) + delete(_rtid, _createrID);
                    break;
                case (int)Entities.QSRulesTableStatus.Audit:
                    operStr = audit(_rtid);
                    break;
                case (int)Entities.QSRulesTableStatus.Finished:
                    if (statusInUse == "0")//状态：已完成 使用状态：未使用 情况下可以编辑、删除
                    {
                        operStr = edit(_rtid, _createrID) + delete(_rtid, _createrID);
                    }
                    break;
            }

            return operStr;
        }
        //编辑
        private string edit(int RTID, int createrID)
        {
            if (right_edit && createrID == userID)
            {
                return "<a href=\"/QualityStandard/ScoreTableManage/EditScoreTable.aspx?QS_RTID=" + RTID + "\" target='_blank'>编辑</a>&nbsp;";
            }
            else
            {
                return "";
            }
        }

        //审核
        private string audit(int RTID)
        {
            if (right_audit)
            {
                return "<a href=\"/QualityStandard/ScoreTableManage/ScoreTableReview.aspx?QS_RTID=" + RTID + "\" target='_blank'>审核</a>&nbsp;";
            }
            else
            {
                return "";
            }
        }

        //删除
        private string delete(int RTID, int createrID)
        {
            if (right_delete && createrID == userID)
            {
                return "<a onclick='javascript:deleteRulesTable(" + RTID + ")' href=\"javascript:void(0);\">删除</a>&nbsp;";
            }
            else
            {
                return "";
            }
        }

    }
}