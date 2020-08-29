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
    public partial class ExclusiveMissedCallsListExport : PageBase
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

        private string AgentID
        {
            get
            {
                return Request["AgentID"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentID"].ToString().Trim());
            }
        }
        private string AgentNum
        {
            get
            {
                return Request["AgentNum"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentNum"].ToString().Trim());
            }
        }

        private string AgentGroup
        {
            get
            {
                return Request["AgentGroup"] == null ? "" :
                HttpUtility.UrlDecode(Request["AgentGroup"].ToString().Trim());
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //增加“来电记录--未接来电”导出功能验证逻辑
            int userId = BLL.Util.GetLoginUserID();
            if (BLL.Util.CheckRight(userId, "SYS024BUT4122"))
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
            ExclusiveMissedCalls query = new ExclusiveMissedCalls();
            query.ANI = ANI;
            query.BeginTime = DateTimeToString(BeginTime, "00:00:00");
            query.EndTime = DateTimeToString(EndTime, "23:59:59");
            query.AgentNum = AgentNum;
            query.AgentID = AgentID;
            if (AgentGroup != "" && CommonFunction.ObjectToInteger(AgentGroup) > 0)
            {
                query.AgentGroup = AgentGroup;
            }


            int RecordCount = 0;
            DataTable dt = BLL.CustomerVoiceMsg.Instance.GetExclusiveMissedCallingsData(query, "a.StartTime desc ", 1, -1, out RecordCount);
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
                        && dt.Columns[i].ColumnName.ToUpper() != "BUSINESSGROUPNAME"
                        && dt.Columns[i].ColumnName.ToUpper() != "EXCLUSIVEAGENTNUM"
                        && dt.Columns[i].ColumnName.ToUpper() != "EXCLUSIVEUSERNAME"
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
                dt.Columns["BUSINESSGROUPNAME"].SetOrdinal(n++);
                dt.Columns["EXCLUSIVEAGENTNUM"].SetOrdinal(n++);
                dt.Columns["EXCLUSIVEUSERNAME"].SetOrdinal(n++);


                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    switch (dt.Columns[i].ColumnName.ToUpper())
                    {
                        case "CALLNO": dt.Columns[i].ColumnName = "主叫号码"; break;
                        case "STARTTIME": dt.Columns[i].ColumnName = "开始时间"; break;
                        case "ENDTIME": dt.Columns[i].ColumnName = "挂断时间时间"; break;
                        case "BUSINESSGROUPNAME": dt.Columns[i].ColumnName = "所属分组"; break;
                        case "EXCLUSIVEAGENTNUM": dt.Columns[i].ColumnName = "工号"; break;
                        case "EXCLUSIVEUSERNAME": dt.Columns[i].ColumnName = "客服"; break;
                    }
                }

                BLL.Util.ExportToCSV("专属客服未接来电" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), dt);
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