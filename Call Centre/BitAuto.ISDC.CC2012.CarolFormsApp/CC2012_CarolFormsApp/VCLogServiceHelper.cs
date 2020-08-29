using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using CC2012_CarolFormsApp.VClogService;

namespace CC2012_CarolFormsApp
{
    public class VCLogServiceHelper
    {
        public static readonly VCLogServiceHelper Instance = new VCLogServiceHelper();
        private string VCLogServiceURL = System.Configuration.ConfigurationSettings.AppSettings["VCLogServiceURL"].ToString();

        #region Contructor
        protected VCLogServiceHelper()
        {

        }
        #endregion


        /// <summary>
        /// 更新指定流水号录音的数据
        /// </summary>
        /// <param name="RefID">录音流水号</param>
        /// <param name="Data">Data  string  json格式字符串，如：{“CustomerID”:”10012345”,”AgentID”:”53201” }</param>
        /// <returns>0 ：更新成功 1001-1020：参照VCLogAgent.ocx 返回值 2001：数据库连接失败 2002：对应流水号数据不存在 2003：数据格式错误 2004：分机号不存在</returns>
        public int UpdateRecordDataByID(string RefID, string Data)
        {
            int k = -1;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                k = client.UpdateRecordDataByID(RefID, Data);
                client.Close();
                return k;
            }
            catch (Exception ex)
            {
                //msg = ex.Message;
                k = -1;
            }
            return k;
        }

        /// <summary>
        /// 更新当前分机录音数据
        /// </summary>
        /// <param name="Extension">分机</param>
        /// <param name="Data">Data  string  json格式字符串，如：{“CustomerID”:”10012345”,”AgentID”:”53201” }</param>
        /// <returns>0 ：更新成功 1001-1020：参照VCLogAgent.ocx 返回值 2001：数据库连接失败 2002：对应流水号数据不存在 2003：数据格式错误 2004：分机号不存在</returns>
        public int UpdateRecordDataByExt(string Extension, string Data)
        {
            int k = -1;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                k = client.UpdateRecordDataExt(Extension, Data);
                client.Close();
                return k;
            }
            catch (Exception ex)
            {
                //msg = ex.Message;
                k = -1;
            }
            return k;
        }


        /// <summary>
        /// 根据分机号，获取到对应的录音器编号和通道编号 
        /// </summary>
        /// <param name="Extension"></param>
        /// <returns>有对应关系，返回5位字符，前两位为VoiceID，后3位为通道编号，如： 01021，表示录音服务器01上的21号录音通道</returns>
        public string GetChannelByExt(string Extension)
        {
            string s = string.Empty;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                s = client.GetChannelByExt(Extension);
                client.Close();
                return s;
            }
            catch (Exception ex)
            {
                //msg = ex.Message;
                return s;
            }
            return s;
        }


        /// <summary>
        /// 坐席登录 
        /// </summary>
        /// <param name="Extension"></param>
        /// <param name="AgentID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AgentLogin(string Extension, string AgentID, ref string msg)
        {
            Loger.Log4Net.Info("[Login]btnLogin_Click AgentLogin begin...");
            bool flag = false;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                int k = client.AgentLogin(Extension, AgentID);
                switch (k)
                {
                    case 1: flag = true;
                        break;
                    case -1: msg = "分机不存在";
                        break;
                    case 0: msg = "失败";
                        break;
                    default: msg = "未知错误";
                        break;
                }
                client.Close();
                Loger.Log4Net.Info("[Login]btnLogin_Click AgentLogin retval is:" + k);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                Loger.Log4Net.Info("[Login]btnLogin_Click AgentLogin errorStackTrace is:" + ex.StackTrace);
                Loger.Log4Net.Info("[Login]btnLogin_Click AgentLogin errorMessage is:" + ex.Message);
            }
            return flag;
        }


        /// <summary>
        /// 坐席登出
        /// </summary>
        /// <param name="Extension"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool AgentLogout(string Extension, ref string msg)
        {
            bool flag = false;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                int k = client.Agentlogout(Extension);
                switch (k)
                {
                    case 1: flag = true;
                        break;
                    case -1: msg = "分机不存在";
                        break;
                    case 0: msg = "失败";
                        break;
                    default: msg = "未知错误";
                        break;
                }
                client.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 开始录音
        /// </summary>
        /// <param name="Extension"></param>
        /// <param name="Data">{"C1":"10012345","C2":"53201","C3":"Jason"}</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool StartRecord(string Extension, string Data, ref string msg)
        {
            Loger.Log4Net.Info("[Login]btnLogin_Click StartRecord begin...");
            bool flag = false;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                int k = client.StartRecord(Extension, Data);
                switch (k)
                {
                    case 1: flag = true;
                        break;
                    case -1: msg = "分机不存在";
                        break;
                    case 0: msg = "失败";
                        break;
                    default: msg = "未知错误";
                        break;
                }
                client.Close();
                Loger.Log4Net.Info("[Login]btnLogin_Click StartRecord retval is:" + k);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                Loger.Log4Net.Info("[Login]btnLogin_Click StartRecord errorStackTrace is:" + ex.StackTrace);
                Loger.Log4Net.Info("[Login]btnLogin_Click StartRecord errorMessage is:" + ex.Message);
            }
            
            return flag;
        }


        /// <summary>
        /// 停止录音
        /// </summary>
        /// <param name="Extension"></param>
        /// <param name="Data">{"C1":"10012345","C2":"53201","C3":"Jason"}</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool StopRecord(string Extension, string Data, ref string msg)
        {
            bool flag = false;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                int k = client.StopRecord(Extension, Data);
                switch (k)
                {
                    case 1: flag = true;
                        break;
                    case -1: msg = "分机不存在";
                        break;
                    case 0: msg = "失败";
                        break;
                    default: msg = "未知错误";
                        break;
                }
                client.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 暂停录音
        /// </summary>
        /// <param name="Extension"></param>
        /// <param name="Data">{"C1":"10012345","C2":"53201","C3":"Jason"}</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PauseRecord(string Extension, string Data, ref string msg)
        {
            bool flag = false;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                int k = client.PauseRecord(Extension, Data);
                switch (k)
                {
                    case 1: flag = true;
                        break;
                    case -1: msg = "分机不存在";
                        break;
                    case 0: msg = "失败";
                        break;
                    default: msg = "未知错误";
                        break;
                }
                client.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 恢复录音
        /// </summary>
        /// <param name="Extension"></param>
        /// <param name="Data">{"C1":"10012345","C2":"53201","C3":"Jason"}</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool ResumeRecord(string Extension, string Data, ref string msg)
        {
            bool flag = false;
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                int k = client.ResumeRecord(Extension, Data);
                switch (k)
                {
                    case 1: flag = true;
                        break;
                    case -1: msg = "分机不存在";
                        break;
                    case 0: msg = "失败";
                        break;
                    default: msg = "未知错误";
                        break;
                }
                client.Close();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return flag;
        }

        /// <summary>
        /// 获取指定分机录音流水号
        /// </summary>
        /// <param name="Extension"></param>
        /// <returns></returns>
        public string GetRefID(string Extension)
        {
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                string s = client.GetRefID(Extension);
                client.Close();
                return s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 获取指定流水号录音文件的文件路径 
        /// </summary>
        /// <param name="Reordreference">分机录音流水号</param>
        /// <returns>返回格式为：http://127.0.0.1:2122/MDAvMDQxL2UwMDA0MTIwMTIxMjIxMTY1ODE3.wav </returns>
        public string GetFileHttpPath(string Reordreference)
        {
            try
            {
                BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                VCLogServiceClient client = new VCLogServiceClient(binding, new EndpointAddress(new Uri(VCLogServiceURL, false)));
                string s = client.GetFileHttpPath(Reordreference);
                client.Close();
                return s;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
