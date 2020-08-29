using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.QualityResultManage
{
    public partial class QualityResultEdit : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //评分表主键
        //private int qs_rtid = 2;
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
        //private int qs_rid = 16;
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

        private Int64 callid = 0;
        public Int64 CallID
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["CallID"]))
                {
                    return callid;
                }
                else
                {
                    if (Int64.TryParse(HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["CallID"].ToString()), out callid))
                    {
                    }
                    return callid;
                }
            }
        }

        //表名后缀
        public string tableEndName
        {
            get
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request["tableEndName"]))
                {
                    return "";
                }
                else
                {
                    return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["tableEndName"].ToString());
                }
            }
        }

        public string TableName;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int userId = BLL.Util.GetLoginUserID();
                if (!BLL.Util.CheckRight(userId, "SYS024BUT600104"))
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
                if (QS_RTID != 0 && CallID != 0)
                {
                    if (QS_RID <= 0)
                    {
                        QS_RID = BLL.QS_Result.Instance.GetRidByCallidAndRtid(QS_RTID, CallID);
                    }

                    Entities.QS_RulesTable tablemodel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(QS_RTID);
                    //由于易湃外呼业务录音不入CC的CallRecordInfo表，所以取CallRecord_ORIG_Business来判断录音是否存在 modify by qizq 2014-4-17
                    //Entities.CallRecordInfo callmodel = BLL.CallRecordInfo.Instance.GetCallRecordInfoByCallID(CallID);
                    Entities.CallRecord_ORIG_Business callmodel = BLL.CallRecord_ORIG_Business.Instance.GetByCallID(CallID, tableEndName);
                    if (tablemodel != null && callmodel != null)
                    {
                        TableName = tablemodel.Name;
                        if (tablemodel.ScoreType == 2)
                        {
                            TableName += "（致命项数" + tablemodel.DeadItemNum + "，非致命项数" + tablemodel.NoDeadItemNum + "）";
                        }
                        bool flag = true;
                        if (QS_RID != 0)
                        {
                            Entities.QS_Result modelQS_Result = BLL.QS_Result.Instance.GetQS_Result(QS_RID);
                            if (modelQS_Result != null && modelQS_Result.Status == (Int32)Entities.QSResultStatus.WaitScore)
                            {

                            }
                            else
                            {
                                flag = false;
                            }

                        }
                        if (flag)
                        {

                            QualityStandardEditID.QS_RTID = QS_RTID;
                            QualityStandardEditID.QS_RID = QS_RID;
                            QualityStandardEditID.CallID = CallID;
                            UCCallRecordView1.QS_RID = QS_RID;
                            UCCallRecordView1.CallID = CallID;
                            UCCallRecordView1.tableEndName = tableEndName;
                        }
                        else
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('当前评分成绩不是待评分状态！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                        }
                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>javascript:alert('评分表不存在或录音不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                    }
                }
                else
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('页面请求参数不正确！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
            }
        }
    }
}