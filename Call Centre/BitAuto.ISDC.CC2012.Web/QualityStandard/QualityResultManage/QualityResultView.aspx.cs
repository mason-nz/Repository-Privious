using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.QualityResultManage
{
    public partial class QualityResultView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {

        public string QS_RID
        {
            get
            {
                return HttpContext.Current.Request["QS_RID"] == null ? string.Empty :
                HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QS_RID"].ToString());
                //return "16";
            }
        }
        public string TableName;
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


                    int intiy = 0;
                    if (int.TryParse(QS_RID, out intiy))
                    {
                        Entities.QS_Result model = BLL.QS_Result.Instance.GetQS_Result(intiy);
                        if (model != null)
                        {
                            Entities.QS_RulesTable QS_RulesTableModel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(model.QS_RTID);
                            if (QS_RulesTableModel != null)
                            {
                                TableName = QS_RulesTableModel.Name;
                                if (QS_RulesTableModel.ScoreType == 2)
                                {
                                    TableName += "（致命项数" + QS_RulesTableModel.DeadItemNum + ",非致命项数" + QS_RulesTableModel.NoDeadItemNum + "）";
                                }
                            }
                            this.QualityStandardViewID.QS_RTID = model.QS_RTID;
                            this.QualityStandardViewID.QS_RID = model.QS_RID;
                            this.QualityStandardViewID.PageFrom = PageFrom;
                            UCCallRecordView1.QS_RID = model.QS_RID;
                            UCCallRecordView1.CallID = model.CallID;
                            UCCallRecordView1.tableEndName = tableEndName;                            
                        }
                        else
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('评分成绩不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                        }

                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>javascript:alert('评分成绩主键数据格式不正确！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");

                    }
                }
                else
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('请求参数错误！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
            }
        }
    }
}