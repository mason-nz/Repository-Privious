using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.IMQualityResultManage
{
    public partial class QualityResultView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        /// 成绩表ID
        /// <summary>
        /// 成绩表ID
        /// </summary>
        public string QS_RID
        {
            get
            {
                return HttpContext.Current.Request["QS_RID"] == null ? string.Empty :
                HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QS_RID"].ToString());
            }
        }

        public string TableName { get; set; }
        /// <summary>
        /// 质检对象类型：cc*（录音）/im（对话）
        /// </summary>
        public string PageFrom
        {
            get
            {
                return HttpContext.Current.Request["pagefrom"] == null ? string.Empty :
                HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["pagefrom"].ToString());
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                if (!string.IsNullOrEmpty(QS_RID))
                {
                    QS_IM_ResultInfo info = CommonBll.Instance.GetComAdoInfo<QS_IM_ResultInfo>(CommonFunction.ObjectToInteger(QS_RID));
                    if (info != null)
                    {
                        int QS_RTID = info.ValueOrDefault_QS_RTID;
                        int CSID = (int)info.ValueOrDefault_CSID;
                        Entities.QS_RulesTable tablemodel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(QS_RTID);
                        if (tablemodel != null)
                        {
                            //设置标题
                            TableName = tablemodel.Name;
                            if (tablemodel.ScoreType == 2)
                            {
                                TableName += "（致命项数" + tablemodel.DeadItemNum + "，非致命项数" + tablemodel.NoDeadItemNum + "）";
                            }
                            //设置页面
                            QualityStandardViewID.QS_RTID = QS_RTID;
                            QualityStandardViewID.PageFrom = PageFrom;
                            QualityStandardViewID.QS_RID = CommonFunction.ObjectToInteger(QS_RID);
                            UCConversationsViewID.CSID = CSID.ToString();
                        }
                    }
                    else
                    {
                        AlertMessage("评分成绩不存在");
                    }
                }
                else
                {
                    AlertMessage("请求参数错误");
                }
            }
        }
        private void AlertMessage(string msg)
        {
            Response.Write(@"<script language='javascript'>javascript:alert('" + msg + "！');try {window.external.MethodScript('/browsercontrol/closepage');} catch (e) {window.opener = null; window.open('', '_self'); window.close();};</script>");
        }
    }
}