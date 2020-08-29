using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BitAuto.ISDC.CC2012.WebService.GetAgentInfo.ServiceReference;


namespace BitAuto.ISDC.CC2012.WebService
{
    public class CallRecordServiceHelper
    {
        //private string CallRecordServiceURL = System.Configuration.ConfigurationManager.AppSettings["CallRecordServiceURL"].ToString();//服务URL
        private string CallRecordAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["CallRecordAuthorizeCode"].ToString();//验证码
        CallRecordServiceProxy proxy = new CallRecordServiceProxy();

        #region Instance
        public static readonly CallRecordServiceHelper Instance = new CallRecordServiceHelper();
        #endregion

        /// <summary>
        ///获取话务数据，最大不能超过1万条
        /// </summary>
        /// <param name="maxID">主键id</param>
        /// <returns>返回CallRecord_ORIG集合</returns>
        public DataTable GetCallRecordByMaxID(int maxID, ref string msg)
        {
            //object Obj = WebServiceHelper.InvokeWebService(CallRecordServiceURL, "GetCallRecordByMaxID", new object[] { CallRecordAuthorizeCode, maxID, msg });
            //return (DataTable)Obj;
            return proxy.GetCallRecordByMaxID(CallRecordAuthorizeCode, maxID, ref msg);
        }
        /// <summary>
        /// 根据CallID，更新业务数据，businessID为业务ID，BGID为分组ID，SCID为分类ID
        /// </summary>
        /// <param name="callID">callID</param>
        /// <param name="businessID">业务ID</param>
        /// <param name="BGID">分组ID</param>
        /// <param name="SCID">分类ID</param>
        /// <param name="msg">返回信息</param>
        /// <returns></returns>
        public int UpdateBusinessDataByCallID(Int64 callID, string businessID, int BGID, int SCID, int createuserid, ref string msg)
        {
            //object[] args = new object[] { CallRecordAuthorizeCode, callID, businessID, BGID, SCID, createuserid, msg };
            //int r = (int)WebServiceHelper.InvokeWebService(CallRecordServiceURL, "UpdateBusinessDataByCallID", args);
            //msg = args[6].ToString().Trim();
            //return r;
            return proxy.UpdateBusinessDataByCallID(CallRecordAuthorizeCode,callID, businessID, BGID, SCID, createuserid,ref msg);
        }


        ///// <summary>
        ///// 根据当前登录者ID或真实姓名，查询坐席信息（坐席姓名、坐席工号、坐席ID、在呼叫中心系统对应角色）
        ///// </summary>
        ///// <param name="LoginUserID">当前登录者ID</param>
        ///// <param name="TrueName">真实姓名</param>
        ///// <param name="PageSize">页大小</param>
        ///// <param name="PageIndex">当前页码</param>
        ///// <param name="RecordCount">记录数</param>
        ///// <param name="msg">返回验证信息</param>
        ///// <returns>返回查询坐席信息</returns>
        //public DataTable GetAgentInfoByBGID(int LoginUserID, string TrueName, int PageSize, int PageIndex, ref int RecordCount, ref string msg)
        //{
        //    RecordCount = 0;
        //    object[] args = new object[] { CallRecordAuthorizeCode, LoginUserID, TrueName, PageSize, PageIndex, RecordCount, msg };
        //    DataTable dt = (DataTable)WebServiceHelper.InvokeWebService(CallRecordServiceURL, "GetAgentInfoByBGID", args);
        //    RecordCount = int.Parse(args[5].ToString().Trim());
        //    msg = args[6].ToString().Trim();
        //    return dt;
        //}

        /// <summary>
        /// 根据域账号，返回呼叫中心系统当前用户角色信息
        /// </summary>
        /// <param name="domainAccount">域账号</param>
        /// <param name="msg">返回验证信息</param>
        /// <returns>返回角色信息</returns>
        public string GetAgentRoleNameByDomainAccount(string domainAccount)
        {
            //object[] args = new object[] { CallRecordAuthorizeCode, domainAccount };
            //string RoleName = (string)WebServiceHelper.InvokeWebService(CallRecordServiceURL, "GetAgentRoleNameByDomainAccount", args);
            ////msg = args[2].ToString().Trim();
            //return RoleName;
            return proxy.GetAgentRoleNameByDomainAccount(CallRecordAuthorizeCode, domainAccount);
        }

        ///// <summary>
        ///// 查询客服系统全部相关人员信息，包含（员工编号、邮箱、角色名称），不分页
        ///// </summary>
        ///// <param name="Verifycode"></param>
        ///// <param name="RecordCount"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public DataTable GetAgentInfo(int UserID, string ADName, ref int RecordCount, ref string msg)
        //{
        //    RecordCount = 0;
        //    object[] args = new object[] { CallRecordAuthorizeCode,UserID,ADName, RecordCount, msg };
        //    DataTable dt = (DataTable)WebServiceHelper.InvokeWebService(CallRecordServiceURL, "GetAgentInfo", args);
        //    RecordCount = int.Parse(args[3].ToString().Trim());
        //    msg = args[4].ToString().Trim();
        //    return dt;
        //}

        //public DataTable GetAgentInfoByCondition(AgentInfoCondition AgentInfoCondition, int PageSize, int PageIndex, ref int RecordCount, ref string msg)
        //{
        //    RecordCount = 0;
        //    object[] args = new object[] { CallRecordAuthorizeCode, AgentInfoCondition, PageSize, PageIndex, RecordCount, msg };
        //    DataTable dt = (DataTable)WebServiceHelper.InvokeWebService(CallRecordServiceURL, "GetAgentInfoByCondition", args);
        //    RecordCount = int.Parse(args[4].ToString().Trim());
        //    msg = args[5].ToString().Trim();
        //    return dt;
        //}

        ///// <summary>
        ///// 先进行话务与业务之间的绑定，然后插入个人用户信息
        ///// </summary>
        ///// <param name="strJson"></param>
        ///// <returns></returns>
        //public object InsertCustData(string jsonDataStr)
        //{
        //    object[] args = new object[] { CallRecordAuthorizeCode, jsonDataStr };
        //    object result = (object)WebServiceHelper.InvokeWebService(CallRecordServiceURL, "InsertCustData", args);
        //    return result;
        //}

        /// <summary>
        /// 先进行话务与业务之间的绑定，然后插入个人用户信息
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public string InsertCustData(string jsonDataStr)
        {
            //object[] args = new object[] { CallRecordAuthorizeCode, jsonDataStr };
            //string result = (string)WebServiceHelper.InvokeWebService(CallRecordServiceURL, "InsertCustDataReturnStr", args);
            //return result;
            return proxy.InsertCustDataReturnStr(CallRecordAuthorizeCode, jsonDataStr);
        }
    }


    class CallRecordServiceProxy : CC.CallRecordService.GetCallRecordList
    {
        public CallRecordServiceProxy()
        {
            string strTimeout = System.Configuration.ConfigurationManager.AppSettings["CallRecordServiceTimeout"];
            int timeout = 0;
            this.Timeout = 1000 * (int.TryParse(strTimeout, out timeout) ? timeout : WebServiceHelper.WebServiceTimeout);

            string url = System.Configuration.ConfigurationManager.AppSettings["CallRecordServiceURL"];
            if (string.IsNullOrEmpty(url) == false)
            {
                this.Url = url;
            }
        }
    }
}
