using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Core;
using BitAuto.DSC.IM_2015.Web.Channels;
using BitAuto.DSC.IM_2015.Entities;
using System.Data;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Web.AjaxServers.QueueManage
{
    public partial class QueueList : System.Web.UI.Page
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
                dtAreaInfo = BLL.BaseData.Instance.GetAllAreaInfo();
                BindRpt();
            }
        }

        private void BindRpt()
        {
            try
            {
                //获取当前登录人的管辖分组
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
                    List<Entities.UserVisitLog> listUsers = new List<Entities.UserVisitLog>();
                    for (int i = 0; i < lstCometWait.Count; i++)
                    {
                        listUsers.Add(lstCometWait[i].Userloginfo);
                    }

                    #region  测试数据
                    //Entities.UserVisitLog ac = new Entities.UserVisitLog();
                    //ac.UserName = "test1";
                    //ac.SourceType = "1";
                    //ac.UserReferTitle = "www.baidu.com";
                    //ac.CreatTime = DateTime.Now.AddSeconds(-12);
                    //ac.ProvinceID = 10;
                    //ac.CityID = 1001;
                    //listUsers.Add(ac);
                    //Entities.UserVisitLog ac2 = new Entities.UserVisitLog();
                    //ac2.SourceType = "1";
                    //ac2.UserReferTitle = "www.baidu.com";
                    //ac2.CreatTime = DateTime.Now.AddSeconds(-12);
                    //ac2.ProvinceID = 10;
                    //ac2.CityID = 1001;
                    //ac2.UserName = "test2";
                    //listUsers.Add(ac2);
                    //Entities.UserVisitLog ac23 = new Entities.UserVisitLog();
                    //ac23.SourceType = "1";
                    //ac23.UserReferTitle = "www.baidu.com";
                    //ac23.CreatTime = DateTime.Now.AddSeconds(-12);
                    //ac23.ProvinceID = 10;
                    //ac23.CityID = 1001;
                    //ac23.UserName = "test3";
                    //listUsers.Add(ac23);
                    #endregion

                    RecordCount = listUsers.Count;

                    if (listUsers != null)
                    {
                        //分页
                        int start = (PageCommon.Instance.PageIndex - 1) * page_seize;
                        int end = PageCommon.Instance.PageIndex * page_seize - 1;
                        List<Entities.UserVisitLog> result = Pages(listUsers, start, end);


                        DataTable dtResult = new DataTable();
                        DataColumn col1 = new DataColumn("UserName");
                        DataColumn col2 = new DataColumn("SourceType");
                        DataColumn col3 = new DataColumn("UserReferTitle");
                        DataColumn col4 = new DataColumn("CreatTime");
                        DataColumn col5 = new DataColumn("Seconds");
                        DataColumn col6 = new DataColumn("ProvinceICity");
                        dtResult.Columns.Add(col1);
                        dtResult.Columns.Add(col2);
                        dtResult.Columns.Add(col3);
                        dtResult.Columns.Add(col4);
                        dtResult.Columns.Add(col5);
                        dtResult.Columns.Add(col6);
                        DateTime dtnow = DateTime.Now;
                        for (int i = 0; i < result.Count; i++)
                        {
                            DataRow newRow = dtResult.NewRow();
                            newRow["UserName"] = result[i].UserName;
                            newRow["SourceType"] = BLL.Util.GetSourceTypeName(result[i].SourceType);
                            newRow["UserReferTitle"] = result[i].UserReferTitle;
                            newRow["CreatTime"] = Convert.ToDateTime(result[i].CreatTime).ToString("yyyy-MM-dd HH:mm:ss");
                            newRow["Seconds"] = ((dtnow - Convert.ToDateTime(result[i].CreatTime)).Hours * 3600 +
                                                 (dtnow - Convert.ToDateTime(result[i].CreatTime)).Minutes * 60 + 
                                                 (dtnow - Convert.ToDateTime(result[i].CreatTime)).Seconds).ToString();
                            newRow["ProvinceICity"] = GetProvinceAreaName(result[i].ProvinceID.ToString(), result[i].CityID.ToString());
                            dtResult.Rows.Add(newRow);
                        }
                        rpt.DataSource = dtResult;
                        rpt.DataBind();
                       // litPagerQueueData.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
                        litPagerQueueData.Text =     PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, listUsers.Count, page_seize, PageCommon.Instance.PageIndex, 1);

                    }
                }
                else
                {
                    Response.Write("对不起，没有找到您管辖的分组数据！");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                BLL.Loger.Log4Net.Info("[AjaxService/QueueList]..错误Message:" + ex.Message);
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
        /*
             lstCometWait[0].MemberName; lstCometWait[0].Address; lstCometWait[0].CityGroupName;
    lstCometWait[0].LastConBeginTime; lstCometWait[0].LastMessageTime; lstCometWait[0].Distribution;
    lstCometWait[0].UserReferTitle; lstCometWait[0].ConverSTime
         */
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