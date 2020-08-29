using System;
using System.Linq;
using System.Web;
using XYAuto.ChiTu2018.API.Models;
using XYAuto.ChiTu2018.Infrastructure.VerifyArgs;
using XYAuto.CTUtils.Log;

namespace XYAuto.ChiTu2018.API.Common
{
    /// <summary>
    /// 
    /// </summary>
    internal class Util
    {
        /// <summary>
        /// 校验IP是否正确
        /// </summary>
        /// <returns></returns>
        public static bool CheckIP()
        {
            try
            {
                var IPSIgnore = System.Configuration.ConfigurationManager.AppSettings["IPSIgnore"].ToString().Split(',');
                string userIP = HttpContext.Current.Request.UserHostAddress;
                userIP = userIP.LastIndexOf(".") > 0 ? userIP.Substring(0, userIP.LastIndexOf(".")) + "." : userIP;

                if (!string.IsNullOrEmpty(IPSIgnore.FirstOrDefault(s => s.StartsWith(userIP))))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Default().Error("[CheckIP]报错", ex);
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

        public static string GetIP()
        {
            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(result))
            {
                Log4NetHelper.Default().Info($"获取当前客户端ID：HTTP_X_FORWARDED_FOR={result}");
                result = result.ToString().Split(',')[0].Trim();
            }
            Log4NetHelper.Default().Info($"获取当前客户端ID：REMOTE_ADDR={HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]}");
            Log4NetHelper.Default().Info($"获取当前客户端ID：UserHostAddress={HttpContext.Current.Request.UserHostAddress}");
            if (string.IsNullOrEmpty(result))
            {
                //否则直接读取REMOTE_ADDR获取客户端IP地址
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            if (!string.IsNullOrEmpty(result) &&
                System.Text.RegularExpressions.Regex.IsMatch(result, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$"))
            {
                //BLL.Loger.Log4Net.Info($"获取当前客户端ID：UserHostAddress={result}");
                return result;
            }
            return "0.0.0.0";
        }
    }
}