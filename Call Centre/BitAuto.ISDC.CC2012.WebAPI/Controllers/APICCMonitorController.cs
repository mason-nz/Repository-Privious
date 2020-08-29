using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BitAuto.ISDC.CC2012.WebAPI.Filter;
using BitAuto.ISDC.CC2012.WebAPI.WebServices;
using BitAuto.Utils.Config;
using BitAuto.ISDC.CC2012.BLL;
using System.Data;
using BitAuto.ISDC.CC2012.WebAPI.Models.ChartModels;
using Newtonsoft.Json;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.WebAPI.Helper;

namespace BitAuto.ISDC.CC2012.WebAPI.Controllers
{
    [BitAutoAuthorize(NeedCheckIP = true)]
    public class APICCMonitorController : ApiController
    {
        private string MonitorDate = ConfigurationManager.AppSettings["MonitorDate"];

        #region 辅助
        /// 第一屏左图最大y值
        /// <summary>
        /// 第一屏左图最大y值
        /// </summary>
        public const int MaxTop1_L = 15;
        /// 第一屏右图最大y值
        /// <summary>
        /// 第一屏右图最大y值
        /// </summary>
        public const int MaxTop1_R = 100;
        /// 第二屏左上最大y值
        /// <summary>
        /// 第二屏左上最大y值
        /// </summary>
        public const int MaxTop2_1 = 60;
        /// 第二屏左下最大y值
        /// <summary>
        /// 第二屏左下最大y值
        /// </summary>
        public const int MaxTop2_2 = 20;
        /// 第三屏左上最大y值
        /// <summary>
        /// 第三屏左上最大y值
        /// </summary>
        public const int MaxTop3_1 = 80;
        /// 第三屏左下最大y值
        /// <summary>
        /// 第三屏左下最大y值
        /// </summary>
        public const int MaxTop3_2 = 20;
        /// 透明度
        /// <summary>
        /// 透明度
        /// </summary>
        public const double Alpha = 1;

        /// 生成线对象
        /// <summary>
        /// 生成线对象
        /// </summary>
        /// <param name="dt">数据表（只取第一行数据）</param>
        /// <param name="seriesNm">线名称</param>
        /// <param name="seriesColor">线颜色</param>
        /// <param name="colnm_color">列名称和点颜色的对应关系</param>
        /// <param name="dl">点标注</param>
        /// <param name="maxY">最大Y值（-1代表没有）（点中的name是实际值，y是转换过用来画图的值）</param>
        /// <param name="unit">数值单位</param>
        /// <returns></returns>
        public series CreateSeriesByDataTable(DataTable dt, string seriesNm, string seriesColor, Dictionary<string, string> colnm_color, DataLabels dl, int maxY, string unit = "")
        {
            series s = new series() { name = seriesNm, color = seriesColor, data = new List<ChartNode>(), dataLabels = dl };
            if (colnm_color == null || dt == null || dt.Rows.Count == 0) return s;
            int x = 0;
            foreach (string colnm in colnm_color.Keys)
            {
                double value = CommonFunction.ObjectToDouble(dt.Rows[0][colnm]);
                ChartNode cn = new ChartNode();
                if (string.IsNullOrEmpty(unit))
                {
                    cn.name = value.ToString();
                }
                else
                {
                    cn.name = string.Format("{0:N2}{1}", value, unit);
                }
                cn.x = x;
                if (maxY != -1 && value > maxY)
                {
                    cn.y = maxY;
                }
                else
                {
                    cn.y = value;
                }
                cn.color = colnm_color[colnm];

                x++;
                s.data.Add(cn);
            }
            return s;
        }
        /// 将DataTable里面的数据转换为series
        /// <summary>
        /// 将DataTable里面的数据转换为series
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<series> ConvertDt2Series4CallOutConnected(DataTable dt)
        {
            List<series> lst = new List<series>();
            series sCalloutTotal = new series() { name = "外呼量", color = "#ff9900", data = new List<ChartNode>() };
            series sConnectedTotal = new series() { name = "接通量", color = "#33a187", data = new List<ChartNode>() };
            if (dt.Rows != null && dt.Rows.Count > 0)
            {
                int i = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    sCalloutTotal.data.Add(new ChartNode() { x = i, y = CommonFunction.ObjectToInteger(dr[1]) });
                    sConnectedTotal.data.Add(new ChartNode() { x = i, y = CommonFunction.ObjectToInteger(dr[2]) });
                    i++;
                }

            }
            lst.Add(sCalloutTotal);
            lst.Add(sConnectedTotal);
            return lst;
        }
        /// 生成数据点标注提示
        /// <summary>
        /// 生成数据点标注提示
        /// </summary>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <param name="fontname"></param>
        /// <param name="fontbold"></param>
        /// <param name="align"></param>
        /// <param name="rotation"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public DataLabels CreateDataLabels(string color, int size, string fontname, bool fontbold, string fontcolor, string align, int rotation, int x, int y)
        {
            DataLabels dl = new DataLabels() { enabled = true };
            dl.color = color;
            dl.rotation = rotation;
            dl.x = x;
            dl.y = y;
            dl.align = align;
            dl.style = new Style();
            dl.style.fontFamily = fontname;
            dl.style.fontSize = size + "px";
            dl.style.fontWeight = fontbold ? "bolder" : null;
            dl.style.color = fontcolor;
            return dl;
        }
        /// 获取线图数据
        /// <summary>
        /// 获取线图数据
        /// </summary>
        /// <param name="dtNow"></param>
        /// <param name="bgid"></param>
        /// <returns></returns>
        public string GetCallConnectData(string dtNow, string bgid)
        {
            List<series> lst = ConvertDt2Series4CallOutConnected(Routepoint.Instance.GetCallOutConnectedInfoData(dtNow, bgid));
            CommonChartResult res = new CommonChartResult() { data = lst, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 返回颜色
        /// <summary>
        /// 返回颜色
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetColor(int r, int g, int b)
        {
            return "rgb(" + r + "," + g + "," + b + ")";
        }
        #endregion

        #region 大屏一期
        #region 第一屏
        /// 左上
        /// <summary>
        /// 左上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport1_1()
        {
            //个人
            string bgid = ConfigurationUtil.GetAppSettingValue("个人热线组", false);

            DataTable dt = Routepoint.Instance.GetRealTimeQueueData(2, bgid);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("n_entered_out", GetColor(126, 172, 208));
            colnm_color.Add("Call_num", GetColor(82, 111, 177));
            colnm_color.Add("Free_num", GetColor(0, 185, 206));
            colnm_color.Add("Busy_num", GetColor(2, 114, 152));
            colnm_color.Add("ACW_num", GetColor(156, 170, 215));
            colnm_color.Add("Online_num", GetColor(81, 147, 225));
            series s = CreateSeriesByDataTable(dt, "个人热线 - 实时队列", null, colnm_color, null, MaxTop1_L);
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 左下
        /// <summary>
        /// 左下
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport1_2()
        {
            //企业
            string bgid = ConfigurationUtil.GetAppSettingValue("企业热线组", false);

            DataTable dt = Routepoint.Instance.GetRealTimeQueueData(1, bgid);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("n_entered_out", GetColor(126, 172, 208));
            colnm_color.Add("Call_num", GetColor(82, 111, 177));
            colnm_color.Add("Free_num", GetColor(0, 185, 206));
            colnm_color.Add("Busy_num", GetColor(2, 114, 152));
            colnm_color.Add("ACW_num", GetColor(156, 170, 215));
            colnm_color.Add("Online_num", GetColor(81, 147, 225));
            series s = CreateSeriesByDataTable(dt, "企业热线 - 实时队列", null, colnm_color, null, MaxTop1_L);
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 右上
        /// <summary>
        /// 右上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport1_3()
        {
            //个人
            string bgid = ConfigurationUtil.GetAppSettingValue("个人热线组", false);
            string dtNow = string.IsNullOrEmpty(MonitorDate) ? DateTime.Now.ToString("yyyy-MM-dd") : MonitorDate;

            DataTable dt = Routepoint.Instance.GetIndicatorsComData(2, bgid, dtNow);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("pc_n_answered", GetColor(243, 108, 79));
            colnm_color.Add("pc_n_distrib_in_tr", GetColor(210, 64, 135));
            colnm_color.Add("jiejue_rate", GetColor(215, 36, 66));
            colnm_color.Add("manyi_rate", GetColor(178, 31, 65));
            series s = CreateSeriesByDataTable(dt, "个人热线 - 指标完成", null, colnm_color, null, MaxTop1_R, "%");
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 右下
        /// <summary>
        /// 右下
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport1_4()
        {
            //企业
            string bgid = ConfigurationUtil.GetAppSettingValue("企业热线组", false);
            string dtNow = string.IsNullOrEmpty(MonitorDate) ? DateTime.Now.ToString("yyyy-MM-dd") : MonitorDate;

            DataTable dt = Routepoint.Instance.GetIndicatorsComData(1, bgid, dtNow);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("pc_n_answered", GetColor(243, 108, 79));
            colnm_color.Add("pc_n_distrib_in_tr", GetColor(210, 64, 135));
            colnm_color.Add("jiejue_rate", GetColor(215, 36, 66));
            colnm_color.Add("manyi_rate", GetColor(178, 31, 65));
            series s = CreateSeriesByDataTable(dt, "企业热线 - 指标完成", null, colnm_color, null, MaxTop1_R, "%");
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        #endregion

        #region 第二屏
        /// 左上
        /// <summary>
        /// 左上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport2_1()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("智能平台运营支持部", false);

            DataTable dt = Routepoint.Instance.GetRealTimeMonitoring(bgid);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("Call_num", GetColor(153, 204, 204));
            colnm_color.Add("Free_num", GetColor(0, 166, 142));
            colnm_color.Add("Busy_num", GetColor(117, 186, 17));
            colnm_color.Add("Online_num", GetColor(90, 195, 188));
            series s = CreateSeriesByDataTable(dt, "呼出业务队列实时监控 - 智能平台运营支持部", null, colnm_color, null, MaxTop2_1);
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 左下
        /// <summary>
        /// 左下
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport2_2()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("二手车运营支持部", false);

            DataTable dt = Routepoint.Instance.GetRealTimeMonitoring(bgid);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("Call_num", GetColor(153, 204, 204));
            colnm_color.Add("Free_num", GetColor(0, 166, 142));
            colnm_color.Add("Busy_num", GetColor(117, 186, 17));
            colnm_color.Add("Online_num", GetColor(90, 195, 188));
            series s = CreateSeriesByDataTable(dt, "呼出业务队列实时监控 - 二手车运营支持部", null, colnm_color, null, MaxTop2_2);
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 右上
        /// <summary>
        /// 右上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport2_3()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("智能平台运营支持部", false);
            string dtNow = string.IsNullOrEmpty(MonitorDate) ? DateTime.Now.ToString("yyyy-MM-dd") : MonitorDate;
            return GetCallConnectData(dtNow, bgid);
        }
        /// 右下
        /// <summary>
        /// 右下
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport2_4()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("二手车运营支持部", false);
            string dtNow = string.IsNullOrEmpty(MonitorDate) ? DateTime.Now.ToString("yyyy-MM-dd") : MonitorDate;
            return GetCallConnectData(dtNow, bgid);
        }
        #endregion

        #region 第三屏
        /// 左上
        /// <summary>
        /// 左上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport3_1()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("线索运营支持部", false);

            DataTable dt = Routepoint.Instance.GetRealTimeMonitoring(bgid);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("Call_num", GetColor(82, 111, 177));
            colnm_color.Add("Free_num", GetColor(154, 192, 221));
            colnm_color.Add("Busy_num", GetColor(210, 64, 135));
            colnm_color.Add("Online_num", GetColor(243, 108, 79));
            series s = CreateSeriesByDataTable(dt, "呼出业务队列实时监控 - 线索运营支持部", null, colnm_color, null, MaxTop3_1);
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 左下
        /// <summary>
        /// 左下
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport3_2()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("汽车金融服务部", false);

            DataTable dt = Routepoint.Instance.GetRealTimeMonitoring(bgid);
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("Call_num", GetColor(82, 111, 177));
            colnm_color.Add("Free_num", GetColor(154, 192, 221));
            colnm_color.Add("Busy_num", GetColor(210, 64, 135));
            colnm_color.Add("Online_num", GetColor(243, 108, 79));
            series s = CreateSeriesByDataTable(dt, "呼出业务队列实时监控 - 汽车金融服务部", null, colnm_color, null, MaxTop3_2);
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 右上
        /// <summary>
        /// 右上
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport3_3()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("线索运营支持部", false);
            string dtNow = string.IsNullOrEmpty(MonitorDate) ? DateTime.Now.ToString("yyyy-MM-dd") : MonitorDate;
            return GetCallConnectData(dtNow, bgid);
        }
        /// 右下
        /// <summary>
        /// 右下
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetBigScreenReport3_4()
        {
            string bgid = ConfigurationUtil.GetAppSettingValue("汽车金融服务部", false);
            string dtNow = string.IsNullOrEmpty(MonitorDate) ? DateTime.Now.ToString("yyyy-MM-dd") : MonitorDate;
            return GetCallConnectData(dtNow, bgid);
        }
        #endregion

        #region 第四屏
        [HttpGet]
        public string GetBigScreenReport4(string Action, string r)
        {
            string msg = "";
            string dtNow = string.IsNullOrEmpty(MonitorDate) ? DateTime.Now.ToString("yyyy-MM-dd") : MonitorDate;
            DataTable dt = new DataTable();
            switch (Action)
            {
                case "1":
                    dt = Routepoint.Instance.GetFourScreenData(ConfigurationUtil.GetAppSettingValue("线索运营支持部", false), dtNow);
                    break;
                case "2":
                    dt = Routepoint.Instance.GetFourScreenData(ConfigurationUtil.GetAppSettingValue("智能平台运营支持部", false), dtNow);
                    break;
                case "3":
                    dt = Routepoint.Instance.GetFourScreenData(ConfigurationUtil.GetAppSettingValue("汽车金融服务部", false), dtNow);
                    break;
                case "4":
                    dt = Routepoint.Instance.GetFourScreenData(ConfigurationUtil.GetAppSettingValue("二手车运营支持部", false), dtNow);
                    break;
            }

            int rowCount = dt.Rows.Count;

            if (rowCount < 10)
            {
                int addCount = 10 - rowCount;
                for (int i = 0; i < addCount; i++)
                {
                    DataRow newR = dt.NewRow();
                    newR[0] = "TOP" + (++rowCount);
                    newR[1] = "-1";
                    dt.Rows.Add(newR);
                }
            }

            int j = 0;
            foreach (DataRow dr in dt.Select("", "TotalCount desc"))
            {
                if (j == 0)
                {
                    msg += "[";
                }
                if (j == dt.Rows.Count - 1)
                {
                    msg += "{TrueName:'" + dr["TrueName"].ToString() + "',TotalCount:'" + dr["TotalCount"].ToString() + "'}]";
                }
                else
                {
                    msg += "{TrueName:'" + dr["TrueName"].ToString() + "',TotalCount:'" + dr["TotalCount"].ToString() + "'},";
                }
                j++;
            }
            return msg;
        }
        #endregion
        #endregion

        #region 大屏二期
        /// 根据热线获取实时队列数据：Waiting Call Free Busy Acw Online CSR
        /// <summary>
        /// 根据热线获取实时队列数据：Waiting Call Free Busy Acw Online CSR
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetHotLineRealInfo(string hl)
        {
            Dictionary<string, string> HotLineDic = GetHotLineInfo(ref hl);
            DataTable dt = null;

            //查询数据库，获取Call Free Busy Acw Online CSR
            if (hl == "北京测试")
            {
                #region 测试代码
                //北京测试热线的分机号范围：3001-3010
                dt = Routepoint.Instance.GetHotLineRealInfo_BJTest(3010, 3001);
                //北京测试热线使用的是87237722的技能组，所以在Holly服务连接192.168.15.84的情况下取87237722的Waiting值
                hl = "87237722";
                #endregion
            }
            else
            {
                dt = Routepoint.Instance.GetHotLineRealInfo(hl);
            }

            //查询mogodb，获取Waiting
            dt.Columns.Add("n_entered_out");
            Dictionary<string, SyncHelyBigScreenData.HotLineInf> dicHotLineInf = SyncHelyBigScreenData.getHotLineDictionary();

            //数据合并
            int n_entered_out = (dicHotLineInf != null && dicHotLineInf.ContainsKey(hl)) ? dicHotLineInf[hl].ContactInQueueNum : 0;
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["n_entered_out"] = n_entered_out;
            }

            //界面赋值
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("n_entered_out", GetColor(126, 172, 208));
            colnm_color.Add("Call_num", GetColor(82, 111, 177));
            colnm_color.Add("Free_num", GetColor(0, 185, 206));
            colnm_color.Add("Busy_num", GetColor(2, 114, 152));
            colnm_color.Add("ACW_num", GetColor(156, 170, 215));
            colnm_color.Add("Online_num", GetColor(81, 147, 225));
            series s = CreateSeriesByDataTable(dt, HotLineDic[hl] + " - 实时队列", null, colnm_color, null, MaxTop1_L);
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        /// 根据热线获取指标完成数据：接通率 30s服务水平 问题解决率 客户满意率
        /// <summary>
        /// 根据热线获取指标完成数据：接通率 30s服务水平 问题解决率 客户满意率
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetHotLineStateInfo(string hl)
        {
            Dictionary<string, string> HotLineDic = GetHotLineInfo(ref hl);

            if (hl == "北京测试")
            {
                #region 测试代码
                //北京测试热线，落地号码
                hl = "58103008";
                HotLineDic["58103008"] = "北京测试";
                #endregion
            }

            //查询数据库：问题解决率 客户满意率
            DataTable dt = Routepoint.Instance.GetHotLineStateInfo(hl);

            //查询合力报表库：接通率 30s服务水平
            dt.Columns.Add("pc_n_answered");
            dt.Columns.Add("pc_n_distrib_in_tr");
            Dictionary<string, SyncHelyBigScreenData.HotLineInf> dicHotLineInf = SyncHelyBigScreenData.getHotLineDictionary();

            //合并数据
            var pc_n_answered = (dicHotLineInf != null && dicHotLineInf.ContainsKey(hl)) ? dicHotLineInf[hl].pc_n_answered : 0;
            var pc_n_distrib_in_tr = (dicHotLineInf != null && dicHotLineInf.ContainsKey(hl)) ? dicHotLineInf[hl].pc_n_distrib_in_tr : 0;
            if (dt.Rows.Count > 0)
            {
                dt.Rows[0]["pc_n_answered"] = pc_n_answered;
                dt.Rows[0]["pc_n_distrib_in_tr"] = pc_n_distrib_in_tr;
            }

            //界面赋值
            Dictionary<string, string> colnm_color = new Dictionary<string, string>();
            colnm_color.Add("pc_n_answered", GetColor(243, 108, 79));
            colnm_color.Add("pc_n_distrib_in_tr", GetColor(210, 64, 135));
            colnm_color.Add("jiejue_rate", GetColor(215, 36, 66));
            colnm_color.Add("manyi_rate", GetColor(178, 31, 65));
            series s = CreateSeriesByDataTable(dt, HotLineDic[hl] + " - 指标完成", null, colnm_color, null, MaxTop1_R, "%");
            CommonChartResult res = new CommonChartResult() { data = new List<series>() { s }, ErrorMsg = "", Success = true, ErrorNumber = 0 };
            return JsonConvert.SerializeObject(res, Formatting.Indented, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }

        /// 获取热线信息，转换查询关键字
        /// <summary>
        /// 获取热线信息，转换查询关键字
        /// </summary>
        /// <param name="hl"></param>
        /// <returns></returns>
        private static Dictionary<string, string> GetHotLineInfo(ref string hl)
        {
            hl = System.Web.HttpUtility.UrlDecode(hl);
            Dictionary<string, string> HotLineDic = new Dictionary<string, string>();
            HotLineDic = CommonFunction.GetAllNodeContentByFile<string, string>(AppDomain.CurrentDomain.BaseDirectory + "1-热线数据统计.xml", "key", "name", null);
            //兼容名称和key
            if (HotLineDic.Keys.Contains(hl))
            {
                //不变
            }
            else if (HotLineDic.Values.Contains(hl))
            {
                string value = hl;
                hl = HotLineDic.FirstOrDefault(o => o.Value == value).Key;
            }
            return HotLineDic;
        }
        #endregion
    }
}
