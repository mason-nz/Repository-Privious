using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.DSC.IM_2015.Entities;
using System.Data;
using BitAuto.YanFa.SysRightManager.Common;
using BitAuto.Utils.Config;
using BitAuto.DSC.IM_2015.Entities.Constants;

namespace BitAuto.DSC.IM_2015.Web
{
    public partial class ConversationHistoryForCC : System.Web.UI.Page
    {
        public int RecordCount;

        public string Reqdata
        {
            get
            {
                return BLL.Util.GetCurrentRequestStr("data").ToString().Trim();
            }
        }
        public CCPrame jsondata;
        /// <summary>
        /// 此页面供CC系统查看聊天记录。TimeStamp和AgentID为必须传入的参数；OrderID和CSID传任意一个即可
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

               string strReq = BLL.Util.DecryptString(Reqdata);
               jsondata = (CCPrame)Newtonsoft.Json.JavaScriptConvert.DeserializeObject(strReq, typeof(CCPrame));

                DateTime datetime = new DateTime();
                try
                {
                    int agentid = int.Parse(jsondata.AgentID);//如果不能转换成int，就会抛出异常，从而证明agentid组成非法
                    datetime = GetDateFromTimeStamp(jsondata.TimeStamp);
                }
                catch (Exception ex)
                {
                    Response.Write("此链接无效！");
                    Response.End();
                }
                if (!CheckUser())
                {
                    Response.Write("对不起，您没有查看此页面的权限！");
                    Response.End();
                }
                DateTime datenow = DateTime.Now;
                if ((datenow - datetime).TotalMinutes > 60)
                {
                    Response.Write("链接已过期，请尝试刷新父页面！");
                    Response.End();
                }
                else
                {
                    try
                    {
                        BindData();
                    }
                    catch (Exception ex)
                    {
                        DataTable dt = new DataTable();
                        DataColumn col1 = new DataColumn("newName");
                        DataColumn col2 = new DataColumn("Content");
                        dt.Columns.Add(col1);
                        dt.Columns.Add(col2);

                        DataRow row = dt.NewRow();
                        row[0] = "";
                        row[1] = "没有找到符合查询条件的数据！";
                        dt.Rows.Add(row);

                        Rt_CSHistoryData.DataSource = dt;
                        Rt_CSHistoryData.DataBind();
                    }
                }
            }
        }
       
        public void BindData()
        {
            RecordCount = 0;

            QueryConversations query = new QueryConversations();
            if (string.IsNullOrEmpty(jsondata.OrderID) && string.IsNullOrEmpty(jsondata.CSID))
            {
                query.CSID = -1;
            }
            else if (!string.IsNullOrEmpty(jsondata.OrderID))
            {
                query.OrderID = jsondata.OrderID;
            }
            //通过会话id进行查询
            else if (!string.IsNullOrEmpty(jsondata.CSID))
            {
                int csid;
                if (int.TryParse(jsondata.CSID, out csid))
                {
                    query.CSID = csid;
                }
            }

            try
            {
                DataTable dt = BLL.Conversations.Instance.GetConversationHistoryDataForCC(query, "c.CreateTime asc", BLL.PageCommon.Instance.PageIndex, 10, out RecordCount);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataColumn col1 = new DataColumn("newName", typeof(string));
                    dt.Columns.Add(col1);
                    foreach (DataRow row in dt.Rows)
                    {
                        switch (row["Sender"].ToString())
                        {
                            case "1": row["newName"] = "客服代表" + row["AgentNum"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                            case "2": row["newName"] = row["UserName"].ToString() + "    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                            default: row["newName"] = "系统消息    【" + Convert.ToDateTime(row["CreateTime"].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "】"; break;
                        }
                    }

                    Rt_CSHistoryData.DataSource = dt;
                    Rt_CSHistoryData.DataBind();
                }
                else
                {
                    DataTable dt2 = new DataTable();
                    DataColumn col1 = new DataColumn("newName");
                    DataColumn col2 = new DataColumn("Content");
                    dt2.Columns.Add(col1);
                    dt2.Columns.Add(col2);

                    DataRow row = dt2.NewRow();
                    row[0] = "";
                    row[1] = "没有找到符合查询条件的数据！";
                    dt2.Rows.Add(row);

                    Rt_CSHistoryData.DataSource = dt2;
                    Rt_CSHistoryData.DataBind();
                }
            }
            catch (Exception ex)
            {
                DataTable dt2 = new DataTable();
                DataColumn col1 = new DataColumn("newName");
                DataColumn col2 = new DataColumn("Content");
                dt2.Columns.Add(col1);
                dt2.Columns.Add(col2);

                DataRow row = dt2.NewRow();
                row[0] = "";
                row[1] = "没有找到符合查询条件的数据！";
                dt2.Rows.Add(row);

                Rt_CSHistoryData.DataSource = dt2;
                Rt_CSHistoryData.DataBind();
            }

            litPagerDown_History.Text = BLL.PageCommon.Instance.LinkStringByPost(BLL.Util.GetUrl(), 8, RecordCount, 10, BLL.PageCommon.Instance.PageIndex, 1);
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
        /// <summary>
        /// 时间戳转为日期时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public DateTime GetDateFromTimeStamp(string now)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(now + "0000");  //说明下，时间格式为13位后面补加4个"0"，如果时间格式为10位则后面补加7个"0",至于为什么我也不太清楚，也是仿照人家写的代码转换的
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow); //得到转换后的时间

            return dtResult;
        }


        public bool CheckUser()
        {
            string sysID = ConfigurationUtil.GetAppSettingValue("ThisSysID");
            DataTable dtParent = UserInfo.Instance.GetParentModuleInfoByUserID(Convert.ToInt32(jsondata.AgentID), sysID);
            if (dtParent != null && dtParent.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
    public class CCPrame
    {
        private string _orderid;
        private string _csid;
        private string _agentid;
        private string _timestamp;

        public CCPrame()
        {
            _orderid = Constant.STRING_INVALID_VALUE;
            _csid = Constant.STRING_INVALID_VALUE;
            _agentid = Constant.STRING_INVALID_VALUE;
            _timestamp = Constant.STRING_INVALID_VALUE;
        }

        public string OrderID
        {
            get { return _orderid; }
            set { _orderid = value; }
        }
        public string CSID
        {
            get { return _csid; }
            set { _csid = value; }
        }
        public string AgentID
        {
            get { return _agentid; }
            set { _agentid = value; }
        }
        public string TimeStamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
    }
}