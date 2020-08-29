using Senparc.Weixin;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities.Request;
using Senparc.Weixin.MP.MvcExtension;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Configuration;
using XYAuto.BUOC.Chitunion2017.WeChat.CommonService.CustomMessageHandler;
using XYAuto.ITSC.Chitunion2017.BLL;

namespace XYAuto.BUOC.Chitunion2017.WebAPIWeChat
{
    /// <summary>
    /// WeiXin 的摘要说明
    /// </summary>
    public class WeiXin : IHttpHandler
    {

        private static string Token = WebConfigurationManager.AppSettings["WeixinToken"];
        private static string EncodingAESKey = WebConfigurationManager.AppSettings["WeixinEncodingAESKey"];
        private static string AppId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private static string AppSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        readonly Func<string> _getRandomFileName = () => DateTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);
        public void ProcessRequest(HttpContext context)
        {
            string echoString = HttpContext.Current.Request["echoStr"];
            string signature = HttpContext.Current.Request["signature"];
            string timestamp = HttpContext.Current.Request["timestamp"];
            string nonce = HttpContext.Current.Request["nonce"];
            string postString = string.Empty;
            if (HttpContext.Current.Request.HttpMethod.ToUpper() == "POST")
            {
                using (Stream stream = HttpContext.Current.Request.InputStream)
                {
                    Byte[] postBytes = new Byte[stream.Length];
                    stream.Read(postBytes, 0, (Int32)stream.Length);
                    postString = Encoding.UTF8.GetString(postBytes);
                }

                if (!string.IsNullOrEmpty(postString))
                {
                    try
                    {
                        //post method - 当有用户想公众账号发送消息时触发
                        if (!CheckSignature.Check(signature, timestamp, nonce, Token))
                        {
                            WriteContent("参数错误！");
                            return;
                        }

                        //post method - 当有用户想公众账号发送消息时触发
                        var postModel = new PostModel()
                        {
                            Signature = HttpContext.Current.Request["signature"],
                            Msg_Signature = HttpContext.Current.Request["msg_signature"],
                            Timestamp = HttpContext.Current.Request["timestamp"],
                            Nonce = HttpContext.Current.Request["nonce"],
                            Token = Token,
                            EncodingAESKey = EncodingAESKey,
                            AppId = AppId
                        };
                        var maxRecordCount = 10;

                        //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
                        var messageHandler = new CustomMessageHandler(HttpContext.Current.Request.InputStream, postModel, maxRecordCount);

                        #region 设置消息去重

                        /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
                         * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的RequestMessage*/
                        messageHandler.OmitRepeatedMessage = true;//默认已经开启，此处仅作为演示，也可以设置为false在本次请求中停用此功能


                        #endregion

                        try
                        {

                            #region 记录 Request 日志

                            var logPath = System.Web.HttpContext.Current.Server.MapPath(string.Format("~/App_Data/MP/{0}/", DateTime.Now.ToString("yyyy-MM-dd")));

                            if (!Directory.Exists(logPath))
                            {
                                Directory.CreateDirectory(logPath);
                            }

                            //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                            messageHandler.RequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_{1}_{2}.txt", _getRandomFileName(),
                                messageHandler.RequestMessage.FromUserName,
                                messageHandler.RequestMessage.MsgType)));
                            if (messageHandler.UsingEcryptMessage)
                            {
                                messageHandler.EcryptRequestDocument.Save(Path.Combine(logPath, string.Format("{0}_Request_Ecrypt_{1}_{2}.txt", _getRandomFileName(),
                                    messageHandler.RequestMessage.FromUserName,
                                    messageHandler.RequestMessage.MsgType)));
                            }

                            #endregion


                            //执行微信处理过程
                            messageHandler.Execute();

                            #region 记录 Response 日志

                            //测试时可开启，帮助跟踪数据

                            //if (messageHandler.ResponseDocument == null)
                            //{
                            //    throw new Exception(messageHandler.RequestDocument.ToString());
                            //}
                            if (messageHandler.ResponseDocument != null)
                            {
                                messageHandler.ResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_{1}_{2}.txt", _getRandomFileName(),
                                    messageHandler.ResponseMessage.ToUserName,
                                    messageHandler.ResponseMessage.MsgType)));
                            }

                            if (messageHandler.UsingEcryptMessage && messageHandler.FinalResponseDocument != null)
                            {
                                //记录加密后的响应信息
                                messageHandler.FinalResponseDocument.Save(Path.Combine(logPath, string.Format("{0}_Response_Final_{1}_{2}.txt", _getRandomFileName(),
                                    messageHandler.ResponseMessage.ToUserName,
                                    messageHandler.ResponseMessage.MsgType)));
                            }

                            #endregion

                            //WriteContent(messageHandler.ResponseDocument.ToString());//v0.7-
                            //return new WeixinResult(messageHandler);//v0.8+
                            WriteContent(new FixWeixinBugWeixinResult(messageHandler).ToString());//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
                        }
                        catch (Exception ex)
                        {

                            #region 异常处理
                            WeixinTrace.Log("MessageHandler错误：{0}", ex.Message);

                            using (TextWriter tw = new StreamWriter(System.Web.HttpContext.Current.Server.MapPath("~/App_Data/Error_" + _getRandomFileName() + ".txt")))
                            {
                                tw.WriteLine("ExecptionMessage:" + ex.Message);
                                tw.WriteLine(ex.Source);
                                tw.WriteLine(ex.StackTrace);
                                //tw.WriteLine("InnerExecptionMessage:" + ex.InnerException.Message);

                                if (messageHandler.ResponseDocument != null)
                                {
                                    tw.WriteLine(messageHandler.ResponseDocument.ToString());
                                }

                                if (ex.InnerException != null)
                                {
                                    tw.WriteLine("========= InnerException =========");
                                    tw.WriteLine(ex.InnerException.Message);
                                    tw.WriteLine(ex.InnerException.Source);
                                    tw.WriteLine(ex.InnerException.StackTrace);
                                }

                                tw.Flush();
                                tw.Close();
                            }
                            WriteContent(ex.ToString());
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        WriteContent(ex.ToString());
                    }
                }
            }
            else
            {
                Auth(signature, timestamp, nonce, echoString); //微信接入的测试
            }
        }

        private void Auth(string signature,string timestamp,string nonce,string echoString)
        {
            string token = ConfigurationManager.AppSettings["WeixinToken"];//从配置文件获取Token
            if (string.IsNullOrEmpty(token))
            {
                ITSC.Chitunion2017.BLL.Loger.Log4Net.Error(string.Format("WeixinToken 配置项没有配置！"));
            }

            if (CheckSignature.Check(signature, timestamp, nonce, token))
            {
                if (!string.IsNullOrEmpty(echoString))
                {
                    HttpContext.Current.Response.Write(echoString);
                    HttpContext.Current.Response.End();
                }
            }
        }


        private void WriteContent(string str)
        {
            HttpContext.Current.Response.Output.Write(str);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}