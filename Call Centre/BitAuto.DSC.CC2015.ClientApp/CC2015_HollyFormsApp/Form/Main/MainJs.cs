using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.ComponentModel;
using System.Threading;
using System.Configuration;
using BitAuto.ISDC.CC2012.Entities;
using CC2015_HollyFormsApp.Code.Utils;
using CC2015_HollyFormsApp.Code.Logic;
using Microsoft.JScript;

namespace CC2015_HollyFormsApp
{
    /// 和web直接的通讯接口实现
    /// <summary>
    /// 和web直接的通讯接口实现
    /// </summary>
    public partial class Main : Form
    {
        public DateTime? TimeStart, TimeEnd;
        /// iframe容器的名称
        /// <summary>
        /// iframe容器的名称
        /// </summary>
        public string SendMsgToIframeSubPageName = System.Configuration.ConfigurationManager.AppSettings["SendMsgToIframeSubPageName"];
        /// 存在ifarme容器的页面id集合
        /// <summary>
        /// 存在ifarme容器的页面id集合
        /// </summary>
        private List<string> alowReceiveCallMsg_WebpageIDList = new List<string>();

        /// web端调用方法入口
        /// <summary>
        /// web端调用方法入口
        /// </summary>
        /// <param name="urlPath"></param>
        /// <returns></returns>
        public string MethodScript(string urlPath)
        {
            if (urlPath == "colsewindow")
            {
                if (LoginHelper.Instance.ExitLogin())
                {
                    System.Environment.Exit(0);
                }
            }
            Loger.Log4Net.Info("[MainJs] [MethodScript] urlPath " + urlPath);
            urlPath = new WebCommandManage(urlPath).GetWebCommand();
            if (urlPath == null)
            {
                Loger.Log4Net.Info("[MainJs] [MethodScript] urlPath 命令无效");
                return "";
            }
            string msg = string.Empty;
            try
            {
                System.Uri url = new Uri("http://localhost" + urlPath);
                string[] absPath = url.AbsolutePath.Split('/');
                if (absPath.Length > 0)
                {
                    switch (absPath[1].ToLower())
                    {
                        case "callcontrol":
                            msg = CallControl(url, absPath);
                            break;
                        case "agent":
                            msg = GetAgentInfo(absPath);
                            break;
                        case "browsercontrol":
                            msg = BrowserControl(url, absPath);
                            break;
                        case "time":
                            msg = TimeProgress(url, absPath);
                            break;
                        case "pagecontrol":
                            msg = PageControl(url, absPath);
                            break;
                        default:
                            msg = "";
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("[MainJs] [MethodScript] 异常", ex);
                msg = ex.Message;
                MessageBox.Show("调用方法出错：" + ex.Message);
            }
            Loger.Log4Net.Info("[MainJs] [MethodScript] 命令调用完成，返回结果=" + msg);
            return msg;
        }

        /// 向业务系统发送消息-入口
        /// <summary>
        /// 向业务系统发送消息-入口
        /// </summary>
        /// <param name="msg"></param>
        public void SendToWeb(string msg, int recount = 0)
        {
            SendToWeb(msg, CurrentOutBoundTabPageName, recount);
        }
        /// 向业务系统发送消息-入口
        /// <summary>
        /// 向业务系统发送消息-入口
        /// </summary>
        /// <param name="msg"></param>
        public void SendToWeb(string msg, string webpageid, int recount = 0)
        {
            Loger.Log4Net.Info("[MainJs] [SendToWeb] 发送网页消息：" + recount + " 次数，指定页面ID：" + webpageid);
            try
            {
                IHTMLWindow2 win = GetIHTMLWindow2ByWebPageID(webpageid);
                string para = string.Format("&outBoundType={0}", ((int)BusinessProcess.OutBoundType).ToString());
                msg += para;

                //呼入振铃，则弹屏
                if (msg.StartsWith("UserEvent=" + UserEvent.Transferred.ToString()))
                {
                    TimeProgress(null, new string[] { "", "", "start" });

                    Loger.Log4Net.Info("[MainJs] [SendMesToWeb] 弹屏 " + ConfigurationManager.AppSettings["DefaultURL"] + "/CTI/PopTransfer.aspx");
                    NewPageForReq(ConfigurationManager.AppSettings["DefaultURL"] + "/CTI/PopTransfer.aspx?" + msg, true);
                }
                //发送事件
                win.execScript("try{MethodScript('" + msg + "');}catch(e){}", "Javascript");
                var subwin = CrossFrameIE.GetFrameWindowObject(win, SendMsgToIframeSubPageName);
                //给当前Page中iframe子页面发送消息，若子页面没有订阅消息，则不会发送
                if (subwin != null && alowReceiveCallMsg_WebpageIDList.Contains(webpageid))
                {
                    subwin.execScript("try{MethodScript('" + msg + "');}catch(e){}", "Javascript");
                }
                Loger.Log4Net.Info("[MainJs] [SendMesToWeb] CTIEventMsg msg：" + msg);

                if (msg.StartsWith("UserEvent=Released"))
                {
                    CurrentOutBoundTabPageName = "";
                    Loger.Log4Net.Info("[MainJs] [CurrentOutBoundTabPageName] 挂断清空");
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Error("客户端回调函数出错：" + ex.Message + "|" + ex.StackTrace);

                //2秒后重新执行,尝试5次
                if (recount <= 5)
                {
                    Thread thread = new Thread(new ThreadStart(() =>
                    {
                        Thread.Sleep(2 * 1000);
                        this.Invoke(new Action<string, int>(SendToWeb), msg, recount + 1);
                    }));
                    thread.Start();
                }
            }
        }

        #region 辅助
        /// 电话控制
        /// <summary>
        /// 电话控制
        /// </summary>
        /// <param name="url"></param>
        /// <param name="absPath"></param>
        /// <returns></returns>
        private string CallControl(System.Uri url, string[] absPath)
        {
            switch (absPath[2].ToLower())
            {
                case "makecall":
                    if (!LoginUser.isLoggedIn)
                    {
                        MessageBox.Show("客服已签出，当前状态不可呼出！");
                        return "";
                    }
                    if (!string.IsNullOrEmpty(CurrentOutBoundTabPageName))
                    {
                        if ((int)Main_PhoneStatus <= (int)PhoneStatus.PS06_话后)
                        {
                            //合力状态和客户端状态不一致
                            MessageBox.Show("正在通话中，当前状态不可呼出（如果没有在通话中，需要重新打开该页面）！");
                        }
                        else
                        {
                            MessageBox.Show("正在通话中，当前状态不可呼出！");
                        }
                        return "";
                    }
                    else
                    {
                        string[] para = url.Query.Substring(1).Split('&');
                        TabPage cmdTabPage = null;
                        string webpageid = GetWebPageIDFormParas("外呼", para, out cmdTabPage);
                        if (para[0].ToLower().StartsWith("targetdn"))
                        {
                            //默认外显号码标记
                            string tag = "9";
                            if (!string.IsNullOrEmpty(LoginUser.OutNum))
                            {
                                tag = LoginUser.OutNum;
                            }
                            BusinessProcess.OutBoundType = OutBoundTypeEnum.OT1_页面呼出;
                            //组装电话号码和出局号码，然后外呼
                            MakeCallForPhoneAndOut(para[0].Split('=')[1].Trim(), tag, webpageid);
                        }
                        else
                        {
                            MessageBox.Show("呼叫参数错误！应为格式：/CallControl/MakeCall?TargetDN=XXX&OutShowTag=X");
                        }
                    }
                    break;
                case "releasecall":
                    //挂机
                    HollyContactHelper.Instance.ActHangup();
                    break;
                case "getcallid"://获取当前话务ID（挂断后不清空）
                    return CurrentCallInfo.CallID > 0 ? CurrentCallInfo.CallID.ToString() : "";
                case "getcallidforhangupempty"://获取当前话务ID（挂断后自动清空）
                    return CurrentCallInfo.CallIDForHangupEmpty > 0 ? CurrentCallInfo.CallIDForHangupEmpty.ToString() : "";
                case "getcallnum"://获取当前呼入电话的主叫号码（挂断后不清空）
                    return (string.IsNullOrEmpty(CurrentCallInfo.CallNum) ? "" : CurrentCallInfo.CallNum);
                case "getcallnumforhangupempty"://获取当前呼入电话的主叫号码（挂断后自动清空）
                    return (string.IsNullOrEmpty(CurrentCallInfo.CallNumForHangupEmpty) ? "" : CurrentCallInfo.CallNumForHangupEmpty);
                case "startreceivecallmsg":
                    //开始接受话务回调事件消息（针对每个页面）
                    return ReceiveCallMsg(url, true).ToString();
                case "stopreceivecallmsg":
                    //停止接受话务回调事件消息（针对每个页面）
                    return ReceiveCallMsg(url, false).ToString();
                default: break;
            }
            return "";
        }

        /// <summary>
        /// 根据当前请求的命令，来进行订阅推送客户端话务消息，给iframe子页面
        /// </summary>
        /// <param name="url">当前请求的命令</param>
        /// <param name="isStart">开始推送为True，取消推送为False</param>
        /// <returns>调用命令结果（true成功，false是吧）</returns>
        private bool ReceiveCallMsg(Uri url, bool isStart)
        {
            bool flag = false;
            TabPage cmdTabPage = null;
            string webpageid = GetWebPageIDFormParas(isStart ? "StartReceiveCallMsg" : "EndReceiveCallMsg",
                url != null && !string.IsNullOrEmpty(url.Query) ? url.Query.Substring(1).Split('&') : null, out cmdTabPage);

            if (isStart)
            {
                if (!alowReceiveCallMsg_WebpageIDList.Contains(webpageid))
                {
                    alowReceiveCallMsg_WebpageIDList.Add(webpageid);
                }
                flag = true;
            }
            else
            {
                alowReceiveCallMsg_WebpageIDList.Remove(webpageid);
                flag = true;
            }
            Loger.Log4Net.Info("[MainJs] [MethodScript] 执行命令，在PageID为：" + webpageid + "页签中，" + (isStart ? "开始" : "停止") + "接受话务回调事件消息，结果为：" + flag);
            return flag;
        }
        /// 根据外显号码标记，呼出电话
        /// <summary>
        /// 根据外显号码标记，呼出电话
        /// </summary>
        /// <param name="mobile">电话号码</param>
        /// <param name="outshowTag">外显号码标记</param>
        public bool MakeCallForPhoneAndOut(string mobile, string outshowTag, string webpageid)
        {
            string outNumber = ""; string errorMsg = "";
            if (LoginUser.LoginAreaType == AreaType.西安)
            {
                //本地区域属于西安
                VerifyPhoneFormatHelper.Instance.VerifyFormatXiAn(mobile, out outNumber, out errorMsg);
            }
            else if (LoginUser.LoginAreaType == AreaType.北京)
            {
                //本地区域属于北京
                VerifyPhoneFormatHelper.Instance.VerifyFormat(mobile, out outNumber, out errorMsg);
            }
            if (!string.IsNullOrEmpty(errorMsg))
            {
                MessageBox.Show(errorMsg);
                return false;
            }
            else
            {
                //外呼的具体实现
                MakeCall(outshowTag + outNumber, HollyContactHelper.ConDeviceType.外线号码, new System.Action(() =>
                {
                    //外呼成功之后
                    //设置当前呼出窗口名称
                    CurrentOutBoundTabPageName = webpageid;
                    Loger.Log4Net.Info("[MainJs] [CurrentOutBoundTabPageName] 开始--外呼通话网页名称：" + CurrentOutBoundTabPageName);
                }));
            }
            return true;
        }
        /// 获取客服信息
        /// <summary>
        /// 获取客服信息
        /// </summary>
        /// <param name="absPath"></param>
        /// <returns></returns>
        private static string GetAgentInfo(string[] absPath)
        {
            string msg = "";
            switch (absPath[2].ToLower())
            {
                //获取分机号码
                case "username":
                    msg = LoginUser.ExtensionNum;
                    break;
                //获取userid
                case "userid":
                    msg = LoginUser.UserID == null ? "" : LoginUser.UserID.Value.ToString();
                    break;
                default: break;
            }
            return msg;
        }
        /// 浏览器控制
        /// <summary>
        /// 浏览器控制
        /// </summary>
        /// <param name="url"></param>
        /// <param name="absPath"></param>
        /// <returns></returns>
        private string BrowserControl(System.Uri url, string[] absPath)
        {
            string msg = "";
            string webpageid = "";
            TabPage cmdTabPage = null;//当前操作的TabPage

            switch (absPath[2].ToLower())
            {
                //新建窗口，并导航到参数url地址
                case "newpage":
                    string[] para = url.Query.Substring(1).Split('=');
                    if (para[0].ToLower().StartsWith("url"))
                    {
                        string ss = url.Query.Substring(1).Substring(4).Trim();
                        NewPageForReq(System.Web.HttpUtility.UrlDecode(ss), false);
                        msg = "ok";
                    }
                    else
                    {
                        MessageBox.Show("新建窗口，必须有url这一参数");
                        msg = "failed";
                    }
                    break;
                //呼入时，新建窗口，并导航到参数url地址$
                case "newpageinbound":
                    string[] para2 = url.Query.Substring(1).Split('&');
                    if (para2[0].ToLower().StartsWith("url"))
                    {
                        if (para2[0].IndexOf("=") >= 0)
                        {
                            string ss = para2[0].Substring(para2[0].IndexOf("=") + 1).Trim();
                            NewPageForReq(System.Web.HttpUtility.UrlDecode(ss), true);
                            msg = "ok";
                        }
                        else
                        {
                            msg = "failed";
                        }
                    }
                    else
                    {
                        MessageBox.Show("新建窗口，必须有url这一参数");
                        msg = "failed";
                    }
                    break;
                //关闭当前窗口
                case "closepage":
                    string[] para3 = GetQueryParams(url);
                    webpageid = GetWebPageIDFormParas("关闭当前窗口", para3, out cmdTabPage);
                    WebClosePageEvent(webpageid);
                    msg = "ok";
                    break;
                //关闭当前窗口，并刷新父窗口
                case "closepagereloadppage":
                    string[] para4 = GetQueryParams(url);
                    webpageid = GetWebPageIDFormParas("关闭当前窗口，并刷新父窗口", para4, out cmdTabPage);
                    string ptabPageName = (cmdTabPage.Tag == null ? string.Empty : (string)cmdTabPage.Tag);
                    WebClosePageEvent(webpageid);

                    if (ptabPageName != string.Empty)
                    {
                        foreach (TabPage tp in tabControl1.TabPages)
                        {
                            if ((!string.IsNullOrEmpty(ptabPageName)) && ((string)tp.Name).Equals(ptabPageName))
                            {
                                ((WebBrowser)tp.Controls[0]).Refresh(WebBrowserRefreshOption.Completely);
                                tabControl1.SelectedTab = tp;
                                break;
                            }
                        }
                    }
                    msg = "ok";
                    break;
                //关闭当前窗口，并调用父窗口中指定标签中的click方法
                case "closepagecallparentpagefun":
                    string[] para5 = GetQueryParams(url);
                    webpageid = GetWebPageIDFormParas("关闭当前窗口，并自动点击父窗口查询按钮", para5, out cmdTabPage);
                    if (para5[0].ToLower().StartsWith("parentpagecontrolid"))
                    {
                        string inputID = para5[0].Substring(para5[0].IndexOf("=") + 1).Trim();
                        string ptabPageName2 = (cmdTabPage.Tag == null ? string.Empty : (string)cmdTabPage.Tag);
                        WebClosePageEvent(webpageid);
                        if (ptabPageName2 != string.Empty)
                        {
                            foreach (TabPage tp in tabControl1.TabPages)
                            {
                                if ((!string.IsNullOrEmpty(ptabPageName2)) && ((string)tp.Name).Equals(ptabPageName2))
                                {
                                    tabControl1.SelectedTab = tp;
                                    IHTMLWindow2 win;
                                    win = (IHTMLWindow2)this.GetCurrentBrowser().Document.Window.DomWindow;
                                    win.execScript("try{document.getElementById('" + inputID + "').click();}catch(e){}", "Javascript");
                                    break;
                                }
                            }
                        }
                        msg = "ok";
                    }
                    else
                    {
                        MessageBox.Show("父窗口中指定标签ID，必须有这一参数");
                        msg = "failed";
                    }
                    break;
                default: break;
            }
            return msg;
        }
        private string[] GetQueryParams(System.Uri url)
        {
            if (url.Query == null || url.Query.Length <= 1)
            {
                return new string[] { };
            }
            else
            {
                return url.Query.Substring(1).Split('&');
            }
        }
        /// 时间处理
        /// <summary>
        /// 时间处理
        /// </summary>
        /// <param name="url"></param>
        /// <param name="absPath"></param>
        /// <returns></returns>
        private string TimeProgress(Uri url, string[] absPath)
        {
            string msg = "";
            string[] para = null;
            switch (absPath[2].ToLower())
            {
                case "start":
                    TimeStart = Common.GetCurrentTime();
                    Loger.Log4Net.Info("[=====计时日志=====] 弹屏开始 " + TimeStart);
                    break;
                case "end":
                    para = url.Query.Substring(1).Split('=');
                    if (para[0].ToLower().StartsWith("msg") && TimeStart != null && para.Length >= 2)
                    {
                        TimeEnd = Common.GetCurrentTime();
                        string ss = System.Web.HttpUtility.UrlDecode(para[1]);
                        Loger.Log4Net.Info("[=====计时日志=====] 弹屏结束 " + TimeEnd + " 总耗时（" + ((TimeEnd.Value - TimeStart.Value).TotalSeconds.ToString("0.00")) + "s）" + " 其中dll加载：" + ss);
                        TimeStart = null;
                    }
                    break;
                case "time":
                    para = url.Query.Substring(1).Split('=');
                    if (para[0].ToLower().StartsWith("msg") && para.Length >= 2)
                    {
                        string ss = System.Web.HttpUtility.UrlDecode(para[1]);
                        Loger.Log4Net.Info("[=====计时日志=====] " + ss);
                    }
                    break;
            }
            return msg;
        }
        /// 从命令行中获取webpageid
        /// <summary>
        /// 从命令行中获取webpageid
        /// </summary>
        /// <param name="paras"></param>
        /// <param name="defaultWebpageid"></param>
        /// <returns></returns>
        private string GetWebPageIDFormParas(string name, string[] paras, out TabPage tp)
        {
            //默认当前页面
            string defaultWebpageid = ((TabPage)GetCurrentBrowser().Parent).Name;
            tp = GetTabPageByWebPageID(defaultWebpageid);

            if (paras == null || paras.Length == 0)
            {
                return defaultWebpageid;
            }
            string webpageid = paras.FirstOrDefault(x => x.StartsWith("WebPageID="));
            if (webpageid == null)
            {
                return defaultWebpageid;
            }
            webpageid = webpageid.Replace("WebPageID=", "");
            if (string.IsNullOrEmpty(webpageid))
            {
                return defaultWebpageid;
            }
            TabPage newtp = GetTabPageByWebPageID(webpageid);
            if (newtp == null)
            {
                Loger.Log4Net.Info("[MainJs] [GetWebPageIDFormParas] " + name + " 命令中获取不到页面ID使用默认值：" + defaultWebpageid + " 父页面ID：" + CommonFunction.ObjectToString(tp.Tag));
                return defaultWebpageid;
            }
            else
            {
                tp = newtp;
                Loger.Log4Net.Info("[MainJs] [GetWebPageIDFormParas] " + name + " 命令中获取到了页面ID：" + webpageid + " 父页面ID：" + CommonFunction.ObjectToString(tp.Tag));
                return webpageid;
            }
        }
        /// <summary>
        /// 页面控制
        /// </summary>
        /// <param name="url"></param>
        /// <param name="absPath"></param>
        /// <returns></returns>
        private string PageControl(Uri url, string[] absPath)
        {
            string msg = "";
            string webpageid = "";
            TabPage cmdTabPage = null;//当前操作的TabPage

            switch (absPath[2].ToLower())
            {
                //转发页面消息（目前只支持同一个页面内互转）
                case "sendmsg":
                    string[] para5 = GetQueryParams(url);
                    if (para5[0].ToLower().StartsWith("content"))
                    {
                        //接收到网页的消息(客户端调用CC客户端接口之前需要encodeURIComponent；服务器端返回到客户端之前，需要escape；客户端接收到消息后，需要decodeURIComponent)
                        string content = para5[0].Substring(para5[0].IndexOf("=") + 1).Trim();
                        content = GlobalObject.decodeURIComponent(content);

                        Loger.Log4Net.Info("[MainJs] [MethodScript] 接收到网页的消息=" + content);
                        //转发网页的消息会当前页面
                        NoticeData ud = new NoticeData("SendMsgToWindows", Common.EscapeString(content));
                        webpageid = GetWebPageIDFormParas("转发页面消息", para5, out cmdTabPage);
                        Loger.Log4Net.Info("[MainJs] [MethodScript] 转发页面消息到 " + webpageid + " content=" + Common.EscapeString(content));
                        SendToWeb(ud.ToString(), webpageid);
                        msg = "ok";
                    }
                    else
                    {
                        MessageBox.Show("参数content必须指定");
                        msg = "failed";
                    }
                    break;
                default: break;
            }
            return msg;
        }
        #endregion
    }
}
