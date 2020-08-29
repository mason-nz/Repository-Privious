using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.BLL;
using BitAuto.DSC.IM_2015.Entities;
using BitAuto.DSC.IM_2015.Entities.Constants;
using System.Data;
using System.Configuration;
using BitAuto.DSC.IM_2015.Web.Channels;

namespace BitAuto.DSC.IM_2015.Web.TrailManager
{
    public partial class ToadySummary : System.Web.UI.Page
    {
        //客服ID
        private string YiCheLine
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["YiCheLine"];
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            BitAuto.YanFa.SysRightManager.Common.UserInfo.Check();
            if (!IsPostBack)
            {
                BindData();
                BindDataToadyTotal();
                BindDataChildToadyTotal();
            }
        }

        //绑定数据
        private void BindData()
        {
            rpeList.DataSource = DefaultChannelHandler.StateManager.GetAgentRealTime_OnlineHaveBGID();
            rpeList.DataBind();
        }
        int totalCount = 0;

        private void BindDataToadyTotal()
        {
            int count = 0;
            var dt = BLL.Conversations.Instance.Today_Total_Select(new QueryBussinessLineTotal() { BeginTime = DateTime.Now, EndTime = DateTime.Now, SelectType = 1, SourceType = -1 }, "", 1, 10000, out count);
            var dr = dt.Select("SourceType not in(" + YiCheLine + ")");//获取一级来源
            var drSecond = dt.Select("SourceType  in(" + YiCheLine + ")");//获取二级来源
            var dtSum = BLL.Conversations.Instance.Today_Total_Select(new QueryBussinessLineTotal() { BeginTime = DateTime.Now, EndTime = DateTime.Now, SelectType = 1, SourceType = -1 });
            totalCount = drSecond.Length > 0 ? dr.Length + 1 : dr.Length;
            DataTable dtNew = dt.Clone();//构建新表
            foreach (DataRow item in dr)
            {
                dtNew.ImportRow(item);//填充来源为一级数据
            }
            if (drSecond.Length > 0)
            {
                DataRow drNew = dtNew.NewRow();//构建易车汇总列
                drNew["SourceType"] = "100";
                drNew["SumVisit"] = GetSum(drSecond, "SumVisit");
                drNew["SumConversation"] = GetSum(drSecond, "SumConversation"); ;
                drNew["SumReception"] = GetSum(drSecond, "SumReception"); ;
                drNew["SumQueueFail"] = GetSum(drSecond, "SumQueueFail"); ;
                drNew["LeaveMessage"] = GetSum(drSecond, "LeaveMessage"); ;
                dtNew.Rows.Add(drNew);
            }
            if (totalCount > 0)
            {
                foreach (DataRow item in dtSum.Rows)//汇总数据
                {
                    dtNew.ImportRow(item);//追加汇总数据到最后一行
                }
            }
            rptToadyTotal.DataSource = dtNew;
            rptToadyTotal.DataBind();
        }

        private void BindDataChildToadyTotal()
        {
            int count = 0;
            var dt = BLL.Conversations.Instance.Today_Total_Select(new QueryBussinessLineTotal() { BeginTime = DateTime.Now, EndTime = DateTime.Now, SelectType = 1, SourceType = -1 }, "", 1, 10000, out count);
            var dr = dt.Select("SourceType in(" + YiCheLine + ")");//获取二级来源
            DataTable dtNew = dt.Clone();
            foreach (DataRow item in dr)
            {
                dtNew.ImportRow(item);
            }
            rptChildTotal.DataSource = dtNew;
            rptChildTotal.DataBind();
        }

        public string GetInBGIDName(string InBGIDName, string InBGID)
        {
            if (!string.IsNullOrEmpty(InBGID))
            {
                return "<a href='ServiceMonitoringList.aspx?BGID=" + InBGID + "'>" + InBGIDName + "</a>";
            }
            else
            {
                return InBGIDName;
            }
        }

        /// <summary>
        /// 值累加
        /// </summary>
        /// <returns></returns>
        public int GetSum(DataRow[] drList, string cloumnName)
        {
            int total = 0;
            foreach (DataRow item in drList)
            {
                if (!string.IsNullOrEmpty(item[cloumnName].ToString()))
                {
                    total = total + int.Parse(item[cloumnName].ToString());
                }
            }
            return total;
        }
        //率计算方法
        public string GetAvg(string str1, string str2)
        {
            if (str1 != "0" && str1 != "" && str2 != "")
            {
                return (float.Parse(str2) / float.Parse(str1) * 100).ToString("N2") + "%";
            }
            return "-";
        }
        /// <summary>
        /// 来源名称
        /// </summary>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public string GetSourceTypeName(string sourceType)
        {
            if (sourceType == "100")
            {
                return "易车汇总";
            }
            else if (!string.IsNullOrEmpty(sourceType))
            {
                return BLL.Util.GetSourceTypeName(sourceType);
            }
            else
            {
                return "合计(共" + totalCount + "项）";
            }
        }
        //会话
        public int GetConvertCountByBusinessLine(string bussinessId)
        {
            return DefaultChannelHandler.StateManager.GetConvertCountByBusinessLine(bussinessId);
        }
        //排队
        public int GetQueueCountByBusinessLine(string bussinessId)
        {
            return DefaultChannelHandler.StateManager.GetQueueCountByBusinessLine(bussinessId);
        }
    }
}