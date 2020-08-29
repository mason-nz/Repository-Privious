/********************************************************
*创建人：hant
*创建时间：2017/12/18 16:31:00 
*说明：
*版权所有：Copyright  2017 流量变现平台事业部-北京行圆汽车信息技术有限公司
*********************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Request;
using XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo.Dto.Response;
using XYAuto.BUOC.ChiTuData2017.Infrastruction;

namespace XYAuto.BUOC.ChiTuData2017.BLL.TaskInfo
{
    public class TaskInfo
    {
        #region
        public static readonly TaskInfo Instance = new TaskInfo();
        #endregion


        /// <summary>
        /// 获取授权码
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetAppKey(RequestBase req, ref string code, ref string msg)
        {
            string appkey = string.Empty;
            StringBuilder json = new StringBuilder();

            try
            {
                #region 验证
                //参数验证
                if (Authentication.Instance.ParaValid<RequestBase>(req, ref code, ref msg))
                {
                    //用户验证
                    if (Authentication.Instance.Access(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                    {
                        string sign = Authentication.Instance.GetSign<RequestBase>(req);
                        //签名验证
                        if (Authentication.Instance.SignValid(req.sign, sign, ref code, ref msg))
                        {
                            if (BLL.TaskInfo.AppInfo.Instance.UpdateAppKey(Convert.ToInt32(req.appId), req.appkey))
                            {
                                Entities.Task.AppInfo info = BLL.TaskInfo.AppInfo.Instance.GeAppInfo(Convert.ToInt32(req.appId));
                                appkey = info.AppKey;
                                code = "1";
                                msg = "success";
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"GetAppKey :{ex.Message}");
                msg = "系统错误";
                code = "-1";
            }
            json.Append("{");
            json.AppendFormat("\"code\":\"{0}\",\"msg\":\"{1}\",\"appkey\":\"{2}\"", code, msg, appkey);
            json.Append("}");
            return json.ToString();
        }


        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetTaskStatus(RequestGetTaskStatus req, ref string code, ref string msg)
        {
            string status = string.Empty;
            StringBuilder json = new StringBuilder();

            try
            {
                #region 验证
                //参数验证
                if (Authentication.Instance.ParaValid<RequestGetTaskStatus>(req, ref code, ref msg))
                {
                    //用户验证
                    if (Authentication.Instance.Access(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                    {
                        string sign = Authentication.Instance.GetSign<RequestGetTaskStatus>(req);
                        //签名验证
                        if (Authentication.Instance.SignValid(req.sign, sign, ref code, ref msg))
                        {
                            //调用次数
                            if (Authentication.Instance.CallNumber(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                            {
                                Entities.Task.TaskList info = Dal.TaskInfo.TaskList.Instance.GetTask(Convert.ToInt32(req.taskid));
                                status = info.Status.ToString();
                                code = "1";
                                msg = "success";
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"GetTaskStatus :{ex.Message}");
                msg = "系统错误";
                code = "-1";
            }
            json.Append("{");
            json.AppendFormat("\"code\":\"{0}\",\"msg\":\"{1}\",\"status\":\"{2}\"", code, msg, status);
            json.Append("}");
            return json.ToString();
        }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public ResponseGetTaskList GetTaskList(RequestGetTaskList req,ref string code,ref string msg)
        {
            ResponseGetTaskList list = new ResponseGetTaskList();

            try
            {

                #region 验证参数
                if (req.appId == null || req.appkey == null || req.sign == null || req.tasktime == null || req.timestamp == null || req.version == null )
                {
                    msg = "参数不能为空";
                    code = "-203";
                    list.List = null;
                    return list;
                }
                if (req.version != "1.0")
                {
                    msg = "版本错误";
                    code = "-204";
                    list.List = null;
                    return list;
                }
                #endregion

                #region 用户验证
                
                if (!BLL.TaskInfo.Authentication.Instance.Access(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                {
                    list.List = null;
                    return list;
                }
   
                #endregion

                #region 验证签名
                string sign = Authentication.Instance.GetSign<RequestGetTaskList>(req);
                if (!sign.Equals(req.sign))
                {
                    msg = "签名错误";
                    code = "-102";
                    list.List = null;
                    return list;
                }
                #endregion

                #region 调用次数
                if (!BLL.TaskInfo.Authentication.Instance.CallNumber(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                {
                    list.List = null;
                    return list;
                }
                #endregion

                #region 获取数据
                int totalCount = 0;
                List<Entities.Task.TaskList> listtask = Dal.TaskInfo.TaskList.Instance.GetTaskList(Convert.ToInt32(req.page_index) , Convert.ToInt32(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("InterfacePageSize")),Convert.ToInt32(req.tasktype),Convert.ToDateTime(req.tasktime),out totalCount);

                code = "1";
                msg = "success";
                list.List = listtask;
                #endregion
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"GetTaskList :{ex.Message}");
                msg = "系统错误";
                code = "-1";
                list.List = null;
            }
            return list;
        }


        /// <summary>
        /// 获取推广Url
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public string GetTaskOrderUrl(RequestTaskOrderUrl req, ref string code, ref string msg)
        {
            string orderurl = string.Empty;
            StringBuilder json = new StringBuilder();

            try
            {
                #region 验证
                //参数验证
                if (Authentication.Instance.ParaValid<RequestTaskOrderUrl>(req, ref code, ref msg))
                {
                    //用户验证
                    if (Authentication.Instance.Access(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                    {
                        string sign = Authentication.Instance.GetSign<RequestTaskOrderUrl>(req);
                        //签名验证
                        if (Authentication.Instance.SignValid(req.sign, sign, ref code, ref msg))
                        {
                            //调用次数
                            if (Authentication.Instance.CallNumber(Convert.ToInt32(req.appId), req.appkey, ref msg, ref code))
                            {
                                Entities.Task.AppInfo appinfo = BLL.TaskInfo.AppInfo.Instance.GeAppInfo(Convert.ToInt32(req.appId));
                                //获取数据
                                string postdate = "TaskId=" + req.taskid + "&UserIdentity=" + req.useridentity + "&ChannelId=" + appinfo.ChannelID;
                                string geturl = PostWebRequest(XYAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("GetOrderUrlByPost"),postdate);
                                Entities.Task.OrderUrl url = Newtonsoft.Json.JsonConvert.DeserializeObject<Entities.Task.OrderUrl>(geturl);
                                if (url.Status == 0)
                                {
                                    orderurl = url.Result.OrderUrl;
                                    code = "1";
                                    msg = "success";
                                }
                                else
                                {
                                    msg = url.Message;
                                    code = url.Status.ToString();
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error($"GetTaskOrderUrl :{ex.Message}");
                msg = "系统错误";
                code = "-1";
            }
            json.Append("{");
            json.AppendFormat("\"code\":\"{0}\",\"msg\":\"{1}\",\"orderurl\":\"{2}\"", code, msg, orderurl);
            json.Append("}");
            return json.ToString();
        }

        /// <summary>
        /// Post提交数据
        /// </summary>
        /// <param name="postUrl">URL</param>
        /// <param name="paramData">参数</param>
        /// <returns></returns>
        private string PostWebRequest(string postUrl, string paramData)
        {
            string ret = string.Empty;
            try
            {
                if (!postUrl.StartsWith("http://"))
                    return "";

                byte[] byteArray = Encoding.Default.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }


        public HttpResponseMessage toJson(Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }

    }
}
