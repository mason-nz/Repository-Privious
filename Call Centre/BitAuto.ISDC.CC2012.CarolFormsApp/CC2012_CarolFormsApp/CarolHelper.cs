using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RECONCOMLibrary;
using System.ServiceModel;

namespace CC2012_CarolFormsApp
{
    public class CarolHelper
    {
        public static readonly CarolHelper Instance = new CarolHelper();
        //protected ReconCOM rc = new ReconCOM();

        #region Contructor
        protected CarolHelper()
        {

        }
        #endregion

        //protected void Trace(string s)
        //{
        //    System.Console.WriteLine(s);
        //    rc.R_Trace(3, s);
        //}

        /// <summary>
        /// 恢复电话
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="userName"></param>
        /// <param name="callid"></param>
        /// <returns></returns>
        public bool ReconnectCall(ReconCOM rc, string userName, int callid)
        {
            bool flag = false;
            try
            {
                int iRet = rc.T_ReconnectCall(userName, callid);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 转接电话
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="userName"></param>
        /// <param name="sDestDN"></param>
        /// <returns></returns>
        public bool ConsultCall(ReconCOM rc, string userName, string sDestDN)
        {
            bool flag = false;
            try
            {
                KeyValueList kvl = new KeyValueList();
                rc.T_GetAttachedDataList(Program.CallID, out kvl);
                int iRet = rc.T_ConsultCall(userName, sDestDN, Program.CallID, kvl);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
                Loger.Log4Net.Info("[IVRSatisfaction_Click]ConsultCall iRet=" + iRet);
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 差商状态下转接
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="sDestDN"></param>
        /// <returns></returns>
        public bool TransferCall(ReconCOM rc, string userName)
        {
            bool flag = false;
            try
            {
                //KeyValueList kvl = new KeyValueList();
                //rc.T_GetAttachedDataList(Program.CallID, out kvl);
                //kvl.AddPair("CallRecordID", "001");
                int iRet = rc.T_TransferCall(userName, Program.CallID);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
                Loger.Log4Net.Info("[IVRSatisfaction_Click]TransferCall iRet="+ iRet);
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 来电转移
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="sDestDN"></param>
        /// <returns></returns>
        public bool DivertCall(ReconCOM rc, string sDestDN)
        {
            bool flag = false;
            try
            {
                //KeyValueList kvl = new KeyValueList();
                //rc.T_GetAttachedDataList(Program.CallID, out kvl);
                //kvl.AddPair("CallRecordID", "001");
                int iRet = rc.T_DivertCall(sDestDN, Program.CallID);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 设置随路数据
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="kvl"></param>
        /// <returns></returns>
        public bool AttachDataList(ReconCOM rc, KeyValueList kvl)
        {
            bool flag = false;
            try
            {                
                int iRet = rc.T_AttachDataList(Program.CallID, kvl);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 设置随路数据
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="kvl"></param>
        /// <returns></returns>
        public bool AttachDataPair(ReconCOM rc,int iCallID, string sKey,string sValue)
        {
            bool flag = false;
            try
            {
                int iRet = rc.T_AttachDataPair(iCallID, sKey, sValue);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 挂断电话
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="userName"></param>
        /// <param name="callid"></param>
        /// <returns></returns>
        public bool ReleaseCall(ReconCOM rc, string userName, int callid)
        {
            bool flag = false;
            try
            {
                int iRet = rc.T_ReleaseCall(userName, callid);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 呼出电话
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="userName"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public bool MakeCall(ReconCOM rc, string userName, string mobile)
        {
            bool flag = false;
            try
            {
                int iRet = rc.T_MakeCall(userName, mobile, null);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
                Loger.Log4Net.Info("[CarolHelper]MakeCall ret is:" + iRet);
            }
            catch (Exception)
            {
                flag = false;
            }
            
            return flag;
        }

        /// <summary>
        /// 设置坐席状态（就绪3、置忙4、事后处理5）
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="userName"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool AgentSetState(ReconCOM rc, string userName, int status, int dwAuxState)
        {
            Loger.Log4Net.Info("[CarolHelper]AgentSetState...begin....status is:"+ status + ",auxstate is:"+ dwAuxState);
            bool flag = false;
            try
            {
                int iRet = 0;
                if (status == 4)
                {
                    //iRet = rc.T_AgentSetState(userName, userName, status, dwAuxState);
                    //add by lihf 2014-2-18
                    iRet = rc.T_AgentSetState(userName, LoginUser.AgentNum, status, dwAuxState);
                }
                else
                {
                    //iRet = rc.T_AgentSetState(userName, userName, status, 0);
                    //add by lihf 2014-2-18
                    iRet = rc.T_AgentSetState(userName, LoginUser.AgentNum, status, 0);
                }
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;
                }
                Loger.Log4Net.Info("[CarolHelper]AgentSetState...iRet is:" + iRet);
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }


        public bool AgentLogOff(ReconCOM rc, string userName)
        {
            Loger.Log4Net.Info("[CarolHelper]AgentLogOff...begin....");
            bool flag = false;
            try
            {
                //int iRet = rc.T_AgentLogOff(userName, userName);
                //add by lihf 2014-2-18
                int iRet = rc.T_AgentLogOff(userName, LoginUser.AgentNum);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    flag = true;

                    //退出后，删除状态临时表记录
                    SqlTool tool = new SqlTool();
                    if (!tool.DeleteAgentState2DB())
                    {
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOff...坐席退出时删除状态临时表记录失败....");
                    }
                    else
                    {
                        //Loger.Log4Net.Info("[CarolHelper]AgentLogOff...坐席退出时删除状态临时表记录成功....");
                        //记录坐席退出状态时间
                        DateTime tdate = tool.GetCurrentTime();//DateTime.Now;
                        //测试发现退出状态，在西门子事件里，会处理。所以注释掉
                        //int ival = tool.InsertAgentStateDetail2DB(1, 0, -2, tdate, tdate);
                        //Loger.Log4Net.Info("[CarolHelper]AgentLogOff...坐席退出时ival....IS:" + ival);                        
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOff...坐席退出时LoginOnOid....IS:"+ LoginUser.LoginOnOid);
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOff...坐席退出时tool.GetCurrentTime()....IS:" + tdate.ToString("yyyy-MM-dd HH:mm:ss"));
                        if(LoginUser.LoginOnOid != null)
                            tool.UpdateStateDetail2DB(Convert.ToInt32(LoginUser.LoginOnOid), tdate);
                    }
                    Loger.Log4Net.Info("[CarolHelper]AgentLogOff...退出成功....");
                }
                else
                {
                    Loger.Log4Net.Info("[CarolHelper]AgentLogOff...退出失败....iRet="+ iRet);
                }
            }
            catch (Exception ex)
            {
                flag = false;
                Loger.Log4Net.Info("[CarolHelper]AgentLogOff...errorMessage is:" + ex.Message);
                Loger.Log4Net.Info("[CarolHelper]AgentLogOff...errorSource is:" + ex.Source);
                Loger.Log4Net.Info("[CarolHelper]AgentLogOff...errorStackTrace is:" + ex.StackTrace);
            }
            return flag;
        }

        public bool AgentLogOn(ReconCOM rc, string userName)
        {
            Loger.Log4Net.Info("[CarolHelper]AgentLogOn...begin222....");
            bool flag = false;
            try
            {
                //int iRet = rc.T_AgentLogOn(userName, userName, userName, 0);
                //add by lihf 2014-2-18
                int iRet = rc.T_AgentLogOn(userName, LoginUser.AgentNum, LoginUser.AgentNum, 0);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    Loger.Log4Net.Info("[CarolHelper]AgentLogOn...登录成功....");
                    flag = true;

                    //登录成功后，插入状态到临时表
                    SqlTool tool = new SqlTool();
                    DateTime tdate = DateTime.Now;
                    //登录后，首先进入置忙
                    if (tool.InsertAgentState2DB(4, 0, -2, 0, tdate))
                    {
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOn...置忙状态插入成功....");
                        string msg = "";
                        msg = tool.UpdateLoginOffTime(LoginUser.UserID.ToString(), tdate);
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOn...更新退出时间...."+ msg);
                        LoginUser.LoginOnOid = tool.InsertAgentStateDetail2DB(2, 0, -2, tdate, tdate);                        
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOn...登录状态LoginOnOid....is:" + LoginUser.LoginOnOid);
                    }
                    else
                    {
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOn...登录成功后，插入状态到临时表失败.....");
                    }
                }
                else
                {
                    Loger.Log4Net.Info("[CarolHelper]AgentLogOn...notE_SUCCESS,iRet is:" + iRet);
                }
            }
            catch (Exception)
            {
                flag = false;
            }
            return flag;
        }

        public int AgentLogOn(ReconCOM rc, string userName,string userID)
        {
            Loger.Log4Net.Info("[CarolHelper]AgentLogOn...begin....");
            int iRet = 0;
            try
            {
                //iRet = rc.T_AgentLogOn(userName, userName, userName, 0);
                //add by lihf 2014-2-18
                iRet = rc.T_AgentLogOn(userName, LoginUser.AgentNum, LoginUser.AgentNum, 0);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    Loger.Log4Net.Info("[CarolHelper]AgentLogOn...登录成功....");

                    //登录成功后，插入状态到临时表
                    SqlTool tool = new SqlTool();
                    DateTime tdate = DateTime.Now;
                    //登录后，首先进入置忙
                    if (tool.InsertAgentState2DB(4, 0, -2, 0, tdate))
                    {
                        //tool.InsertAgentStateDetail2DB(2, 0, -2, tdate, tdate);
                        string msg = "";
                        msg = tool.UpdateLoginOffTime(LoginUser.UserID.ToString(), tdate);
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOn...更新退出时间...." + msg);
                        LoginUser.LoginOnOid = tool.InsertAgentStateDetail2DB(2, 0, -2, tdate, tdate);
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOn...登录状态LoginOnOid....is:" + LoginUser.LoginOnOid);
                    }
                    else
                    {
                        Loger.Log4Net.Info("[CarolHelper]AgentLogOn...登录成功后，插入状态到临时表失败.....");
                    }
                }
                else
                {
                    Loger.Log4Net.Info("[CarolHelper]AgentLogOn...notE_SUCCESS,iRet is:" + iRet);
                }
            }
            catch (Exception ex)
            {
                Loger.Log4Net.Info("[CarolHelper]AgentLogOn...登录失败....");
                Loger.Log4Net.Info("[CarolHelper]AgentLogOn...errorMessage...is:" + ex.Message);
                Loger.Log4Net.Info("[CarolHelper]AgentLogOn...errorSource...is:" + ex.Source);
                Loger.Log4Net.Info("[CarolHelper]AgentLogOn...errorStackTrace...is:" + ex.StackTrace);
            }
            return iRet; ;
        }

        public void Clear(ReconCOM rc, string userName)
        {           
            //rc.CS_Login(userName, userName);
            //add by lihf 2014-2-18
            rc.CS_Login(LoginUser.AgentNum, LoginUser.AgentNum);
            AgentLogOff(rc, userName);
            rc.CS_Logout();
        }

        public void AllClear(ReconCOM rc, string userName)
        {
            string msg = string.Empty;
            VCLogServiceHelper.Instance.StopRecord(userName, string.Empty, ref msg);
            VCLogServiceHelper.Instance.AgentLogout(userName, ref msg);
            //rc.T_AgentLogOff(userName, userName);
            int iRet = 0;
            //rc.CS_Login(userName, userName);
            AgentLogOff(rc, userName);
            iRet = rc.CS_Logout();
            Loger.Log4Net.Info("[CarolHelper]AllClear...rc.CS_Logout()...iRet is:"+ iRet);
            rc.R_Close();
        }

        public int Login(ReconCOM rc, string username, bool IsBindRecord)
        {
            Loger.Log4Net.Info("[CarolHelper]Login(ReconCOM rc, string username, bool IsBindRecord) begin...");
            string msg = string.Empty;
            int iRet = -1;
            try
            {
                //iRet = rc.CS_Login(username, username);
                //add by lihf 2014-2-18
                iRet = rc.CS_Login(LoginUser.AgentNum, LoginUser.AgentNum);
                if (iRet == (int)enErrorCode.E_SUCCESS)
                {
                    Loger.Log4Net.Info("[CarolHelper]Login->rc.CS_Login(username, username)...E_SUCCESS...");
                    iRet = rc.T_StartMonitor(username, 1);
                    if (iRet == (int)enErrorCode.E_SUCCESS)
                    {
                        //iRet = rc.T_AgentLogOn(username, username, username, 0);
                        iRet = AgentLogOn(rc, username,"");
                        if (iRet == (int)enErrorCode.E_SUCCESS)
                        {
                            if (IsBindRecord)
                            {
                                if (VCLogServiceHelper.Instance.AgentLogin(username, username, ref msg) &&
                                VCLogServiceHelper.Instance.StartRecord(username, string.Empty, ref msg))
                                {
                                    iRet = (int)enErrorCode.E_SUCCESS;
                                }
                                else
                                {
                                    iRet = -1;//宇高录音登录失败
                                    //rc.T_AgentLogOff(username, username);
                                    //add by lihf 2014-2-18
                                    rc.T_AgentLogOff(username, LoginUser.AgentNum);
                                    rc.CS_Logout();
                                    //rc.R_Close();
                                }
                            }
                        }
                        else
                        {
                            Loger.Log4Net.Info("[CarolHelper]Login->rc.CS_Login-AgentLogOn...失败...iRet is:" + iRet);
                            rc.CS_Logout();
                        }
                    }
                    else
                    {
                        Loger.Log4Net.Info("[CarolHelper]Login->rc.CS_Login-T_StartMonitor(username, 1)...失败...iRet is:"+ iRet);
                        rc.CS_Logout();
                    }
                }
                else
                {
                    Loger.Log4Net.Info("[CarolHelper]Login->rc.CS_Login(username, username)...NOTSUCCESS...iRET IS:" + iRet);
                }
                //System.Console.WriteLine();
                //System.Console.WriteLine((enErrorCode)iRet);
                return iRet;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Exception: ", ex + " - " + ex.Message);
                Loger.Log4Net.Info("[CarolHelper]Login...errorMessage is:" + ex.Message);
                Loger.Log4Net.Info("[CarolHelper]Login...errorSource is:" + ex.Source);
                Loger.Log4Net.Info("[CarolHelper]Login...errorStackTrace is:" + ex.StackTrace);
            }
            return iRet;
        }
    }
}
