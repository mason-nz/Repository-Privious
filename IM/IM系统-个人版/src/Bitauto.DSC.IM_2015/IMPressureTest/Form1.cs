using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

[assembly: log4net.Config.DOMConfigurator(ConfigFile = "App", ConfigFileExtension = "config", Watch = true)]

namespace IMPressureTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LogHelper.SetConfig();
            //LogHelper.WriteLog("美丽i归属3333333");
        }

        private int nParaWY = 0;              //并发网友数量
        private int nParaAgent = 0;              //并发网友数量
        private int LongConnInterVel = 5;   //长连接时间间隔
        private int WYSendMsgInterVel = 3;  //网友发消息时间间隔

        private int TotalAgentNum = 0;
        private int TotalWYNum = 0;

        private int nIdentity = Convert.ToInt32(DateTime.Now.ToString("mmss")) * 1000;

        public bool AgentLongMonitor
        {
            get { return this.ckAgentLong.Checked; }
        }
        public bool WYSendMsg
        {
            get { return this.ckSendMsg.Checked; }
        }

        public int txtSendInterVel
        {
            get { return Convert.ToInt32(this.txtSendMsgInterval.Text.Trim()); }
        }


        public bool WYLongMonitor
        {
            get { return this.ckWYLong.Checked; }
        }

        private void InitParas()
        {
            if (!int.TryParse(txtWYNum.Text.Trim(), out nParaWY))
            {
                nParaWY = 2000;
            }
            if (!int.TryParse(txtLongConnetInterval.Text.Trim(), out LongConnInterVel))
            {
                LongConnInterVel = 5;
            }
            if (!int.TryParse(txtSendMsgInterval.Text.Trim(), out WYSendMsgInterVel))
            {
                WYSendMsgInterVel = 3;
            }
            if (!int.TryParse(txtAgentNum.Text.Trim(), out nParaAgent))
            {
                nParaAgent = 10;
            }

        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            InitParas();
            Thread thd = new Thread(() =>
            {
                Parallel.For(0, nParaWY, (i) =>
                {
                    var WYID = Interlocked.Increment(ref TotalWYNum).ToString();
                    AddNotes("添加网友:" + WYID);
                    new CommonModel(this, "网友 " + WYID).InitWY(0);
                });
            });
            thd.Start();
        }
        public void AddNotes(string msg)
        {
            //return;
            if (this.listNotes.InvokeRequired)
            {
                Action<string> d = new Action<string>(AddNotes);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                this.listNotes.Items.Add(msg);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.listNotes.Items.Clear();
        }

        private void btnInitAgent_Click(object sender, EventArgs e)
        {
            InitParas();
            Thread thd = new Thread(() =>
            {
                Parallel.For(0, nParaAgent, (i) =>
                {
                    var AgentID = (Interlocked.Increment(ref TotalAgentNum) + nIdentity).ToString();

                    AddNotes("添加坐席:" + AgentID);
                    new CommonModel(this, "坐席 " + AgentID)
                    {
                        AgentID = AgentID,
                        strPrivateToken = AgentID,
                        IsAgent = 1,
                        LongConnInterVel = this.LongConnInterVel,
                        WYSendMsgInterVel = this.WYSendMsgInterVel

                    }.InitAgent();
                });
            });
            thd.Start();


        }

        private void BtnStartTest_Click(object sender, EventArgs e)
        {

        }
    }



    public class CommonModel
    {
        private Form1 form;
        private string CSID = string.Empty; //会话ID
        public string AgentID = string.Empty;  //坐席ID
        private string StrName;
        private string strInit;
        public string strPrivateToken = string.Empty;
        private int lastMessageId;
        private int nTimerCount = 0;
        public System.Threading.Timer tm;
        public int IsAgent = 0;
        public int LongConnInterVel = 5;   //长连接时间间隔
        public int WYSendMsgInterVel = 8;  //网友发消息时间间隔

        public DateTime dtLongConn;
        public DateTime dtSendMsg;

        public CommonModel(Form1 form, string name)
        {

            this.StrName = name;
            this.form = form;
            tm = new System.Threading.Timer(TimerEvent, nTimerCount, 1000, Timeout.Infinite);
        }

        private void TimerEvent(object state)
        {

            //if (!int.TryParse(this.form.txtSendInterVel, out WYSendMsgInterVel))
            //{
            //    LongConnInterVel = 5;
            //}

            this.nTimerCount++;

            if (this.nTimerCount % WYSendMsgInterVel == 0)
            {
                if (IsAgent == 0)
                {
                    SendMsg();
                }
                //发送随机数
            }
            //if (this.nTimerCount % LongConnInterVel == 0)
            //{
            //    SendLongConnet();
            //}
            this.tm.Change(1000, Timeout.Infinite);
        }

        public void InitAgent()
        {
            strInit = "http://im.sys1.bitauto.com/AjaxServers/Handler.ashx";

            IDictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("action", "init");
            paras.Add("FromPrivateToken", this.AgentID);
            paras.Add("usertype", "1");

            //var ss= HttpHelper.GetResponseString(HttpHelper.CreatePostHttpResponse(strInit, paras, null));
            // HttpHelper.GetResponseString(HttpHelper.CreateGetHttpResponse(strInit,10000, null, null));
            Encoding encoding = Encoding.GetEncoding("gb2312");
            try
            {
                var sResult =
                    HttpWebResponseUtility.GetResponseString(HttpWebResponseUtility.CreatePostHttpResponse(strInit,
                        paras, 10000, "", encoding, null));
                this.form.AddNotes("坐席：" + this.StrName + " Init结束:  " + sResult + DateTime.Now.ToString("O"));

                SetAgentStatus();
                SendLongConnet();
            }
            catch
            {

            }
        }

        public void SetAgentStatus()
        {

            strInit = "http://im.sys1.bitauto.com/AjaxServers/Handler.ashx";

            IDictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("action", "setagentstate");
            paras.Add("FromPrivateToken", this.AgentID);
            paras.Add("AgentState", "1");

            try
            {

                Encoding encoding = Encoding.GetEncoding("gb2312");
                var sResult = HttpWebResponseUtility.GetResponseString(HttpWebResponseUtility.CreatePostHttpResponse(strInit, paras, 10000, "", encoding, null));
                this.form.AddNotes("坐席：" + this.StrName + " 更改坐席状态结束:  " + sResult + DateTime.Now.ToString("O"));
            }
            catch (Exception)
            {

                //throw;
            }

        }
        public void InitWY(int isAgent)
        {
            if (isAgent == 1)
            {
                strInit = "";
            }
            else
            {
                strInit = "http://im.sys1.bitauto.com/AjaxServers/Handler.ashx";
                //"?action=init&EPTitle=%E7%99%BE%E5%BA%A6%E9%A6%96%E9%A1%B5&FromPrivateToken=&UserReferURL=http%3A%2F%2Flocalhost%3A57690%2FWebForm1.aspx&" +
                //"LoginID=&SourceType=2&EPPostURL=http%3A%2F%2Fattend.oa.bitauto.com%2Fadmin%2FAttendLogTable.aspx&usertype=2&CityID=201&ProvinceID=2";
            }

            //Dictionary<string, string> paras = new Dictionary<string, string>();
            IDictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("action", "init");
            paras.Add("EPTitle", "%E7%99%BE%E5%BA%A6%E9%A6%96%E9%A1%B5");
            paras.Add("FromPrivateToken", "");
            paras.Add("UserReferURL", "http%3A%2F%2Flocalhost%3A57690%2FWebForm1.aspx");
            paras.Add("LoginID", "");
            paras.Add("SourceType", "2");
            paras.Add("EPPostURL", "");
            paras.Add("usertype", "2");
            paras.Add("CityID", "201");
            paras.Add("ProvinceID", "2");
            //var ss= HttpHelper.GetResponseString(HttpHelper.CreatePostHttpResponse(strInit, paras, null));
            // HttpHelper.GetResponseString(HttpHelper.CreateGetHttpResponse(strInit,10000, null, null));
            Encoding encoding = Encoding.GetEncoding("gb2312");
            try
            {
                var sResult = HttpWebResponseUtility.GetResponseString(HttpWebResponseUtility.CreatePostHttpResponse(strInit, paras, 10000, "", encoding, null));
                this.form.AddNotes("网友：" + this.StrName + " Init结束:  " + sResult + DateTime.Now.ToString("O"));

                if (sResult.IndexOf("loginid") > 0)
                {
                    strPrivateToken = sResult.Substring(31, 36);
                    cominquene();
                    SendLongConnet();
                }
            }
            catch (Exception)
            {


            }

        }

        public void SendLongConnet()
        {
            IDictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("privateToken", strPrivateToken);
            paras.Add("lastMessageId", lastMessageId.ToString());
            Encoding encoding = Encoding.GetEncoding("gb2312");

            //var resp = HttpWebResponseUtility.CreatePostHttpResponse("http://im.sys1.bitauto.com/DefaultChannel.ashx",
            //    paras, 100000, "", encoding, null);

            if (IsAgent == 1)
            {
                HttpWebResponseUtility.CreatePostHttpResponseAsync("http://im.sys1.bitauto.com/DefaultChannel.ashx",
                    paras, 100000, "", encoding, null, new AsyncCallback(GetAgentRequestStreamCallback));       //坐席    
            }
            else
            {
                HttpWebResponseUtility.CreatePostHttpResponseAsync("http://im.sys1.bitauto.com/DefaultChannel.ashx",
                   paras, 100000, "", encoding, null, new AsyncCallback(GetWYRequestStreamCallback));           //网友
            }

        }

        private void GetWYRequestStreamCallback(IAsyncResult asyncResult)
        {
            string sResult = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;

                string strLog = string.Format("{0}       开始发长连接:{1}", this.StrName, request.Date.ToString("O"));
                sResult = HttpWebResponseUtility.GetResponseString(request.EndGetResponse(asyncResult) as HttpWebResponse);

                strLog += (string.Format(" 长连接结束，用时{0}，消息内容:{1}", (DateTime.Now - request.Date).TotalSeconds, sResult));

                //if (sResult.IndexOf("aspNetComet.timeout") > -1)
                //{

                //}



                LogHelper.WriteLog(strLog);

                //Debug.WriteLine(sResult);
                if (this.form.WYLongMonitor)
                {
                    this.form.AddNotes(strLog);
                }
                if (sResult.IndexOf("mid") > 0)
                {
                    var mid = sResult.IndexOf("mid");
                    var s = sResult.IndexOf(",", mid);
                    var lastmid = sResult.Substring(mid + 5, s - mid - 5);
                    if (int.TryParse(lastmid, out mid))
                    {
                        Interlocked.Exchange(ref lastMessageId, mid);
                    }
                }
                InitCSIDStuff(sResult);
            }
            catch
            {

            }

            if (sResult.IndexOf("aspNetComet.error") == -1)
            {
                SendLongConnet();
            }
        }

        private void GetAgentRequestStreamCallback(IAsyncResult asyncResult)
        {
            string sResult = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                string strLog = string.Format("{0}       开始发长连接:{1}", this.StrName, request.Date.ToString("O"));

                sResult = HttpWebResponseUtility.GetResponseString(request.EndGetResponse(asyncResult) as HttpWebResponse);

                strLog += (string.Format(" 长连接结束，用时{0}，消息内容:{1}", (DateTime.Now - request.Date).TotalSeconds, sResult));

                //Debug.WriteLine(sResult);
                if (this.form.AgentLongMonitor)
                {
                    this.form.AddNotes(strLog);
                }
                if (sResult.IndexOf("mid") > 0)
                {
                    var mid = sResult.IndexOf("mid");
                    var s = sResult.IndexOf(",", mid);
                    var lastmid = sResult.Substring(mid + 5, s - mid - 5);
                    if (int.TryParse(lastmid, out mid))
                    {
                        Interlocked.Exchange(ref lastMessageId, mid);
                    }
                }
                InitCSIDStuff(sResult);
            }
            catch (Exception ex)
            {
                this.form.AddNotes(ex.Message);
            }
            if (sResult.IndexOf("aspNetComet.error") == -1)
            {
                SendLongConnet();
            }
        }

        private void InitCSIDStuff(string strResult)
        {
            if (strResult.IndexOf("MAllocAgent") > 0)
            {
                int nS = strResult.IndexOf("\"a\"");
                int nE = strResult.IndexOf(",", nS);
                AgentID = strResult.Substring(nS + 4, nE - nS - 4);

                nS = strResult.LastIndexOf("\"cs\"");
                nE = strResult.IndexOf(",", nS);
                CSID = strResult.Substring(nS + 5, nE - nS - 5);
            }
        }

        public void SendMsg()
        {
            IDictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("action", "sendmessage");
            paras.Add("FromPrivateToken", strPrivateToken);
            paras.Add("usertype", "2");
            string strMessage = System.Web.HttpUtility.UrlEncode(this.StrName + "=>  你好  " + nTimerCount.ToString() + DateTime.Now.ToString("O"));

            paras.Add("message", strMessage);
            paras.Add("SendToPrivateToken", AgentID);
            paras.Add("AllocID", CSID);
            Encoding encoding = Encoding.GetEncoding("gb2312");
            DateTime dtNow = DateTime.Now;
            //string strLog = string.Format("{0}       开始发消息:{1}", this.StrName, dtNow.ToString("O"));

            //var sResult = HttpWebResponseUtility.CreatePostHttpResponseAsync();
            HttpWebResponseUtility.CreatePostHttpResponseAsync("http://im.sys1.bitauto.com/AjaxServers/Handler.ashx", paras, 100000, "", encoding, null, new AsyncCallback(SendMsgCallBack));


            //strLog += (string.Format("发消息结束，用时{0}，消息内容:{1}", (DateTime.Now - dtNow).TotalSeconds, sResult));
            //LogHelper.WriteLog(strLog);

            //Debug.WriteLine(sResult);
            //this.form.AddNotes("网友：" + this.StrName + " 发消息   :  " + sResult + DateTime.Now.ToString("O"));

        }

        private void SendMsgCallBack(IAsyncResult asyncResult)
        {
            string strLog = string.Empty;
            try
            {
                HttpWebRequest request = (HttpWebRequest)asyncResult.AsyncState;
                strLog = this.StrName + "       开始发消息给: " + this.AgentID +"     "+ request.Date.ToString("O");

                string sResult = HttpWebResponseUtility.GetResponseString(request.EndGetResponse(asyncResult) as HttpWebResponse);

                strLog += (string.Format("          消息结束，用时{0}，消息内容:{1}", (DateTime.Now - request.Date).TotalSeconds, sResult));

            }
            catch (Exception ex)
            {
                strLog += string.Format("{0}    发送消息失败，内容:{2}", this.StrName, ex.Message);
            }
            if (this.form.WYSendMsg)
            {
                this.form.AddNotes(strLog);
            }

            LogHelper.WriteLog(strLog);

        }

        public void cominquene()
        {
            IDictionary<string, string> paras = new Dictionary<string, string>();
            paras.Add("FromPrivateToken", strPrivateToken);
            paras.Add("SourceType", "2");
            paras.Add("action", "cominquene");
            Encoding encoding = Encoding.GetEncoding("gb2312");
            try
            {
                var sResult = HttpWebResponseUtility.GetResponseString(HttpWebResponseUtility.CreatePostHttpResponse("http://im.sys1.bitauto.com/AjaxServers/Handler.ashx", paras, 10000, "", encoding, null));
                Debug.WriteLine(sResult);
                this.form.AddNotes("网友：" + this.StrName + " 排队结束    :  " + sResult + DateTime.Now.ToString("O"));
            }
            catch (Exception)
            {


            }


        }
    }



}
