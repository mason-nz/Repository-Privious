using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Entities.Constants;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web.QueueManage
{
    public partial class QueueView : System.Web.UI.Page
    {

        private string AgentID
        {
            get { return BLL.Util.GetLoginUserID().ToString(); }
        }
        public DataTable dtAreaInfo;
        public int RecordCount;
        public int page_seize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                BindRpt();
            }
        }


        private void BindRpt()
        {
            try
            {
                //获取当前登录人的管辖分组
                lbuserid.InnerText = "当前人id：" + BLL.Util.GetLoginUserID();
                DataTable dt = BLL.BaseData.Instance.GetUserGroupDataRigth(BLL.Util.GetLoginUserID());
                string bgids = Constant.STRING_INVALID_VALUE;
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        bgids += "," + row["BGID"].ToString();
                    }
                }
                
                if (!string.IsNullOrEmpty(bgids))
                {
                    lbbgids.InnerText = "管辖分组：" + bgids.Substring(1);
                    bgids = bgids.Substring(1);

                    DataTable dtsourceTypeId = BLL.AgentStatusDetail.Instance.GetSourceTypeByBGIDS(bgids);
                    List<CometClient> lstCometWait = new List<CometClient>();

                    for (int i = 0; i < dtsourceTypeId.Rows.Count; i++)
                    {
                        string ss = dtsourceTypeId.Rows[i]["LineID"].ToString();
                        List<CometClient> ls = DefaultChannelHandler.StateManager.GetWaitingCometClientsByBusinessLine(dtsourceTypeId.Rows[i]["LineID"].ToString());
                        if (ls != null)
                        {
                            //过滤管辖分组数据 InBGID
                            for (int j = 0; j < ls.Count; j++)
                            {
                                lstCometWait.Add(ls[j]);
                            }
                        }
                    }
                    lbqueuenum.InnerText = "队列中的数据总共有：" + +lstCometWait.Count + " 个";
                }
                else
                {
                    Response.Write("对不起，没有找到您管辖的分组数据！");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                Response.Write("[AjaxService/QueueList]..错误Message:" + ex.Message);
            }
        }
        public string GetProvinceAreaName(string provinceID, string cityID)
        {
            DataRow[] dtpro = dtAreaInfo.Select("AreaID='" + provinceID + "'");
            DataRow[] dtCit = dtAreaInfo.Select("AreaID='" + cityID + "'");
            return ((dtpro == null || dtpro.Length <= 0) ? "" : dtpro[0]["AreaName"].ToString()) + "  "
                + ((dtCit == null || dtCit.Length <= 0) ? "" : dtCit[0]["AreaName"].ToString());
        }

        private List<Entities.UserVisitLog> Pages(List<Entities.UserVisitLog> data, int start, int end)
        {
            if (start < 0)
                start = 0;
            if (end > data.Count - 1)
            {
                end = data.Count - 1;
            }

            List<Entities.UserVisitLog> result = new List<Entities.UserVisitLog>();
            for (int i = start; i <= end; i++)
            {
                result.Add(data[i]);
            }
            List<Entities.UserVisitLog> sortedResult = result.OrderBy(s => s.CreatTime).ToList();
            return sortedResult;
        }
 
        public string GetConnetTime(DateTime dts)
        {

            var t = (DateTime.Now - dts);
            return string.Format("{0}:{1}:{2}", FormatNumber(t.Hours), FormatNumber(t.Minutes), FormatNumber(t.Seconds));
        }

        private string FormatNumber(int num)
        {
            if (num < 10)
            {
                return "0" + num.ToString();
            }
            else
            {
                return num.ToString();
            }
        }

    }
}