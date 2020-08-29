using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XYAuto.ITSC.Chitunion2017.BLL;
using System.Net.Http;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using XYAuto.ITSC.Chitunion2017.BLL.Media.Verification;

namespace XYAuto.ITSC.Chitunion2017.Test
{
    public class UtilZKB
    {
        /// <summary>
        /// 校验IP是否正确
        /// </summary>
        /// <returns></returns>
        public static bool CheckIP()
        {
            try
            {
                var IPSIgnore = "127.0.222.*,192.168.112.*,192.168.0.*,127.0.0.*" .Split(',');
                string userIP = HttpContext.Current.Request.UserHostAddress;
                userIP = userIP.LastIndexOf(".") > 0 ? userIP.Substring(0, userIP.LastIndexOf(".")) + "." : userIP;

                if (!string.IsNullOrEmpty(IPSIgnore.FirstOrDefault(s => s.StartsWith(userIP))))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[CheckIP]报错", ex);
            }
            return false;
        }

        /// <summary>
        /// 根据查询结果，返回通用Json格式
        /// </summary>
        /// <param name="obj">查询结果</param>
        /// <param name="message">返回Message信息，默认为：OK</param>
        /// <param name="status">返回status信息，默认为：0</param>
        /// <returns>返回Json数据</returns>
        public static JsonResult GetJsonDataByResult(object obj, string message = "OK", int status = 0)
        {
            var jr = new JsonResult();
            jr.Status = status;
            jr.Message = message;
            if (obj != null)
            {
                jr.Result = obj;
            }

            //try
            //{
            //    System.Web.HttpContext webHttp = System.Web.HttpContext.Current;
            //    int loginUserid = int.Parse(webHttp.Session["userid"].ToString());
            //    //jr.IsOverdue = XYAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.GetIsOverdueByUserID(loginUserid);
            //    //XYAuto.YanFa.OASysRightManager2011.Common.EmployeeInfo.Instance.UpdateUserLoginKeyByUserID(loginUserid);
            //}
            //catch (Exception ex)
            //{
            //    Loger.Log4Net.Error("[GetJsonDataByResult]报错", ex);
            //}
            //IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            //timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            //return JsonConvert.SerializeObject(jr, Formatting.Indented, timeFormat);
            //return new HttpResponseMessage
            //{
            //    Content = new StringContent(JsonConvert.SerializeObject(jr, Formatting.Indented, timeFormat), System.Text.Encoding.UTF8, "application/json")
            //};

            return jr;
        }

        /// <summary>
        /// Auth:lixiong
        /// 查询参数校验
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public static JsonResult Verify<T>(T requestDto) where T : class, new()
        {
            var jsonResult = new JsonResult() { Message = "请输入参数", Status = 500 };
            if (requestDto == null)
                return jsonResult;
            var retValue = new VerifyOfNecessaryParameters<T>().VerifyNecessaryParameters(requestDto);
            if (retValue.HasError)
            {
                jsonResult.Message = retValue.Message;
                return jsonResult;
            }
            jsonResult.Message = "success";
            jsonResult.Status = 0;
            return jsonResult;
        }
    }
}