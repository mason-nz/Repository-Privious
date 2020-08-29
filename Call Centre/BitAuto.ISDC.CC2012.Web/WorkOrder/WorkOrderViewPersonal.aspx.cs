using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Configuration;

namespace BitAuto.ISDC.CC2012.Web.WorkOrder
{
    public partial class WorkOrderViewPersonal : BitAuto.ISDC.CC2012.Web.Base.PageBase
    {
        //取工单ＩＤ
        public string CategoryID = string.Empty;
        public string WorkOrderID
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("OrderID");
            }
        }
        public string CustID = string.Empty;
        public string CHUrl = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(WorkOrderID))
                {
                    Response.Write(@"<script language='javascript'>javascript:alert('工单ID不能为空！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                }
                else
                {
                    Entities.WorkOrderInfo model = BLL.WorkOrderInfo.Instance.GetWorkOrderInfo(WorkOrderID);
                    if (model == null)
                    {
                        Response.Write(@"<script language='javascript'>javascript:alert('工单不存在！');try {
                 window.external.MethodScript('/browsercontrol/closepage');
            } catch (e) {
                window.opener = null; window.open('', '_self'); window.close();
            };</script>");
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(WorkOrderID) && BLL.WorkOrderInfo.Instance.HasConversation(WorkOrderID))
                        {
                            string imurl = ConfigurationManager.AppSettings["PersonalIMURL"].ToString();
                            int thisuserid = BLL.Util.GetLoginUserID();
                            CHUrl = "<a href='javascript:void(0)' onclick=\"GotoConversation(this,'" + imurl + "/ConversationHistoryForCC.aspx','" + thisuserid + "','','" + WorkOrderID + "')\">关联对话</a>";
                        }

                        CategoryID = model.CategoryID.ToString();
                        this.lblTitle.Text = model.Title;
                        this.lblDataSource.Text = BLL.Util.GetEnumOptText(typeof(Entities.WorkOrderDataSource), Convert.ToInt32(model.DataSource));
                        if (model.IsReturnVisit == 1)
                        {
                            this.lblAcceptVisit.Text = "接受";
                        }
                        else
                        {
                            this.lblAcceptVisit.Text = "不接受";
                        }
                        this.lblAddPerson.Text = BitAuto.YanFa.SysRightManager.Common.UserInfo.GerTrueName(Convert.ToInt32(model.CreateUserID)) + "【" + BitAuto.YanFa.Crm2009.BLL.UserInfo.Instance.GetDepartNameByUserID(Convert.ToInt32(model.CreateUserID)) + "】";
                        this.lblAddTime.Text = Convert.ToDateTime(model.CreateTime).ToString("yyyy年MM月dd日HH:mm");
                        this.lblCategory.Text = BitAuto.ISDC.CC2012.BLL.WorkOrderCategory.Instance.GetCategoryFullName(model.CategoryID.ToString());
                        //推荐活动，如果工单表没有值 则去WorkOrderActivity表找 add lxw 13.1.15
                        try
                        {
                            if (string.IsNullOrEmpty(model.NominateActivity))
                            {
                                int count = 0;
                                DataTable dt_Activity = BLL.WorkOrderActivity.Instance.GetWorkOrderActivity(" and orderID='" + WorkOrderID + "'", "", 1, 1000, out count);
                                string[] guids = new string[dt_Activity.Rows.Count];
                                for (int i = 0; i < dt_Activity.Rows.Count; i++)
                                {
                                    guids[i] = dt_Activity.Rows[i]["ActivityGUID"].ToString();
                                }
                                if (dt_Activity.Rows.Count > 0)
                                {
                                    WebService.Market.MarketServiceHelper market = new WebService.Market.MarketServiceHelper();
                                    DataSet ds_Activity = market.GetDataXml(guids);
                                    DataTable dt_webServiceActivity = null;
                                    if (ds_Activity != null && ds_Activity.Tables.Count > 0)
                                    {
                                        dt_webServiceActivity = ds_Activity.Tables[0];
                                        string activityStr = string.Empty;
                                        for (int k = 0; k < dt_Activity.Rows.Count; k++)
                                        {
                                            string guid = dt_Activity.Rows[k]["ActivityGUID"].ToString();
                                            DataRow[] drs = dt_webServiceActivity.Select("Guid='" + guid + "'");
                                            if (drs.Length > 0)
                                            {
                                                string url = drs[0]["url"].ToString();
                                                if (!string.IsNullOrEmpty(url))
                                                {
                                                    activityStr += "<a href=" + url + " target='_blank'>" + dt_webServiceActivity.Rows[k]["Name"].ToString() + "</a>&nbsp;，";
                                                }
                                                else
                                                {
                                                    activityStr += dt_webServiceActivity.Rows[k]["Name"].ToString() + "&nbsp;，";
                                                }
                                            }
                                            else
                                            {
                                                activityStr += dt_Activity.Rows[k]["Name"].ToString() + "&nbsp;，";
                                            }
                                        }
                                        lblRecommend.Text = activityStr.TrimEnd('，');
                                    }
                                }
                            }
                            else
                            {
                                this.lblRecommend.Text = model.NominateActivity;
                            }
                        }
                        catch (Exception ex)
                        {
                            BLL.Util.InsertUserLog("查找推荐活动失败，工单ID：" + WorkOrderID);
                        }

                        this.lblWorkOrderRecord.Text = model.Content;
                        Entities.QueryWorkOrderTagMapping queryMapping = new Entities.QueryWorkOrderTagMapping();
                        queryMapping.OrderID = WorkOrderID;
                        DataTable dtTag = BLL.WorkOrderTag.Instance.GetWorkOrderTagByOrderID(WorkOrderID);
                        if (dtTag.Rows.Count > 0)
                        {
                            lblTag.Text = dtTag.Rows[0]["TagName"].ToString();
                        }
                        if (string.IsNullOrEmpty(model.AttentionCarBrandName))
                        {
                            this.lblCarCategory.Text = "";
                        }
                        else
                        {
                            this.lblCarCategory.Text = model.AttentionCarBrandName + "-" + model.AttentionCarSerialName + "-" + model.AttentionCarTypeName;
                        }
                        if (string.IsNullOrEmpty(model.SaleCarBrandName))
                        {
                            this.lblOutCarCategory.Text = "";
                        }
                        else
                        {
                            this.lblOutCarCategory.Text = model.SaleCarBrandName + "-" + model.SaleCarSerialName + "-" + model.SaleCarTypeName;
                        }
                        this.lblSelectDealerName.Text = model.SelectDealerName;
                        CustID = BLL.WorkOrderInfo.Instance.GetCustIDByWorkOrderTel(model);
                        if (!string.IsNullOrEmpty(CustID))
                        {
                            CustID = "<a target='_blank' href='/TaskManager/CustInformation.aspx?CustID=" + CustID + "'>查看用户</a>";
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 时间转换为时间戳字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string GenerateTimeStamp(DateTime dt)
        {
            // Default implementation of UNIX time of the current UTC time   
            TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }
}