using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BitAuto.DSC.OASysRightManager2016.Common.WebAPI;
using System.Data;
using log4net;
using Newtonsoft.Json;
using System.Web;
using BitAuto.Utils.Config;
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{
    public class EmpolyeeController : ApiController
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);
        /// <summary>
        /// 获取当前登录人有权限的报表信息
        /// </summary>
        /// <returns></returns> 
        [LoginAuthorize(IsCheckIP = true, IsCheckLogin = true)]
        public HttpResponseMessage GetRightReportPageInfo()
        {
            try
            {
                int loginUserId = BitAuto.YanFa.OASysRightManager2011.Common.Util.GetLoginUserID();
                string sysId = ConfigurationUtil.GetAppSettingValue("ThisSysID", false);
                string strBaseUrl = ConfigurationUtil.GetAppSettingValue("BaseUrl", false);
                string strReportModelId = ConfigurationUtil.GetAppSettingValue("ReportModuleID", false);
                DataTable dt = BitAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.GetChildModuleByUserId(loginUserId, sysId, strReportModelId);
                if (dt != null && dt.Rows.Count > 0)
                {
                    //var resultData = new
                    //{
                    //    运营报表 = dt.Select("moduleid in ('SYS020MOD1001','SYS020MOD1002','SYS020MOD1003','SYS020MOD1004')").Select(t => new { name = t.Field<string>("modulename"), url = strBaseUrl + t.Field<string>("url") }),
                    //    经营报表 = dt.Select("moduleid in ( 'SYS020MOD1006','SYS020MOD1007','SYS020MOD1008')").Select(t => new { name = t.Field<string>("modulename"), url = strBaseUrl + t.Field<string>("url") }),
                    //    公司管理 = dt.Select("moduleid = 'SYS020MOD1005'").Select(t => new { name = t.Field<string>("modulename"), url = strBaseUrl + t.Field<string>("url") })
                    //};
                    DataColumn col = new DataColumn("parentName", typeof(string));
                    DataColumn col2 = new DataColumn("parentOrder", typeof(int));
                    dt.Columns.Add(col);
                    dt.Columns.Add(col2);
                    foreach (DataRow row in dt.Rows)
                    {
                        string moduleid = row["moduleid"].ToString();
                        switch (moduleid)
                        {
                            case "SYS020MOD1001":
                            case "SYS020MOD1002":
                            case "SYS020MOD1003":
                            case "SYS020MOD1004":
                                row["parentName"] = "运营报表";
                                row["parentOrder"] = 1;
                                break;
                            case "SYS020MOD1006":
                            case "SYS020MOD1007":
                            case "SYS020MOD1008":
                                row["parentName"] = "经营报表";
                                row["parentOrder"] = 2;
                                break;
                            case "SYS020MOD1005":
                                row["parentName"] = "公司管理";
                                row["parentOrder"] = 3;
                                break;
                            default: break;
                        }
                    }
                    return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dt.AsEnumerable().
                        OrderBy(p => p.Field<int>("parentOrder")).
                        Select(t => new { parentname = t.Field<string>("parentName"), name = t.Field<string>("modulename"), url = strBaseUrl + t.Field<string>("url") })
                        );
                }
                else
                {
                    return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new string[] { });
                }
            }
            catch (Exception ex)
            {
                log.Error("GetRightReportPageInfo异常:" + ex.Message.ToString() + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取当前登录人对指定模块的权限
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize(IsCheckIP = true, IsCheckLogin = true)]
        public HttpResponseMessage GetRightModelInfo()
        {
            try
            {
                int loginUserId = BitAuto.YanFa.OASysRightManager2011.Common.Util.GetLoginUserID();
                string[] arrReportModelIds = ConfigurationUtil.GetAppSettingValue("ModuleIds", false).Replace('；', ';').TrimEnd(';').Replace(";", ",").Split(',');
                DataTable dt = BitAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.GetModuleRightByUserIdAndModuleIds(loginUserId, arrReportModelIds);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dt);
                }
                else
                {
                    return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(new string[] { });
                }
            }
            catch (Exception ex)
            {
                log.Error("GetRightModelInfo异常:" + ex.Message.ToString() + ex.StackTrace);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 员工总数和本月变动情况
        /// </summary>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1005")]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        public HttpResponseMessage GetEmpolyeeCount(int YearMonth = 0)
        {

            ReturnMessage result = new ReturnMessage();
            result.Success = true;
            try
            {

                //====================
                if (YearMonth <= 0)
                {
                    //获取最大时间
                    YearMonth = BLL.Empolyee.Instance.GetMaxDate();
                }

                //=============================
                DataTable dt = BLL.Empolyee.Instance.GetData(YearMonth, YearMonth);
                string total = "";
                string entry = "";
                string dimission = "";
                string male = "";
                double malepercent = 0;
                string female = "";
                double femalepercent = 0;
                string yearmonth = "";

                if (dt != null && dt.Rows.Count > 0)
                {
                    yearmonth = dt.Rows[0]["yearmonth"].ToString();
                    total = dt.Rows[0]["total"].ToString();
                    entry = dt.Rows[0]["entry"].ToString();
                    dimission = dt.Rows[0]["dimission"].ToString();
                    male = dt.Rows[0]["male"].ToString();
                    female = dt.Rows[0]["female"].ToString();

                    malepercent = Math.Round(double.Parse(male) / double.Parse(total), 6);
                    femalepercent = Math.Round(double.Parse(female) / double.Parse(total), 6);

                    result.Success = true;
                    result.Data = new { yearmonth = yearmonth, total = total, entry = entry, dimission = dimission, male = male, female = female, malepercent = malepercent, femalepercent = femalepercent };
                    result.Message = "成功";
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "失败";
                log.Error("异常:" + ex.Message.ToString());
            }

            log.Debug(JsonConvert.SerializeObject(result));
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);

        }

        /// <summary>
        /// 员工总数和本月变动情况
        /// </summary>
        /// <param name="StartDate"></param> 
        /// <param name="EndDate"></param>
        /// <returns></returns>
        [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1005")]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        public HttpResponseMessage GetEmpolyeeForMonth(string StartDate = "", string EndDate = "")
        {

            ReturnMessage result = new ReturnMessage();
            result.Success = true;

            DateTime time1;
            DateTime time2;

            #region 参数处理
            // 显示12个月数据，间隔11月
            if (!string.IsNullOrWhiteSpace(StartDate))
            {
                time1 = DateTime.Parse(StartDate);
                time2 = DateTime.Parse(StartDate).AddMonths(11);
            }
            else if (!string.IsNullOrWhiteSpace(EndDate))
            {
                time1 = DateTime.Parse(EndDate).AddMonths(-11);
                time2 = DateTime.Parse(EndDate);
            }
            else
            {
                time1 = DateTime.Now.AddMonths(-11);
                time2 = DateTime.Now;
            }
            #endregion

            List<string> seriesname = new List<string>();
            List<string> datakey = new List<string>();
            List<List<string>> dataval = new List<List<string>>();

            seriesname.Add("员工数");
            seriesname.Add("增长率");

            DataTable dt = BLL.Empolyee.Instance.GetData(int.Parse(time1.ToString("yyyyMM")), int.Parse(time2.ToString("yyyyMM")));

            List<string> value1 = new List<string>();
            List<string> value2 = new List<string>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    datakey.Add(ToDate(dt.Rows[i]["yearmonth"].ToString()).ToString("yyyy-MM"));
                    value1.Add(dt.Rows[i]["total"].ToString());
                    value2.Add(dt.Rows[i]["monthbasis"].ToString());
                }

                dataval.Add(value1);
                dataval.Add(value2);

                var min = value2.Min(x => CommonFunction.ObjectToDecimal(x));
                var max = value2.Max(x => CommonFunction.ObjectToDecimal(x));
                var last = value2.Count > 0 ? CommonFunction.ObjectToDecimal(value2[value2.Count - 1]) : 0;

                //返回结果
                result.Success = true;
                result.Data = new { seriesname = seriesname, datakey = datakey, dataval = dataval, markPoint = new decimal[] { min, max, last } };
                result.Message = "成功";
            }

            log.Debug(JsonConvert.SerializeObject(result));
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);
        }

        /// <summary>
        /// 年龄、司龄 、职位、职级 人员分布
        /// </summary>
        /// <param name="ItemType"></param>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1005")]
        [LoginAuthorize(IsCheckIP = false, IsCheckLogin = false)]
        public HttpResponseMessage GetEmpolyeeForType(int ItemType, int YearMonth = 0)
        {
            ReturnMessage result = new ReturnMessage();
            result.Success = true;

            if (ItemType <= 0)
            {
                result.Success = false;
                result.Message = "查询类型错误";
            }

            if (result.Success)
            {
                List<string> seriesname = new List<string>();
                List<string> datakey = new List<string>();
                List<List<string>> dataval = new List<List<string>>();


                //获取最大时间
                if (YearMonth <= 0)
                {
                    YearMonth = BLL.EmployeeItem.Instance.GetMaxDate(ItemType);
                }

                //字典表
                DataTable dtitem = BLL.DictInfo.Instance.GetData("", ItemType.ToString());

                //数据
                DataTable dt = BLL.EmployeeItem.Instance.GetItemData(ItemType, YearMonth);
                DataRow[] drs;
                List<string> value1 = new List<string>();
                List<string> value2 = new List<string>();

                List<object> list = new List<object>();

                if (dtitem != null)
                {
                    string itemname = "";
                    string itemid = "";
                    string count = "";
                    string percent = "";

                    for (int i = 0; i < dtitem.Rows.Count; i++)
                    {
                        itemname = dtitem.Rows[i]["DictName"].ToString();
                        itemid = dtitem.Rows[i]["DictId"].ToString();
                        count = "";
                        percent = "";

                        drs = dt.Select("itemid='" + itemid + "'");
                        if (drs.Length > 0)
                        {
                            count = drs[0]["count"].ToString();
                            percent = drs[0]["percent"].ToString();
                        }

                        datakey.Add(itemname);
                        value1.Add(count);
                        value2.Add(percent);

                        list.Add(new { Name = itemname, ItemId = itemid, Count = count });
                    }
                }

                dataval.Add(value1);
                dataval.Add(value2);

                //返回结果
                result.Success = true;
                result.Message = "成功";
                if (ItemType == 6 || ItemType == 4)
                {
                    result.Data = list;
                }
                else
                {
                    result.Data = new { seriesname = seriesname, datakey = datakey, dataval = dataval };
                }

            }


            log.Debug(JsonConvert.SerializeObject(result));
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);
        }


        private DateTime ToDate(string yearMonth)
        {
            DateTime date;
            try
            {
                if (yearMonth.Length == 6)//yyyyMM 格式
                {
                    yearMonth = yearMonth.Substring(0, 4) + "-" + yearMonth.Substring(4, 2);
                    date = DateTime.Parse(yearMonth);
                }
                else
                {
                    date = DateTime.Parse(yearMonth);
                }

            }
            catch
            {
                date = DateTime.Now;
            }
            return date;
        }

    }
}
