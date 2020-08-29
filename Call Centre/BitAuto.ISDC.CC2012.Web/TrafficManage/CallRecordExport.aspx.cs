using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.TrafficManage
{
    public partial class CallRecordExport : PageBase
    {
        /// <summary>
        /// JSON字符串
        /// </summary>
        public string JsonStr
        {
            get
            {
                return HttpContext.Current.Request["JsonStr"] == null ? string.Empty :
                  HttpUtility.UrlDecode(HttpContext.Current.Request["JsonStr"].ToString());
            }

        }

        public string jsons
        {
            get { return Request.Form["JsonStr"]; }
        }
        public int PageSize = 20;
        public int GroupLength = 8;
        public int RecordCount;
        public int userID = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //增加“话务管理--话务总表”导出验证逻辑
                userID = BLL.Util.GetLoginUserID();
                if (BLL.Util.CheckRight(userID, "SYS024MOD40031"))
                {
                    BindData();
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }
        //绑定数据
        public void BindData()
        {
            Entities.QueryCallRecord_ORIG query = new Entities.QueryCallRecord_ORIG();

            string errMsg = string.Empty;
            BLL.ConverToEntitie<Entities.QueryCallRecord_ORIG> conver = new BLL.ConverToEntitie<Entities.QueryCallRecord_ORIG>(query);
            errMsg = conver.Conver(JsonStr);

            if (errMsg != "")
            {
                return;
            }
            int RecordCount = 0;

            int _loginID = -2;
            string _ownGroup = string.Empty;
            string _oneSelf = string.Empty;
            query.BeginTime = Request["tfBeginTime"];
            query.EndTime = Request["tfEndTime"];
            _loginID = userID;
            query.LoginID = _loginID;

            string tableEndName = BLL.Util.CalcTableNameByMonth(3, CommonFunction.ObjectToDateTime(query.BeginTime));
            DataTable dt = BLL.CallRecord_ORIG.Instance.GetCallRecord_ORIGByList(query, " c.CreateTime desc ", 1, -1, tableEndName, out RecordCount);

            #region 导出

            dt.Columns.Add("OutBoundTypeName");
            dt.Columns.Add("CallStatusName");
            dt.Columns.Add("CallTimeLong", typeof(int));
            dt.Columns.Add("CallTimeTitleLong", typeof(int));
            dt.Columns.Add("CallIDStr");

            foreach (DataRow dr in dt.Rows)
            {
                dr["CallStatusName"] = GetCallStatus(dr["CallStatus"].ToString());
                dr["OutBoundTypeName"] = GetOutBoundType(dr["CallStatus"].ToString(), dr["OutBoundType"].ToString());
                dr["CallTimeLong"] = dr["TallTime"];//修改通话时长导出
                dr["CallTimeTitleLong"] = GetTotalTime(CommonFunction.ObjectToInteger(dr["TallTime"]), CommonFunction.ObjectToInteger(dr["AfterWorkTime"]));
                dr["CallIDStr"] = dr["CallID"].ToString();
            }

            //要导出的字段
            Dictionary<string, string> ExportColums = new Dictionary<string, string>();
            ExportColums.Add("callidstr", "话务ID");
            ExportColums.Add("taskid", "任务ID");
            ExportColums.Add("agentnum", "工号");
            ExportColums.Add("phonenum", "主叫");
            ExportColums.Add("ani", "被叫");

            ExportColums.Add("callstatusname", "话务类型");
            ExportColums.Add("outboundtypename", "呼叫类型");

            ExportColums.Add("establishedtime", "接通时间");
            ExportColums.Add("releasetime", "挂断时间");

            ExportColums.Add("ringingspantime", "振铃时长");
            ExportColums.Add("calltimelong", "通话时长");
            ExportColums.Add("afterworktime", "话后时长");
            ExportColums.Add("calltimetitlelong", "话务总时长");

            dt.Columns["CallIDStr"].SetOrdinal(0);
            dt.Columns["TaskID"].SetOrdinal(1);
            dt.Columns["AgentNum"].SetOrdinal(2);
            dt.Columns["PhoneNum"].SetOrdinal(3);
            dt.Columns["ANI"].SetOrdinal(4);
            dt.Columns["CallStatusName"].SetOrdinal(5);
            dt.Columns["SwitchINNum"].SetOrdinal(6);
            dt.Columns["OutBoundTypeName"].SetOrdinal(7);
            dt.Columns["RingingTime"].SetOrdinal(8);
            dt.Columns["EstablishedTime"].SetOrdinal(9);
            dt.Columns["ReleaseTime"].SetOrdinal(10);
            dt.Columns["RingingSpanTime"].SetOrdinal(11);
            dt.Columns["CallTimeLong"].SetOrdinal(12);
            dt.Columns["AfterWorkTime"].SetOrdinal(13);
            dt.Columns["CallTimeTitleLong"].SetOrdinal(14);
            dt.Columns["ConsultTime"].SetOrdinal(15);
            dt.Columns["ReleaseType"].SetOrdinal(16);

            for (int i = dt.Columns.Count - 1; i >= 0; i--)
            {
                if (ExportColums.ContainsKey(dt.Columns[i].ColumnName.ToLower()))
                {
                    //字段时要导出的字段，改名
                    dt.Columns[i].ColumnName = ExportColums[dt.Columns[i].ColumnName.ToLower()];
                }
                else
                {
                    //不是要导出的字段，删除
                    dt.Columns.RemoveAt(i);
                }
            }

            BLL.Util.ExportToCSV("话务总表", dt);

            #endregion

        }
        //话务总时长
        public int GetTotalTime(int TallTime, int AfterWorkTime)
        {
            int time1 = TallTime == -2 ? 0 : TallTime;
            int time2 = AfterWorkTime == -2 ? 0 : AfterWorkTime;
            return (time1 + time2);
        }
        public string GetCallStatus(string CallStatus)
        {
            return BitAuto.ISDC.CC2012.BLL.Util.GetCallStatus(CallStatus);
        }
        public string GetOutBoundType(string CallStatus, string OutBoundType)
        {
            if (CallStatus == "2")
            {
                if (OutBoundType == "1")
                {
                    return "页面";
                }
                else if (OutBoundType == "2")
                {
                    return "客户端";
                }
                else if (OutBoundType == "4")
                {
                    return "自动";
                }
                else
                {
                    return "无";
                }
            }
            else
            {
                return "无";
            }
        }
    }
}