using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage
{
    public partial class ScoreTableView : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        public string QS_RTID
        {
            get
            {
                return HttpContext.Current.Request["QS_RTID"] == null ? string.Empty :
                        HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["QS_RTID"].ToString());
                //return "2";
            }
        }
        public string TableName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                if (!string.IsNullOrEmpty(QS_RTID))
                {
                    int intiy = 0;
                    if (int.TryParse(QS_RTID, out intiy))
                    {
                        Entities.QS_RulesTable tableModel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(intiy);
                        if (tableModel != null)
                        {
                            TableName = tableModel.Name;
                            if (tableModel.ScoreType == 2)
                            {
                                TableName += "（致命项数" + tableModel.DeadItemNum + ",非致命项数" + tableModel.NoDeadItemNum + "）";
                            }
                            this.ScoreTableViewID.Qs_RTID = intiy;
                        }
                        else
                        {
                            Response.Write(@"<script language='javascript'>javascript:alert('评分表不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                        }
                    }
                    else
                    {
                        Response.Write(@"<script language='javascript'>javascript:alert('评分表主键数据格式不正确！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");

                    }
                }
                else
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('评分表不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
            }
        }
    }
}