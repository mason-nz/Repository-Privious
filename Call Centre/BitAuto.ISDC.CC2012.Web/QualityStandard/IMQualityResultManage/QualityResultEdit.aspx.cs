using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.Utils.Config;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.IMQualityResultManage
{
    public partial class QualityResultEdit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //评分表主键
        private int qs_rtid = 0;
        public int QS_RTID
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["QS_RTID"]))
                {
                    return qs_rtid;
                }
                else
                {
                    if (int.TryParse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QS_RTID"].ToString()), out qs_rtid))
                    {
                    }
                    return qs_rtid;
                }
            }
        }
        //评分成绩表主键
        private int qs_rid = 0;
        public int QS_RID
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["QS_RID"]))
                {
                    return qs_rid;
                }
                else
                {
                    if (int.TryParse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QS_RID"].ToString()), out qs_rid))
                    {
                    }
                    return qs_rid;
                }
            }
            set
            {
                qs_rid = value;
            }
        }
        //对话ID
        private int csid = 0;
        public int CSID
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["CSID"]))
                {
                    return csid;
                }
                else
                {
                    if (int.TryParse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CSID"].ToString()), out csid))
                    {
                    }
                    return csid;
                }
            }
        }

        //标题
        public string TableName { get; set; }
        public int userId;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024BUT600405"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                if (QS_RTID != 0 && CSID != 0)
                {
                    if (QS_RID <= 0)
                    {
                        QS_RID = BLL.QS_IM_Result.Instance.GetRidByCsidAndRtid(QS_RTID, CSID);
                    }
                    DataRow dr = BLL.QS_IM_Result.Instance.GetQS_IM_ResultForCSID(CSID.ToString());
                    Entities.QS_RulesTable tablemodel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(QS_RTID);
                    if (tablemodel != null && dr != null)
                    {
                        //设置标题
                        TableName = tablemodel.Name;
                        if (tablemodel.ScoreType == 2)
                        {
                            TableName += "（致命项数" + tablemodel.DeadItemNum + "，非致命项数" + tablemodel.NoDeadItemNum + "）";
                        }
                        //状态校验
                        int Result_Status = CommonFunction.ObjectToInteger(dr["Result_Status"]);
                        if (Result_Status == (int)QSResultStatus.WaitScore)
                        {
                            QualityStandardEditID.QS_RTID = QS_RTID;
                            QualityStandardEditID.QS_RID = QS_RID;
                            QualityStandardEditID.CallID = -1;
                            QualityStandardEditID.CSID = CSID;
                            UCConversationsViewID.CSID = CSID.ToString();
                        }
                        else
                        {
                            AlertMessage("当前评分成绩不是待评分状态");
                        }
                    }
                    else
                    {
                        AlertMessage("评分表不存在或对话不存在");
                    }
                }
                else
                {
                    AlertMessage("页面请求参数不正确");
                }
            }
        }

        private void AlertMessage(string msg)
        {
            Response.Write(@"<script language='javascript'>javascript:alert('" + msg + "！');try {window.external.MethodScript('/browsercontrol/closepage');} catch (e) {window.opener = null; window.open('', '_self'); window.close();};</script>");
        }

        public string GetIMUrl()
        {
            return ConfigurationUtil.GetAppSettingValue("PersonalIMURL") + "/ConversationHistoryForCCDetail.aspx";
        }
    }
}