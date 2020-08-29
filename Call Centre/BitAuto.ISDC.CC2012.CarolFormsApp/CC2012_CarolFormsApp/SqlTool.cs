using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using BitAuto.Utils;
using System.Data.SqlClient;
using System.Data;

namespace CC2012_CarolFormsApp
{
    public class SqlTool
    {
        string strcn = "";
        AgentTimeState.AgentTimeStateSoapClient agentService = null;
        private string CallRecordAuthorizeCode = System.Configuration.ConfigurationManager.AppSettings["CallRecordAuthorizeCode"];//调用话务记录接口授权码
        public SqlTool()
        {
            strcn = BitAuto.Utils.Config.ConfigurationUtil.GetAppSettingValue("ConnectionStrings_CC");
            agentService = new AgentTimeState.AgentTimeStateSoapClient();
        }

        #region 插入临时记录
        
        public bool InsertAgentState2DB(int state, int agentAuxState,int callType,int agTime,DateTime startTime)
        {
            //string sql = "";
            //sql = "SELECT COUNT(*) FROM CAgent WHERE AgentID='"+ LoginUser.AgentID + "'";

            //System.Data.DataSet ds = BitAuto.Utils.Data.SqlHelper.ExecuteDataset(strcn, System.Data.CommandType.Text, sql);
            //if (Convert.ToInt16(ds.Tables[0].Rows[0][0]) > 0)
            //{
            //    sql = "UPDATE CAgent SET State=" + state + ",agentAuxState=" + agentAuxState + ",CallType=" + callType + ",AGTime=" + agTime + ",StartTime='" + startTime + "'" + ",GroupName='" + LoginUser.GroupName + "'" + ",ExtensionNum='" + LoginUser.ExtensionNum + "'" +
            //          "WHERE AgentID='" + LoginUser.AgentID + "'";
            //}
            //else
            //{
            //    sql = "INSERT CAgent(AgentName,AgentID,State,AgentAuxState,CallType,AGTime,StartTime,GroupName,ExtensionNum) " +
            //           "VALUES('" + LoginUser.TrueName + "','" + LoginUser.AgentID + "'," + state + "," + agentAuxState + "," + callType + "," + agTime + ",'" + startTime + "','" + LoginUser.GroupName + "','" + LoginUser.ExtensionNum + "')";
            //}

            //int ival = -2;
            //ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(strcn, System.Data.CommandType.Text, sql);

            //if (ival < 0)
            //    return false;

            //return true;

            //return agentService.InsertAgentState2DB(state,agentAuxState,callType,agTime,startTime,LoginUser.AgentID.ToString(),LoginUser.TrueName,LoginUser.GroupName,LoginUser.ExtensionNum);
            return agentService.InsertAgentState2DB(CallRecordAuthorizeCode,state, agentAuxState, callType, agTime, startTime, LoginUser.UserID.ToString(), LoginUser.TrueName, LoginUser.BGID, LoginUser.ExtensionNum);
        }

        #endregion

        #region 更新临时记录

        public bool UpdateAgentState2DB(int state, int agentAuxState, int callType, int agTime, DateTime startTime)
        {
            //string sql = "";
            //sql = "UPDATE CAgent SET State=" + state + ",agentAuxState=" + agentAuxState + ",CallType=" + callType + ",AGTime=" + agTime + ",StartTime='" + startTime + "'" +
            //      "WHERE AgentID='"+ LoginUser.AgentID + "'";

            //int ival = -2;
            //ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(strcn, System.Data.CommandType.Text, sql);

            //if (ival < 0)
            //    return false;

            //return true;

            return agentService.UpdateAgentState2DB(CallRecordAuthorizeCode, state, agentAuxState, callType, agTime, startTime, LoginUser.UserID.ToString());
        }

        public bool UpdateAgentState2DB(int agTime)
        {
            string sql = "";
            sql = "UPDATE CAgent SET AGTime=" + agTime +
                  "WHERE AgentID='" + LoginUser.UserID + "'";

            int ival = -2;
            ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(strcn, System.Data.CommandType.Text, sql);

            if (ival < 0)
                return false;

            return true;
        }

        #endregion

        #region 删除临时记录

        public bool DeleteAgentState2DB()
        {
            //string sql = "";
            //sql = "DELETE FROM CAgent "+
            //      "WHERE AgentID='" + LoginUser.AgentID + "'";

            //int ival = -2;
            //ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(strcn, System.Data.CommandType.Text, sql);

            //if (ival < 0)
            //    return false;

            //return true;

            return agentService.DeleteAgentState2DB(CallRecordAuthorizeCode, LoginUser.UserID.ToString());
        }

        #endregion

        #region 插入明细记录

        public int InsertAgentStateDetail2DB(int state, int agentAuxState, int callType, DateTime startTime,DateTime endTime)
        {            
            //SqlParameter[] parameters = {
            //        new SqlParameter("@RecID", SqlDbType.Int,4),
            //        new SqlParameter("@AgentName", SqlDbType.NVarChar,50),
            //        new SqlParameter("@AgentID", SqlDbType.NVarChar,50),
            //        new SqlParameter("@State", SqlDbType.Int,4),
            //        new SqlParameter("@AgentAuxState", SqlDbType.Int,4),
            //        new SqlParameter("@StartTime", SqlDbType.DateTime),
            //        new SqlParameter("@EndTime", SqlDbType.DateTime),
            //        new SqlParameter("@CallType", SqlDbType.Int,4),
            //        new SqlParameter("@ExtensionNum", SqlDbType.NVarChar,10)
            //                            };

            //parameters[0].Direction = ParameterDirection.Output;
            //parameters[1].Value = LoginUser.TrueName;
            //parameters[2].Value = LoginUser.AgentID;
            //parameters[3].Value = state;
            //parameters[4].Value = agentAuxState;
            //parameters[5].Value = startTime;
            //parameters[6].Value = endTime;
            //parameters[7].Value = callType;
            //parameters[8].Value = LoginUser.ExtensionNum;

            //BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(strcn, System.Data.CommandType.StoredProcedure, "p_AgentStateDetail_Insert", parameters);
            //return (int)parameters[0].Value;
            return agentService.InsertAgentStateDetail2DB(CallRecordAuthorizeCode, state, agentAuxState, callType, startTime, endTime, LoginUser.TrueName, LoginUser.UserID.ToString(), LoginUser.ExtensionNum, Convert.ToInt32(LoginUser.BGID));
        }        
        #endregion

        #region 更新状态明细记录

        public bool UpdateStateDetail2DB(int oid,DateTime endTime)
        {
            //string sql = "";
            //sql = "UPDATE AgentStateDetail SET EndTime='" + endTime + "' WHERE Oid=" + oid;

            //int ival = -2;
            //ival = BitAuto.Utils.Data.SqlHelper.ExecuteNonQuery(strcn, System.Data.CommandType.Text, sql);

            //if (ival < 0)
            //    return false;

            //return true;
            return agentService.UpdateStateDetail2DB(CallRecordAuthorizeCode, oid, endTime);
        }

        #endregion

        #region 坐席登录时更新退出时间
        public string UpdateLoginOffTime(string agentID, DateTime endTime)
        {
            return agentService.UpdateLoginOffTime(CallRecordAuthorizeCode, agentID, endTime);
        }
        #endregion

        /// <summary>
        /// 获取负责分组名字字符串
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserGroupNamesStr(int userId)
        {
            //string str = string.Empty;
            //SqlParameter parameter = new SqlParameter("@UserID", userId);
            //object obj = BitAuto.Utils.Data.SqlHelper.ExecuteScalar(strcn, CommandType.StoredProcedure, "p_UserGroupDataRigth_GetUserRightNameStr", parameter);
            //if (obj != null)
            //{
            //    str = obj.ToString().Substring(0, obj.ToString().Length - 1);
            //}

            //return str;
            return agentService.GetUserGroupNamesStr(CallRecordAuthorizeCode, userId);
        }

        /// <summary>
        /// 根据用户ID获取所属分组及外呼出局号
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetBGNameAndOutNum(int userId)
        {
            return agentService.GetBGNameAndOutNum(CallRecordAuthorizeCode, userId);
        }

        #region 根据域名获取用户ID
        public int GetUserIDByNameDomainAccount(string domainAccount)
        {
            return agentService.GetUserIDByNameDomainAccount(CallRecordAuthorizeCode, domainAccount);
        }
        #endregion

        public DateTime GetCurrentTime()
        {
            return agentService.GetCurrentTime(CallRecordAuthorizeCode);
        }
    }
}
