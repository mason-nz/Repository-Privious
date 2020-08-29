using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Web.Base;

namespace BitAuto.ISDC.CC2012.Web.AjaxServers.CallReport
{
    public partial class SatisfactionExport : PageBase
    {
        #region 属性
        /// <summary>
        /// 坐席ID
        /// </summary>
        private string RequestAgentID
        {
            get { return HttpContext.Current.Request["AgentID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AgentID"].ToString()); }
        }
        /// <summary>
        /// 坐席工号
        /// </summary>
        private string RequestAgentNum
        {
            get { return HttpContext.Current.Request["AgentNum"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["AgentNum"].ToString()); }
        }
        /// <summary>
        /// 统计开始时间
        /// </summary>
        private string RequestBeginTime
        {
            get { return HttpContext.Current.Request["TbeginTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TbeginTime"].ToString()); }
        }
        /// <summary>
        /// 统计结束时间
        /// </summary>
        private string RequestEndTime
        {
            get { return HttpContext.Current.Request["TendTime"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["TendTime"].ToString()); }
        }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string RequestBusinessType
        {
            get { return HttpContext.Current.Request["BusinessType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["BusinessType"].ToString()); }
        }
        /// <summary>
        /// 技能组
        /// </summary>
        private string RequestSkillGroup
        {
            get { return HttpContext.Current.Request["SkillGroup"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["SkillGroup"].ToString()); }
        }
        /// <summary>
        /// 1是日，2是周，3是月
        /// </summary>
        public string DateType
        {
            get { return HttpContext.Current.Request["DateType"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["DateType"].ToString()); }
        }
        /// <summary>
        /// 地区ID
        /// </summary>
        public string PlaceID
        {
            get { return HttpContext.Current.Request["PlaceID"] == null ? "" : HttpUtility.UrlDecode(HttpContext.Current.Request["PlaceID"].ToString()); }
        }
        #endregion

        #region 页面请求，导出
        private int userID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //取当前登录人
                userID = BLL.Util.GetLoginUserID();
                //增加 满意度统计 【导出】操作权限
                if (BLL.Util.CheckRight(userID, "SYS024BUT4003"))
                {
                    //取导出的数据
                    DataTable dt = GetDataTable();
                    //生成csv
                    if (dt != null)
                    {
                        Export(dt);
                    }
                }
                else
                {
                    Response.Write(BLL.Util.GetNotAccessMsgPage("您没有访问该页面的权限"));
                    Response.End();
                }
            }
        }
        /// <summary>
        /// 取导出的数据
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataTable()
        {
            #region 组织条件
            DateTime b = System.DateTime.Now;
            DateTime e = System.DateTime.Now;
            int _DateType = 1;
            int.TryParse(DateType, out _DateType);

            Entities.QueryCallRecord_ORIG query = new Entities.QueryCallRecord_ORIG();

            if (!string.IsNullOrEmpty(RequestAgentID))
            {
                int _createuserid = 0;
                if (int.TryParse(RequestAgentID, out _createuserid))
                {
                    query.CreateUserID = _createuserid;
                }
            }
            if (!string.IsNullOrEmpty(RequestAgentNum))
            {
                query.AgentNum = RequestAgentNum;
            }
            if (!string.IsNullOrEmpty(RequestBeginTime) && !string.IsNullOrEmpty(RequestEndTime))
            {
                DateTime.TryParse(RequestBeginTime, out b);
                DateTime.TryParse(RequestEndTime, out e);
                query.BeginTime = RequestBeginTime;
                query.EndTime = RequestEndTime;
            }
            if (!string.IsNullOrEmpty(RequestSkillGroup) && RequestSkillGroup != "-1")
            {
                query.SkillGroup = RequestSkillGroup;
            }
            if (!string.IsNullOrEmpty(RequestBusinessType))
            {
                query.SwitchINNum = RequestBusinessType;
            }
            query.LoginID = userID;
            int RecordCount = 0;
            //placeN=1是西安，DateType=1是按日汇总
            int placeN = 1;
            if (PlaceID == "1")
            {
                placeN = 2;
            }
            #endregion

            #region 查询统计，以及汇总
            string tableEndName = BLL.Util.CalcTableNameByMonth(3, CommonFunction.ObjectToDateTime(query.BeginTime));
            DataTable dt = BLL.CallRecord_ORIG.Instance.GetSatisfactionList(query, "", 1, -1, out RecordCount, placeN, b, e, _DateType, tableEndName);
            //组织导出的数据
            if (dt != null)
            {
                DataColumn colum = new DataColumn("HuiZong", typeof(string));
                dt.Columns.Add(colum);
                //西安
                if (placeN == 1)
                {
                    //参评率
                    DataColumn canpinglv = new DataColumn("canpinglv", typeof(string));
                    dt.Columns.Add(canpinglv);
                    //解决率
                    DataColumn jiejuelv = new DataColumn("jiejuelv", typeof(string));
                    dt.Columns.Add(jiejuelv);
                }
                else
                {
                    //转接率
                    DataColumn zIVRlv = new DataColumn("zIVRlv", typeof(string));
                    dt.Columns.Add(zIVRlv);
                }
                //满意度参评率
                DataColumn manyidcplv = new DataColumn("manyidcplv", typeof(string));
                dt.Columns.Add(manyidcplv);
                //满意度
                DataColumn manyilv = new DataColumn("manyilv", typeof(string));
                dt.Columns.Add(manyilv);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["HuiZong"] = DateType == "1" ? dt.Rows[i]["mind"].ToString().Split(' ')[0] : dt.Rows[i]["mind"].ToString().Split(' ')[0] + "至" + dt.Rows[i]["maxd"].ToString().Split(' ')[0];
                    if (placeN == 1)
                    {
                        //参评率
                        dt.Rows[i]["canpinglv"] = GetRound(dt.Rows[i]["参评率"].ToString());
                        //解决率
                        dt.Rows[i]["jiejuelv"] = GetRound(dt.Rows[i]["解决率"].ToString());
                    }
                    else
                    {
                        dt.Rows[i]["zIVRlv"] = GetRound(dt.Rows[i]["转接比率"].ToString());
                    }

                    //满意度参评率
                    dt.Rows[i]["manyidcplv"] = GetRound(dt.Rows[i]["满意度参评率"].ToString());
                    //满意度
                    dt.Rows[i]["manyilv"] = GetRound(dt.Rows[i]["满意度"].ToString());
                }
                //if (dt.Rows.Count > 0)
                //{
                //取汇总行
                int RecordCountSum = 0;
                DataTable dth = BLL.CallRecord_ORIG.Instance.GetSatisfactionList(query, "", 1, 1, out RecordCountSum, placeN, b, e, tableEndName);
                if (dth != null && dth.Rows.Count > 0)
                {
                    DataRow r = dt.NewRow();
                    r["HuiZong"] = "合 计（共" + RecordCount + "项）";
                    r["TrueName"] = "--";
                    r["总接通量"] = dth.Rows[0]["总接通量"] == DBNull.Value ? 0 : dth.Rows[0]["总接通量"];
                    if (placeN == 1)
                    {
                        r["问题解决参评总数"] = dth.Rows[0]["问题解决参评总数"] == DBNull.Value ? 0 : dth.Rows[0]["问题解决参评总数"];
                        //参评率
                        r["canpinglv"] = GetRound(dth.Rows[0]["参评率"].ToString());
                        r["解决总数"] = dth.Rows[0]["解决总数"] == DBNull.Value ? 0 : dth.Rows[0]["解决总数"];
                        //解决率
                        r["jiejuelv"] = GetRound(dth.Rows[0]["解决率"].ToString());
                        r["未解决总数"] = dth.Rows[0]["未解决总数"] == DBNull.Value ? 0 : dth.Rows[0]["未解决总数"];
                    }
                    else
                    {
                        r["转IVR数"] = dth.Rows[0]["转IVR数"] == DBNull.Value ? 0 : dth.Rows[0]["转IVR数"];
                        r["zIVRlv"] = GetRound(dth.Rows[0]["转接比率"].ToString());
                    }
                    r["满意度参评总数"] = dth.Rows[0]["满意度参评总数"] == DBNull.Value ? 0 : dth.Rows[0]["满意度参评总数"];
                    //满意度参评率
                    r["manyidcplv"] = GetRound(dth.Rows[0]["满意度参评率"].ToString());
                    r["满意个数"] = dth.Rows[0]["满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["满意个数"];
                    //满意度
                    r["manyilv"] = GetRound(dth.Rows[0]["满意度"].ToString());
                    r["不满意个数"] = dth.Rows[0]["不满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["不满意个数"];
                    r["对处理结果不满意个数"] = dth.Rows[0]["对处理结果不满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["对处理结果不满意个数"];
                    r["对服务不满意个数"] = dth.Rows[0]["对服务不满意个数"] == DBNull.Value ? 0 : dth.Rows[0]["对服务不满意个数"];
                    dt.Rows.Add(r);
                }
                //}
                if (placeN == 1)
                {
                    dt.Columns.Remove("参评率");
                    dt.Columns.Remove("解决率");
                }
                else
                {
                    dt.Columns.Remove("转接比率");
                }
                dt.Columns.Remove("满意度参评率");
                dt.Columns.Remove("满意度");

            }
            return dt;
            #endregion
        }
        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetRound(string number)
        {
            decimal n = decimal.Parse(number) * 100;
            decimal d = decimal.Round(n, 2);
            return d.ToString() + "%";
        }

        #region 导出
        private void Export(DataTable dt)
        {
            //要导出的字段
            //placeN=1是西安，DateType=1是按日汇总
            int placeN = 1;
            if (PlaceID == "1")
            {
                placeN = 2;
            }
            Dictionary<string, string> exportColums = new Dictionary<string, string>();
            //列名与导出名对应
            exportColums.Add("HuiZong", "日期");
            exportColums.Add("TrueName", "客服");
            exportColums.Add("总接通量", "总接通量");
            if (placeN == 1)
            {
                exportColums.Add("问题解决参评总数", "问题解决参评总数");
                exportColums.Add("canpinglv", "问题解决参评率");
                exportColums.Add("解决总数", "问题解决个数");
                exportColums.Add("jiejuelv", "问题解决率");
                exportColums.Add("未解决总数", "问题未解决个数");
            }
            else
            {
                exportColums.Add("转IVR数", "转IVR数");
                exportColums.Add("zIVRlv", "转接比率");
            }
            exportColums.Add("满意度参评总数", "满意度参评总数");
            exportColums.Add("manyidcplv", "满意度参评率");
            exportColums.Add("满意个数", "满意个数");
            exportColums.Add("manyilv", "满意度");
            exportColums.Add("不满意个数", "不满意个数");
            exportColums.Add("对处理结果不满意个数", "对处理结果不满意个数");
            exportColums.Add("对服务不满意个数", "对服务不满意个数");

            //设定顺序
            dt.Columns["HuiZong"].SetOrdinal(0);
            dt.Columns["TrueName"].SetOrdinal(1);
            dt.Columns["总接通量"].SetOrdinal(2);
            if (placeN == 1)
            {
                dt.Columns["问题解决参评总数"].SetOrdinal(3);
                dt.Columns["canpinglv"].SetOrdinal(4);
                dt.Columns["解决总数"].SetOrdinal(5);
                dt.Columns["jiejuelv"].SetOrdinal(6);
                dt.Columns["未解决总数"].SetOrdinal(7);
                dt.Columns["满意度参评总数"].SetOrdinal(8);
                dt.Columns["manyidcplv"].SetOrdinal(9);
                dt.Columns["满意个数"].SetOrdinal(10);
                dt.Columns["manyilv"].SetOrdinal(11);
                dt.Columns["不满意个数"].SetOrdinal(12);
                dt.Columns["对处理结果不满意个数"].SetOrdinal(13);
                dt.Columns["对服务不满意个数"].SetOrdinal(14);
            }
            else
            {
                dt.Columns["转IVR数"].SetOrdinal(3);
                dt.Columns["zIVRlv"].SetOrdinal(4);

                dt.Columns["满意度参评总数"].SetOrdinal(5);
                dt.Columns["manyidcplv"].SetOrdinal(6);
                dt.Columns["满意个数"].SetOrdinal(7);
                dt.Columns["manyilv"].SetOrdinal(8);
                dt.Columns["不满意个数"].SetOrdinal(9);
                dt.Columns["对处理结果不满意个数"].SetOrdinal(10);
                dt.Columns["对服务不满意个数"].SetOrdinal(11);
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (exportColums.ContainsKey(dt.Columns[i].ColumnName))
                {
                    //字段时要导出的字段，改名
                    dt.Columns[i].ColumnName = exportColums[dt.Columns[i].ColumnName];
                }
                else
                {
                    //不是要导出的字段，删除
                    dt.Columns.RemoveAt(i);
                    i--;
                }
            }

            BLL.Util.ExportToCSV("满意度统计报表" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), dt);
        }
        #endregion

        #endregion
    }
}