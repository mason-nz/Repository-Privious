using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using XYAuto.ChiTu2018.API.App.Models;
using XYAuto.ChiTu2018.Service.App.AppAuth;

namespace XYAuto.ChiTu2018.API.App.Filter
{
    /// <summary>
    /// 赤兔联盟APP端-对外提供接口签名验证类
    /// Add=masj ，Date=2018-04-26
    /// </summary>
    public class CTAppAuthAttribute : System.Web.Http.Filters.ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            EnumAppAuthRequestVerify enumCodeMsg = new EnumAppAuthRequestVerify();
            try
            {
                //var sr = new StreamReader(HttpContext.Current.Request.InputStream);
                //var stream = sr.ReadToEnd();

                var stream = HttpContext.Current.Request.InputStream;
                stream.Position = 0;
                string requestData = string.Empty;
                using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                {
                    requestData = streamReader.ReadToEndAsync().Result;
                    stream.Position = 0;
                }

                JavaScriptSerializer json = new JavaScriptSerializer();
                var jsonObj = JsonConvert.DeserializeObject<Dictionary<string, object>>(requestData);

                bool flag = AppAuthHelper.Instance.VerifyPara(jsonObj, ref enumCodeMsg);
                flag = flag && AppAuthHelper.Instance.VerifySign(jsonObj, ref enumCodeMsg);
                if (flag)
                {
                    base.OnActionExecuting(actionContext);
                }
                else
                {
                    string logMsg = $"\r\n移动端调用后端接口，验证签名逻辑失败：\r\n请求URL：{HttpContext.Current.Request.Url.AbsoluteUri}\r\n请求方式：{HttpContext.Current.Request.HttpMethod}\r\n请求参数：{stream}\r\n失败原因：{enumCodeMsg.ToString() + $"({(int)enumCodeMsg})"}";
                    XYAuto.CTUtils.Log.Log4NetHelper.Default().Info(logMsg);
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(new JsonResult()
                        {
                            Status = (int)enumCodeMsg,
                            Message = "接口签名验证失败：" + enumCodeMsg.ToString(),
                            Result = false
                        })),
                    };
                }
            }
            catch (Exception ex)
            {
                string logMsg = $"\r\n移动端调用后端接口，验证签名逻辑异常：\r\n请求URL：{HttpContext.Current.Request.Url.AbsoluteUri}\r\n请求方式：{HttpContext.Current.Request.HttpMethod}";
                XYAuto.CTUtils.Log.Log4NetHelper.Default().Error(logMsg, ex);
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(JsonConvert.SerializeObject(new JsonResult()
                    {
                        Status = -1,
                        Message = "接口签名验证异常：" + ex.Message,
                        Result = false
                    })),
                };
            }
        }
    }
}