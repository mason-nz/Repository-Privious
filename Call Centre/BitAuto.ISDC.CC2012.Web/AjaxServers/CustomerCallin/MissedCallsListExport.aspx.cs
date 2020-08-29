using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BitAuto.ISDC.CC2012.Entities;
using System.Data;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CustomerCallin
{
    public partial class MissedCallsListExport : PageBase
    {
        #region 参数
        private string ANI
        {
            get
            {
                return Request["ANI"] == null ? "" :
                HttpUtility.UrlDecode(Request["ANI"].ToString().Trim());
            }
        }
        private string BeginTime
        {
            get
            {
                return Request["BeginTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["BeginTime"].ToString().Trim());
            }
        }
        private string EndTime
        {
            get
            {
                return Request["EndTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["EndTime"].ToString().Trim());
            }
        }
        private string selBusinessType
        {
            get
            {
                return Request["selBusinessType"] == null ? "" :
                HttpUtility.UrlDecode(Request["selBusinessType"].ToString().Trim());
            }
        }
        private string Agent
        {
            get
            {
                return Request["Agent"] == null ? "" :
                HttpUtility.UrlDecode(Request["Agent"].ToString().Trim());
            }
        }
        private string PRBeginTime
        {
            get
            {
                return Request["PRBeginTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["PRBeginTime"].ToString().Trim());
            }
        }
        private string PREndTime
        {
            get
            {
                return Request["PREndTime"] == null ? "" :
                HttpUtility.UrlDecode(Request["PREndTime"].ToString().Trim());
            }
        }
        private string prStatus
        {
            get
            {
                return Request["prStatus"] == null ? "" :
                HttpUtility.UrlDecode(Request["prStatus"].ToString().Trim());
            }
        }
        private string HasSkill
        {
            get
            {
                return Request["HasSkill"] == null ? "" :
                HttpUtility.UrlDecode(Request["HasSkill"].ToString().Trim());
            }
        }
        private string Hasaudio
        {
            get
            {
                return Request["Hasaudio"] == null ? "" :
                HttpUtility.UrlDecode(Request["Hasaudio"].ToString().Trim());
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //增加“来电记录--未接来电”导出功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userId, "SYS024BUT4121"))
            {
                ExportData();
            }
            else
            {
                Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                Response.End();
            }
        }

        private void ExportData()
        {
            //查询数据
            QueryCustomerVoiceMsg query = new QueryCustomerVoiceMsg();
            query.ANI = ANI;
            query.BeginTime = DateTimeToString(BeginTime, "00:00:00");
            query.EndTime = DateTimeToString(EndTime, "23:59:59");
            query.selBusinessType = selBusinessType;
            query.Agent = Agent;
            query.PRBeginTime = DateTimeToString(PRBeginTime, "00:00:00");
            query.PREndTime = DateTimeToString(PREndTime, "23:59:59");
            query.prStatus = prStatus;
            query.HasSkill = HasSkill;
            query.Hasaudio = Hasaudio;

            int RecordCount = 0;
            DataTable dt = BLL.CustomerVoiceMsg.Instance.GetCustomerVoiceMsgData(query, "a.StartTime desc ", 1, -1, out RecordCount);
            ExportData(dt);
        }

        private void ExportData(DataTable dt)
        {
            if (dt != null)
            {
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (dt.Columns[i].ColumnName.ToUpper() != "CALLNO"
                        && dt.Columns[i].ColumnName.ToUpper() != "STARTTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "ENDTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "HOTLINENAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "SKILLNAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "PROCESSUSERNAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "PROCESSTIME"
                        && dt.Columns[i].ColumnName.ToUpper() != "STATUS_NM"
                        )
                    {
                        dt.Columns.Remove(dt.Columns[i].ColumnName);
                    }
                }

                //设置列顺序
                int n = 0;
                dt.Columns["CALLNO"].SetOrdinal(n++);
                dt.Columns["STARTTIME"].SetOrdinal(n++);
                dt.Columns["ENDTIME"].SetOrdinal(n++);
                dt.Columns["HOTLINENAME"].SetOrdinal(n++);
                dt.Columns["SKILLNAME"].SetOrdinal(n++);
                dt.Columns["PROCESSUSERNAME"].SetOrdinal(n++);
                dt.Columns["PROCESSTIME"].SetOrdinal(n++);
                dt.Columns["STATUS_NM"].SetOrdinal(n++);

                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    switch (dt.Columns[i].ColumnName.ToUpper())
                    {
                        case "CALLNO": dt.Columns[i].ColumnName = "主叫号码"; break;
                        case "STARTTIME": dt.Columns[i].ColumnName = "开始时间"; break;
                        case "ENDTIME": dt.Columns[i].ColumnName = "结束时间"; break;
                        case "HOTLINENAME": dt.Columns[i].ColumnName = "业务线"; break;
                        case "SKILLNAME": dt.Columns[i].ColumnName = "技能组"; break;
                        case "PROCESSUSERNAME": dt.Columns[i].ColumnName = "处理人"; break;
                        case "PROCESSTIME": dt.Columns[i].ColumnName = "处理时间"; break;
                        case "STATUS_NM": dt.Columns[i].ColumnName = "处理状态"; break;
                    }
                }

                BLL.Util.ExportToCSV("未接来电" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
            }
        }

        /// 时间处理和转换
        /// <summary>
        /// 时间处理和转换
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string DateTimeToString(string value, string hh)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }
            else return CommonFunction.ObjectToDateTime(value).ToString("yyyy-MM-dd") + " " + hh;
        }
    }
}