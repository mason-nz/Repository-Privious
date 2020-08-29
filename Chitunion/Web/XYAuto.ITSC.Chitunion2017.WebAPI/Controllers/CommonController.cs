using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XYAuto.ITSC.Chitunion2017.WebAPI.Common;

namespace XYAuto.ITSC.Chitunion2017.WebAPI.Controllers
{
    public class CommonController : ApiController
    {
        //public string RequestJsonpCallback
        //{
        //    get {
        //        return BLL.Util.GetCurrentRequestQueryStr("jsonpcallback");
        //    }
        //}

        /// <summary>
        /// 获取节假日信息
        /// </summary>
        /// <returns>返回json格式，包含开始、结束时间，节假日名称</returns>
        [HttpGet]
        //[LoginAuthorize(IsCheckIP = false, IsCheckLogin = true)]
        public JsonResult GetHolidaysInfo()
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();

            DataTable dt =BLL.HolidaysInfo.Instance.GetHolidaysInfo();
            if (dt!=null)
            {
                dt.DefaultView.RowFilter = "";
                DataTable dtDistinctYear = dt.DefaultView.ToTable(true, "CurYear");
                if (dtDistinctYear!=null)
                {
                    foreach (DataRow dr in dtDistinctYear.Rows)
                    {
                        int year = int.Parse(dr["CurYear"].ToString());
                        dt.DefaultView.RowFilter = "CurYear="+year;
                        DataTable dtYear= dt.DefaultView.ToTable(false, new string[]{ "StartDate","EndDate","Name"});
                        result.Add(year.ToString(), dtYear);
                    }
                }
            }

            return Util.GetJsonDataByResult(result, "Success");
            //return RequestJsonpCallback + "(" + JsonConvert.SerializeObject(result) + ")";
        }
    }
}
