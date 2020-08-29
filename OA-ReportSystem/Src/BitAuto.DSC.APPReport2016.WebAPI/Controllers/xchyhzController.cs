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
using BitAuto.DSC.APPReport2016.Entities;

namespace BitAuto.DSC.APPReport2016.WebAPI.Controllers
{
    [BitAuto.DSC.APPReport2016.WebAPI.Filter.LoginAuthorize(CheckReportRight = "SYS020MOD1007")]
    public class xchyhzController : ApiController
    {
        private log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name);

        ///获取新车会员数（日）-柱图标题
        /// <summary>
        ///获取新车会员数（日）-柱图标题
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="ItemId">业务线</param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMemberHZForDay(string ItemId)
        {
            //查询天表-MemberDaily，获取当前时间的合作数和当年最大的合作数
            DataSet ds = BLL.MemberDaily.Instance.GetMemberHZForDay(CommonFunction.ObjectToInteger(ItemId));
            DataTable dt1 = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];
            //返回值
            string date = DateTime.Today.ToString("yyyy-MM-dd");
            string name = "";
            int count = 0;
            int maxcount = 0;
            if (dt1.Rows.Count > 0)
            {
                date = CommonFunction.ObjectToDateTime(dt1.Rows[0]["Date"]).ToString("yyyy-MM-dd");
                name = dt1.Rows[0]["NAME"].ToString();
                count = CommonFunction.ObjectToInteger(dt1.Rows[0]["Count"]);
            }
            if (dt2.Rows.Count > 0)
            {
                maxcount = CommonFunction.ObjectToInteger(dt2.Rows[0]["Count"]);
            }
            var result = new { ItemId = ItemId, date = date, name = name, count = count, maxcount = maxcount };
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);
        }
        /// 获取新车会员数（月）-柱图
        /// <summary>
        /// 获取新车会员数（月）-柱图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMemberHZForMonth()
        {
            //查询Common.DataConfig.MemberTypeAll当前年12月之前的全部合作数据
            DateTime date = new DateTime(DateTime.Today.Year, 12, 1);
            DataTable dt = BLL.MemberDaily.Instance.GetMemberHZForMonth(date, Common.DataConfig.MemberTypeAll);
            //构造数据
            List<string> seriesname = new List<string>();
            List<string> datakey = new List<string>();
            List<List<decimal?>> dataval = new List<List<decimal?>>();
            List<int> zoomindex = new List<int>();
            //辅助
            List<int> seriesids = new List<int>();
            List<decimal?> yugu = new List<decimal?>();//1是0否

            //1-构造seriesname
            foreach (DataRow dr in dt.DefaultView.ToTable(true, "ItemId", "NAME").Select("", "ItemId"))
            {
                seriesname.Add(dr["NAME"].ToString());
                seriesids.Add(CommonFunction.ObjectToInteger(dr["ItemId"]));
            }
            //2-构造datakey
            foreach (DataRow dr in dt.DefaultView.ToTable(true, "YearMonth").Select("", "YearMonth"))
            {
                int yearmonth = CommonFunction.ObjectToInteger(dr["YearMonth"]);
                int i_year = yearmonth / 100;
                int i_month = yearmonth % 100;
                datakey.Add(i_year + "-" + i_month.ToString("00"));

                //大于当前月，是预估数据
                if (i_year == DateTime.Now.Year && i_month > DateTime.Now.Month)
                {
                    yugu.Add(1);
                }
                else
                {
                    yugu.Add(0);
                }
            }

            //3-构造datakey，缺失数据用null补足，取Count数据
            foreach (int itemid in seriesids)
            {
                //一个系列的数据
                List<decimal?> seriesdatas = new List<decimal?>();
                foreach (string yearmonth in datakey)
                {
                    //某个坐标点的数据
                    //搜索数据
                    DataRow[] drs = dt.Select("YearMonth=" + yearmonth.Replace("-", "") + " and ItemId=" + itemid);
                    if (drs.Length > 0)
                    {
                        seriesdatas.Add(Common.Common.Object2DecimalNullable(drs[0]["Count"]));
                    }
                    else
                    {
                        //没有数据，用null占位
                        seriesdatas.Add(null);
                    }
                }
                //添加系列数据
                dataval.Add(seriesdatas);
            }
            //最后添加预估数据
            dataval.Add(yugu);
            //4-计算缩放位置
            zoomindex = new List<int>(Common.Common.GetZoomIndex(datakey.Count));

            var result = new { seriesname = seriesname, datakey = datakey, dataval = dataval, zoomindex = zoomindex };
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);
        }

        /// 会员贡献值（平均）-线图标题
        /// <summary>
        /// 会员贡献值（平均）-线图标题
        /// </summary>
        /// <param name="year"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetAvgAmount(int year = 0)
        {
            ReturnMessage result = new ReturnMessage();
            result.Success = true;

            if (year <= 0)
            {
                //获取最大时间
                DateTime date = BLL.Member.Instance.GetMaxDate(Common.DataConfig.MemberTypeAll);
                year = date.Year;
            }

            DataTable dt = BLL.MemberArpu.Instance.GetData(year, 0, "0," + Common.DataConfig.MemberTypeAll);
            DataRow[] drs;

            string allavg = "";
            string cmtavg = "";
            string cytavg = "";
            string wxtavg = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                drs = dt.Select("ItemId='0'");
                if (drs.Length > 0)
                {
                    allavg = drs[0]["Arpu"].ToString();
                }

                drs = dt.Select("ItemId='" + Common.DataConfig.CmtCode + "'");
                if (drs.Length > 0)
                {
                    cmtavg = drs[0]["Arpu"].ToString();
                }

                drs = dt.Select("ItemId='" + Common.DataConfig.CytCode + "'");
                if (drs.Length > 0)
                {
                    cytavg = drs[0]["Arpu"].ToString();
                }

                drs = dt.Select("ItemId='" + Common.DataConfig.WxtCode + "'");
                if (drs.Length > 0)
                {
                    wxtavg = drs[0]["Arpu"].ToString();
                }
            }

            result.Success = true;
            result.Data = new { year = year, allavg = allavg, cmtavg = cmtavg, cytavg = cytavg, wxtavg = wxtavg };
            result.Message = "成功";


            log.Debug(JsonConvert.SerializeObject(result));
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);
        }
        /// 获取会员贡献值（季度）-线图
        /// <summary>
        /// 获取会员贡献值（季度）-线图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMemberAmountForQuarter(int year = 0)
        {
            ReturnMessage result = new ReturnMessage();
            result.Success = true;

            if (year <= 0)
            {
                //获取最大时间
                DateTime date = BLL.Member.Instance.GetMaxDate(Common.DataConfig.MemberTypeAll);
                year = date.Year;
            }

            List<string> seriesname = new List<string>();
            List<string> datakey = new List<string>();
            List<List<string>> dataval = new List<List<string>>();

            //seriesname===============================================
            DataTable dtitem = BLL.DictInfo.Instance.GetData("", Common.DataConfig.MemberTypeCode);
            if (dtitem != null)
            {
               // seriesname.Add("总体");
                for (int i = 0; i < dtitem.Rows.Count; i++)
                {
                    seriesname.Add(dtitem.Rows[i]["DictName"].ToString());
                }
            }

            //datakey 一年4个季度========================================
            Dictionary<string, string> dic = Common.DataConfig.GetYearQuarter(year);
            foreach (var item in dic)
            {
                datakey.Add(year + item.Key);
            }

            //数据======================================================
            DataTable dt = BLL.MemberArpu.Instance.GetData(year, -1, "0," + Common.DataConfig.MemberTypeAll);
            DataRow[] drs;
            if (dt != null && dt.Rows.Count > 0 && dtitem != null && dtitem.Rows.Count > 0)
            {
                string amount = "";
                string dictId = "";
                List<string> itemvalue = new List<string>();
                //总体========================
                //for (int i = 1; i <= 4; i++)
                //{
                //    //计算每个季度的贡献额
                //    drs = dt.Select("ItemId=0 and quarter='" + i + "'");
                //    if (drs.Length > 0)
                //    {
                //        amount = drs[0]["Arpu"].ToString();
                //        itemvalue.Add(amount);
                //    }
                //}
                //dataval.Add(itemvalue);

                //单个=========================
                for (int i = 0; i < dtitem.Rows.Count; i++)
                {
                    dictId = dtitem.Rows[i]["DictId"].ToString();
                    itemvalue = new List<string>();
                    for (int j = 1; j <= 4; j++)
                    {
                        //计算每个季度的贡献额
                        amount = "";
                        drs = dt.Select("ItemId=" + dictId + " and quarter='" + j + "'");
                        if (drs.Length > 0)
                        {
                            amount = drs[0]["Arpu"].ToString();
                            itemvalue.Add(amount);
                        }
                    }
                    dataval.Add(itemvalue);
                }

                //返回结果
                result.Success = true;
                result.Data = new { seriesname = seriesname, datakey = datakey, dataval = dataval };
                result.Message = "成功";
            }
            else
            {
                result.Success = false;
                result.Message = "数据为空";
            }

            log.Debug(JsonConvert.SerializeObject(result));
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);
        }

        /// <summary>
        /// 本月市场覆盖率
        /// </summary>
        /// <param name="YearMonth"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMemberFGL(int YearMonth = 0)
        {
            ReturnMessage result = new ReturnMessage();
            result.Success = true;

            if (YearMonth <= 0)
            {
                //获取最大时间
                YearMonth = BLL.MemberByDepart.Instance.GetMaxDate(Common.DataConfig.MemberTypeAll);
            }

            DataTable dtitem = BLL.DictInfo.Instance.GetData("", Common.DataConfig.MemberTypeCode);
            DataTable dt = BLL.MemberByDepart.Instance.GetDtFGL(YearMonth, YearMonth, Common.DataConfig.MemberTypeAll);
            DataRow[] drs;
            List<object> list = new List<object>();

            if (dtitem != null && dt != null && dt.Rows.Count > 0)
            {
                string percent = "";
                string count = "";
                string total = "";
                for (int i = 0; i < dtitem.Rows.Count; i++)
                {
                    percent = "";
                    count = "";
                    total = "";

                    drs = dt.Select("itemid='" + dtitem.Rows[i]["DictId"].ToString() + "'");
                    if (drs != null && drs.Length > 0)
                    {
                        try
                        {
                            count = drs[0]["Count"].ToString();
                            total = drs[0]["Total"].ToString();
                            percent = Math.Round(Decimal.Parse(count) / Decimal.Parse(total), 4).ToString();
                        }
                        catch { }
                    }

                    list.Add(new { Name = dtitem.Rows[i]["DictName"].ToString(), TypeID = dtitem.Rows[i]["DictId"].ToString(), Percent = percent, Count = count, Total = total });
                }

            }
            result.Success = true;
            result.Data = list;
            result.Message = "成功";


            log.Debug(JsonConvert.SerializeObject(result));
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(result);
        }
        /// 市场覆盖率排名
        /// <summary>
        /// 市场覆盖率排名
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMemmberDepartFGL(string itemId = "", string yearMonth = "", string orderBy = "", int pageIndex = 1, int pageSize = 10, int lastno = 0)
        {
            //如果会议id为空，取全部的会员
            if (string.IsNullOrWhiteSpace(itemId))
            {
                itemId = Common.DataConfig.MemberTypeAll;
            }
            //获取当前会员在数据库中的最大时间
            if (string.IsNullOrWhiteSpace(yearMonth))
            {
                yearMonth = BLL.MemberByDepart.Instance.GetMaxDate(itemId).ToString();
            }
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            int totalCount = 0;
            DataTable dt = BLL.MemberByDepart.Instance.GetData(itemId, yearMonth, orderBy, pageIndex, pageSize, out totalCount);
            int newlastno = BLL.Util.SetNoForDataTable(ref dt, orderBy, totalCount, lastno);
            var dataObj = new { pageIndex = pageIndex, totalCount = totalCount, data = dt, lastno = newlastno };
            return BitAuto.DSC.OASysRightManager2016.Common.Common.Util.GetJsonDataByResult(dataObj);
        }
    }
}
