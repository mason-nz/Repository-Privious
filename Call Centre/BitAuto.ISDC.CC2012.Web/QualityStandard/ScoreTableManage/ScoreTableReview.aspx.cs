using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BitAuto.ISDC.CC2012.Web.QualityStandard.ScoreTableManage
{
    public partial class ScoreTableReview : BitAuto.ISDC.CC2012.Web.Base.PageBase
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
        public bool right_export = false;
        public string TableName;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                //判断当前登录人是否有评分表复审权限
                right_export = BLL.Util.CheckRight(BLL.Util.GetLoginUserID(), "SYS024BUT600202");
                if (!right_export)
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('您没有评分表审核权限！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
                else
                {

                    //
                    if (!string.IsNullOrEmpty(QS_RTID))
                    {



                        int intiy = 0;
                        if (int.TryParse(QS_RTID, out intiy))
                        {
                            //判断单据状态，如果不是待复审，不能复审
                            Entities.QS_RulesTable QS_RulesTableModel = BLL.QS_RulesTable.Instance.GetQS_RulesTable(intiy);
                            if (QS_RulesTableModel.Status == 10002)
                            {
                                TableName = QS_RulesTableModel.Name;
                                if (QS_RulesTableModel.ScoreType == 2)
                                {
                                    TableName += "（致命项数" + QS_RulesTableModel.DeadItemNum + ",非致命项数" + QS_RulesTableModel.NoDeadItemNum + "）";
                                }

                                this.ScoreTableViewID.Qs_RTID = intiy;
                            }
                            else
                            {
                                Response.Write(@"<script language='javascript'>javascript:alert('该评分表不是待复审状态！');try {
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
}