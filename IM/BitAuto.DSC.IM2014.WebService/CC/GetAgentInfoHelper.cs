using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace BitAuto.DSC.IM2014.WebService.CC
{
    public class GetAgentInfoHelper
    {
        private string AgentInfoServiceURL = System.Configuration.ConfigurationManager.AppSettings["AgentInfoServiceURL"].ToString();//服务URL
        private string AgentInfoAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["AgentInfoAuthorizeCode"].ToString();//验证码

        #region Instance
        public static readonly GetAgentInfoHelper Instance = new GetAgentInfoHelper();
        #endregion

        /// <summary>
        /// 查询客服系统全部相关人员信息，包含（员工编号、邮箱、角色名称），不分页
        /// </summary>
        /// <param name="Verifycode"></param>
        /// <param name="RecordCount"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public DataTable GetAgentInfo(int UserID, string ADName, ref int RecordCount, ref string msg)
        {
            RecordCount = 0;
            object[] args = new object[] { AgentInfoAuthorizeCode, UserID, ADName, RecordCount, msg };
            DataTable dt = (DataTable)WebServiceHelper.InvokeWebService(AgentInfoServiceURL, "GetAgentInfo", args);
            RecordCount = int.Parse(args[3].ToString().Trim());
            msg = args[4].ToString().Trim();
            return dt;
        }
    }
}
