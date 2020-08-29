using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BitAuto.Utils;
using BitAuto.Utils.Data;
using BitAuto.ISDC.CC2012.Entities;
using BitAuto.ISDC.CC2012.Entities.Constants;
using BitAuto.ISDC.CC2012.Entities.CallReport;
using System.Linq;
using System.Diagnostics;

namespace BitAuto.ISDC.CC2012.Dal
{
    //----------------------------------------------------------------------------------
    /// <summary>
    /// 数据访问类CallRecord_ORIG。
    /// </summary>
    /// <author>
    /// masj
    /// </author>
    /// <history>
    /// 2013-04-16 04:11:44 Created
    /// </history>
    /// <version>
    /// 1.0
    /// </version>
    //----------------------------------------------------------------------------------
    public class CallRecord_ORIG : DataBase
    {
        TelNumManage manage = null;
        TelNumManage Manage
        {
            get
            {
                if (manage == null)
                {
                    manage = Dal.CallDisplay.Instance.CreateTelNumManage();
                }
                return manage;
            }
        }

        public string Conn
        {
            get
            {
                return CONNECTIONSTRINGS;
            }
        }

        public static readonly CallRecord_ORIG Instance = new CallRecord_ORIG();

        private const string P_CALLRECORD_ORIG_SELECT = "p_CallRecord_ORIG_Select";
        private const string P_CALLRECORD_ORIG_INSERT = "p_CallRecord_ORIG_Insert";
        private const string P_CALLRECORD_ORIG_UPDATE = "p_CallRecord_ORIG_Update";

        protected CallRecord_ORIG()
        { }

        private Entities.CallRecord_ORIG LoadSingleCallRecord_ORIG(DataRow row)
        {
            Entities.CallRecord_ORIG model = new Entities.CallRecord_ORIG();

            if (row["RecID"].ToString() != "")
            {
                model.RecID = long.Parse(row["RecID"].ToString());
            }
            model.SessionID = row["SessionID"].ToString();
            if (row["CallID"].ToString() != "")
            {
                model.CallID = Int64.Parse(row["CallID"].ToString());
            }
            model.ExtensionNum = row["ExtensionNum"].ToString();
            model.PhoneNum = row["PhoneNum"].ToString();
            model.ANI = row["ANI"].ToString();
            if (row["CallStatus"].ToString() != "")
            {
                model.CallStatus = int.Parse(row["CallStatus"].ToString());
            }
            model.SwitchINNum = row["SwitchINNum"].ToString();
            if (row["OutBoundType"].ToString() != "")
            {
                model.OutBoundType = int.Parse(row["OutBoundType"].ToString());
            }
            model.SkillGroup = row["SkillGroup"].ToString();
            if (row["InitiatedTime"].ToString() != "")
            {
                model.InitiatedTime = DateTime.Parse(row["InitiatedTime"].ToString());
            }
            if (row["RingingTime"].ToString() != "")
            {
                model.RingingTime = DateTime.Parse(row["RingingTime"].ToString());
            }
            if (row["EstablishedTime"].ToString() != "")
            {
                model.EstablishedTime = DateTime.Parse(row["EstablishedTime"].ToString());
            }
            if (row["AgentReleaseTime"].ToString() != "")
            {
                model.AgentReleaseTime = DateTime.Parse(row["AgentReleaseTime"].ToString());
            }
            if (row["CustomerReleaseTime"].ToString() != "")
            {
                model.CustomerReleaseTime = DateTime.Parse(row["CustomerReleaseTime"].ToString());
            }
            if (row["AfterWorkBeginTime"].ToString() != "")
            {
                model.AfterWorkBeginTime = DateTime.Parse(row["AfterWorkBeginTime"].ToString());
            }
            if (row["AfterWorkTime"].ToString() != "")
            {
                model.AfterWorkTime = int.Parse(row["AfterWorkTime"].ToString());
            }
            if (row["ConsultTime"].ToString() != "")
            {
                model.ConsultTime = DateTime.Parse(row["ConsultTime"].ToString());
            }
            if (row["ReconnectCall"].ToString() != "")
            {
                model.ReconnectCall = DateTime.Parse(row["ReconnectCall"].ToString());
            }
            if (row["TallTime"].ToString() != "")
            {
                model.TallTime = int.Parse(row["TallTime"].ToString());
            }
            model.AudioURL = row["AudioURL"].ToString();
            if (row["CreateTime"].ToString() != "")
            {
                model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
            }
            if (row["CreateUserID"].ToString() != "")
            {
                model.CreateUserID = int.Parse(row["CreateUserID"].ToString());
            }
            if (row["CallStatus"].ToString() != "")
            {
                model.CallStatus = int.Parse(row["CallStatus"].ToString());
            }
            if (row["TransferInTime"].ToString() != "")
            {
                model.TransferInTime = DateTime.Parse(row["TransferInTime"].ToString());
            }
            if (row["TransferOutTime"].ToString() != "")
            {
                model.TransferOutTime = DateTime.Parse(row["TransferOutTime"].ToString());
            }
            return model;
        }

        /// <summary>
        ///  增加一条数据
        /// </summary>
        public int Insert(Entities.CallRecord_ORIG model)
        {
            //入库顺序：CallRecord_ORIG_Business（触发入库 CallRecord_ORIG_Task），如果接通 CallRecordInfo，挂断入库 CallRecord_ORIG
            //更新CallRecord_ORIG_Task
            string sql = @"UPDATE CallRecord_ORIG_Task
                                    SET CallStatus=@CallStatus,OutBoundType=@OutBoundType,EstablishedTime=@EstablishedTime,CreateUserID=@CreateUserID
                                    WHERE CallID=@CallID";
            SqlParameter[] parameters0 = {
                    new SqlParameter("@CallID", SqlDbType.BigInt,8){Value=model.CallID},
                    new SqlParameter("@CallStatus", SqlDbType.Int,4){Value=model.CallStatus},          
                    new SqlParameter("@OutBoundType", SqlDbType.Int,4){Value=model.OutBoundType},
                    new SqlParameter("@EstablishedTime", SqlDbType.DateTime){Value=model.EstablishedTime},
                    new SqlParameter("@CreateUserID", SqlDbType.Int){Value=model.CreateUserID}
                                        };
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql, parameters0);

            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@SessionID", SqlDbType.VarChar,50),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@ExtensionNum", SqlDbType.VarChar,20),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,50),
					new SqlParameter("@ANI", SqlDbType.VarChar,50),
					new SqlParameter("@CallStatus", SqlDbType.Int,4),
					new SqlParameter("@SwitchINNum", SqlDbType.VarChar,100),
					new SqlParameter("@OutBoundType", SqlDbType.Int,4),
					new SqlParameter("@SkillGroup", SqlDbType.VarChar,200),
					new SqlParameter("@InitiatedTime", SqlDbType.DateTime),
					new SqlParameter("@RingingTime", SqlDbType.DateTime),
					new SqlParameter("@EstablishedTime", SqlDbType.DateTime),
					new SqlParameter("@AgentReleaseTime", SqlDbType.DateTime),
					new SqlParameter("@CustomerReleaseTime", SqlDbType.DateTime),
					new SqlParameter("@AfterWorkBeginTime", SqlDbType.DateTime),
					new SqlParameter("@AfterWorkTime", SqlDbType.Int,4),
					new SqlParameter("@ConsultTime", SqlDbType.DateTime),
					new SqlParameter("@ReconnectCall", SqlDbType.DateTime),
					new SqlParameter("@TallTime", SqlDbType.Int,4),
					new SqlParameter("@AudioURL", SqlDbType.VarChar,800),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
					new SqlParameter("@CreateUserID", SqlDbType.Int),
                    new SqlParameter("@SiemensCallID", SqlDbType.Int,4),
                    new SqlParameter("@GenesysCallID", SqlDbType.NVarChar,50),
                    new SqlParameter("@TransferInTime", SqlDbType.DateTime),
                    new SqlParameter("@TransferOutTime", SqlDbType.DateTime)};

            parameters[0].Direction = ParameterDirection.Output;
            parameters[1].Value = model.SessionID;
            parameters[2].Value = model.CallID;
            parameters[3].Value = model.ExtensionNum;
            parameters[4].Value = model.PhoneNum;
            parameters[5].Value = model.ANI;
            parameters[6].Value = model.CallStatus;
            parameters[7].Value = model.SwitchINNum;
            parameters[8].Value = model.OutBoundType;
            parameters[9].Value = model.SkillGroup;
            parameters[10].Value = model.InitiatedTime;
            parameters[11].Value = model.RingingTime;
            parameters[12].Value = model.EstablishedTime;
            parameters[13].Value = model.AgentReleaseTime;
            parameters[14].Value = model.CustomerReleaseTime;
            parameters[15].Value = model.AfterWorkBeginTime;
            parameters[16].Value = model.AfterWorkTime;
            parameters[17].Value = model.ConsultTime;
            parameters[18].Value = model.ReconnectCall;
            parameters[19].Value = model.TallTime;
            parameters[20].Value = model.AudioURL;
            parameters[21].Value = model.CreateTime;
            parameters[22].Value = model.CreateUserID;
            parameters[23].Value = model.SiemensCallID;
            parameters[24].Value = model.GenesysCallID;
            parameters[25].Value = model.TransferInTime;
            parameters[26].Value = model.TransferOutTime;

            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_INSERT, parameters);
            return int.Parse(parameters[0].Value.ToString());
        }
        /// <summary>
        ///  更新一条数据
        /// </summary>
        public int Update(Entities.CallRecord_ORIG model)
        {
            SqlParameter[] parameters = {
					new SqlParameter("@RecID", SqlDbType.BigInt,8),
					new SqlParameter("@SessionID", SqlDbType.VarChar,50),
					new SqlParameter("@CallID", SqlDbType.BigInt,8),
					new SqlParameter("@ExtensionNum", SqlDbType.VarChar,20),
					new SqlParameter("@PhoneNum", SqlDbType.VarChar,50),
					new SqlParameter("@ANI", SqlDbType.VarChar,50),
					new SqlParameter("@CallStatus", SqlDbType.Int,4),
					new SqlParameter("@SwitchINNum", SqlDbType.VarChar,100),
					new SqlParameter("@OutBoundType", SqlDbType.Int,4),
					new SqlParameter("@SkillGroup", SqlDbType.VarChar,200),
					new SqlParameter("@InitiatedTime", SqlDbType.DateTime),
					new SqlParameter("@RingingTime", SqlDbType.DateTime),
					new SqlParameter("@EstablishedTime", SqlDbType.DateTime),
					new SqlParameter("@AgentReleaseTime", SqlDbType.DateTime),
					new SqlParameter("@CustomerReleaseTime", SqlDbType.DateTime),
					new SqlParameter("@AfterWorkBeginTime", SqlDbType.DateTime),
					new SqlParameter("@AfterWorkTime", SqlDbType.Int,4),
					new SqlParameter("@ConsultTime", SqlDbType.DateTime),
					new SqlParameter("@ReconnectCall", SqlDbType.DateTime),
					new SqlParameter("@TallTime", SqlDbType.Int,4),
					new SqlParameter("@AudioURL", SqlDbType.VarChar,800),
					new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@TransferInTime", SqlDbType.DateTime),
                    new SqlParameter("@TransferOutTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RecID;
            parameters[1].Value = model.SessionID;
            parameters[2].Value = model.CallID;
            parameters[3].Value = model.ExtensionNum;
            parameters[4].Value = model.PhoneNum;
            parameters[5].Value = model.ANI;
            parameters[6].Value = model.CallStatus;
            parameters[7].Value = model.SwitchINNum;
            parameters[8].Value = model.OutBoundType;
            parameters[9].Value = model.SkillGroup;
            parameters[10].Value = model.InitiatedTime;
            parameters[11].Value = model.RingingTime;
            parameters[12].Value = model.EstablishedTime;
            parameters[13].Value = model.AgentReleaseTime;
            parameters[14].Value = model.CustomerReleaseTime;
            parameters[15].Value = model.AfterWorkBeginTime;
            parameters[16].Value = model.AfterWorkTime;
            parameters[17].Value = model.ConsultTime;
            parameters[18].Value = model.ReconnectCall;
            parameters[19].Value = model.TallTime;
            parameters[20].Value = model.AudioURL;
            parameters[21].Value = model.CreateTime;
            parameters[22].Value = model.TransferInTime;
            parameters[23].Value = model.TransferOutTime;

            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.StoredProcedure, P_CALLRECORD_ORIG_UPDATE, parameters);
        }

        #region 分月查询
        /// 话务总表数据查询
        /// <summary>
        /// 话务总表数据查询
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallRecord_ORIGByList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, string tableEndName, out int totalCount)
        {
            string where = string.Empty;

            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("cob", "c", "BGID", "CreateUserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and ea.AgentNum='" + SqlFilter(query.AgentNum) + "'";
            }

            if (query.TaskID != Constant.STRING_INVALID_VALUE)
            {
                where += " and cob.BusinessID='" + SqlFilter(query.TaskID) + "'";
            }

            if (query.RecID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.RecID=" + query.RecID;
            }
            if (query.CallID != Constant.INT_INVALID_VALUE)
            {
                where += " and c.CallID=" + query.CallID;
            }
            if (query.ANI != Constant.STRING_INVALID_VALUE && query.ANI != "")
            {
                where += " AND c.ANI = '" + SqlFilter(query.ANI) + "'";
            }
            if (query.PhoneNum != Constant.STRING_INVALID_VALUE && query.PhoneNum != "")
            {
                where += " AND c.PhoneNum = '" + SqlFilter(query.PhoneNum) + "'";
            }
            //话务类型
            if (query.CallTypes != Constant.STRING_INVALID_VALUE && query.CallTypes != "")
            {
                where += " and c.CallStatus in (" + Dal.Util.SqlFilterByInCondition(query.CallTypes) + ")";
            }
            //挂断类型
            if (query.CallRelease != Constant.STRING_INVALID_VALUE && query.CallRelease != "")
            {
                if (query.CallRelease.Contains("1") && !query.CallRelease.Contains("2"))//客户
                {
                    where += " and c.CustomerReleaseTime is not null and c.CustomerReleaseTime <> '1900-01-01 0:0:0'";
                }
                else if (!query.CallRelease.Contains("1") && query.CallRelease.Contains("2"))//坐席
                {
                    where += " and c.CustomerReleaseTime is null and c.CustomerReleaseTime <> '1900-01-01 0:0:0'";
                }
            }
            //通话时长
            if (query.SpanTime1 != Constant.INT_INVALID_VALUE)
            {
                where += " and c.TallTime>=" + query.SpanTime1;
            }
            if (query.SpanTime2 != Constant.INT_INVALID_VALUE)
            {
                where += " and c.TallTime<=" + query.SpanTime2;
            }
            //话务总时长
            if (query.TallTime1 != Constant.INT_INVALID_VALUE)
            {
                where += " and ( CONVERT(BIGINT, c.TallTime) + CONVERT(BIGINT, c.AfterWorkTime) )>=" + query.TallTime1;
            }
            if (query.TallTime2 != Constant.INT_INVALID_VALUE)
            {
                where += " and ( CONVERT(BIGINT, c.TallTime) + CONVERT(BIGINT, c.AfterWorkTime) )<=" + query.TallTime2;
            }
            //通话日期
            if (query.BeginTime != Constant.STRING_INVALID_VALUE && query.BeginTime != "")
            {
                where += " and c.CreateTime>='" + SqlFilter(query.BeginTime) + "'";
            }
            if (query.EndTime != Constant.STRING_INVALID_VALUE && query.EndTime != "")
            {
                where += " and c.CreateTime<='" + SqlFilter(query.EndTime) + "'";
            }
            if (query.BusinessGroup != Constant.INT_INVALID_VALUE)
            {
                where += " and cob.BGID=" + query.BusinessGroup + " and c.TallTime>0";
            }
            if (!string.IsNullOrEmpty(query.OutTypes))
            {
                where += " and c.OutBoundType in (" + Dal.Util.SqlFilterByInCondition(query.OutTypes) + ")";
            }

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Value = tableEndName;
            parameters[5].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallRecord_ORIG_SelectByList", parameters);
            totalCount = (int)(parameters[5].Value);
            return ds.Tables[0];
        }
        /// 取满意度统计列表
        /// <summary>
        /// 取满意度统计列表
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlaceID">1是西安，2是北京</param>
        /// <param name="DateType">1是日，2是周，3是月</param>
        /// <returns></returns>
        public DataTable GetSatisfactionList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, out int totalCount, int PlaceID, DateTime b, DateTime e, int DateType, string tableEndName)
        {
            string where = string.Empty;
            totalCount = 0;
            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("s", "y", "BGID", "CreateUserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and e.agentnum='" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }

            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and y.CreateUserID=" + query.CreateUserID;
            }

            if (query.SwitchINNum != Constant.STRING_INVALID_VALUE)
            {
                where += " and y.SwitchINNum='" + StringHelper.SqlFilter(query.SwitchINNum) + "'";
            }
            if (query.SkillGroup != Constant.STRING_INVALID_VALUE)
            {
                where += " and y.SkillGroup='" + StringHelper.SqlFilter(query.SkillGroup) + "'";
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE && query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and y.createTime between '" + StringHelper.SqlFilter(query.BeginTime) + "' and '" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59.997'";
            }


            DataSet ds = null; ;
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@b", SqlDbType.DateTime),
                    new SqlParameter("@e", SqlDbType.DateTime),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20)
					};

            parameters[0].Value = where;
            parameters[1].Value = " maxd desc";
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = b;
            parameters[6].Value = e;
            parameters[7].Value = tableEndName;
            //西安
            if (PlaceID == 1)
            {
                if (DateType == 1)
                {
                    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_XA_Day", parameters);
                    totalCount = (int)(parameters[4].Value);
                }
                else if (DateType == 2)
                {
                    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_XA_Week", parameters);
                    totalCount = (int)(parameters[4].Value);
                }
                else if (DateType == 3)
                {
                    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_XA_Month", parameters);
                    totalCount = (int)(parameters[4].Value);
                }
            }
            //北京
            else
            {
                if (DateType == 1)
                {
                    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_BJ_Day", parameters);
                    totalCount = (int)(parameters[4].Value);
                }
                else if (DateType == 2)
                {
                    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_BJ_Week", parameters);
                    totalCount = (int)(parameters[4].Value);
                }
                else if (DateType == 3)
                {
                    ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_BJ_Month", parameters);
                    totalCount = (int)(parameters[4].Value);
                }
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }
        /// 取满意度汇总列表（总汇总）
        /// <summary>
        /// 取满意度汇总列表（总汇总）
        /// </summary>
        /// <param name="query"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="PlaceID">1是西安，2是北京</param>
        /// <returns></returns>
        public DataTable GetSatisfactionList(QueryCallRecord_ORIG query, string order, int currentPage, int pageSize, out int totalCount, int PlaceID, DateTime b, DateTime e, string TableEndName)
        {
            string where = string.Empty;
            totalCount = 0;
            #region 数据权限判断
            if (query.LoginID != Constant.INT_INVALID_VALUE)
            {
                string whereDataRight = "";
                whereDataRight = Dal.UserGroupDataRigth.Instance.GetSqlRightstr("s", "y", "BGID", "CreateUserID", query.LoginID);

                where += whereDataRight;
            }
            #endregion

            if (!string.IsNullOrEmpty(query.AgentNum))
            {
                where += " and e.agentnum='" + StringHelper.SqlFilter(query.AgentNum) + "'";
            }

            if (query.CreateUserID != Constant.INT_INVALID_VALUE)
            {
                where += " and y.CreateUserID=" + query.CreateUserID;
            }

            if (query.SwitchINNum != Constant.STRING_INVALID_VALUE)
            {
                where += " and y.SwitchINNum='" + StringHelper.SqlFilter(query.SwitchINNum) + "'";
            }
            if (query.SkillGroup != Constant.STRING_INVALID_VALUE)
            {
                where += " and y.SkillGroup='" + StringHelper.SqlFilter(query.SkillGroup) + "'";
            }
            if (query.BeginTime != Constant.STRING_INVALID_VALUE && query.EndTime != Constant.STRING_INVALID_VALUE)
            {
                where += " and y.createTime between '" + StringHelper.SqlFilter(query.BeginTime) + "' and '" + StringHelper.SqlFilter(query.EndTime) + " 23:59:59.997'";
            }


            DataSet ds = null; ;
            DataTable dt = null;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4),
                    new SqlParameter("@b", SqlDbType.DateTime),
                    new SqlParameter("@e", SqlDbType.DateTime),
                    new SqlParameter("@tableend", SqlDbType.NVarChar, 20)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            parameters[5].Value = b;
            parameters[6].Value = e;
            parameters[7].Value = TableEndName;
            //西安
            if (PlaceID == 1)
            {
                ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_XA_HZ", parameters);
                totalCount = (int)(parameters[4].Value);
            }
            //北京
            else
            {
                ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_SatisfactionReport_BJ_HZ", parameters);
                totalCount = (int)(parameters[4].Value);
            }
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }
            return dt;
        }

        /// 得到一个对象实体
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGByCallID(long CallID, string tableEndName)
        {
            string sql = "SELECT  *  FROM  CallRecord_ORIG" + tableEndName + " WHERE CallID='" + CallID + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return LoadSingleCallRecord_ORIG(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 接口调用，只查询现表
        /// 惠买车根据业务分组，分类获取一段时间范围内的话务总表数据
        /// <summary>
        /// 惠买车根据业务分组，分类获取一段时间范围内的话务总表数据
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallDataHuiMC(string starttime, string endtime, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 组成条件

            if (!string.IsNullOrEmpty(starttime))
            {
                where += " AND c.CreateTime >='" + DateTime.Parse(starttime).ToString("yyyy-MM-dd") + "'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                where += " AND c.CreateTime <'" + DateTime.Parse(endtime).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            string bgidscids = System.Configuration.ConfigurationManager.AppSettings["EPEmbedCC_HMCBGIDSCID"].ToString();
            string[] array = bgidscids.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            string whbgscid = "";
            foreach (string item in array)
            {
                string[] bsarray = item.Split(',');
                string bgid = bsarray[0];
                string scid = bsarray[1];

                whbgscid += "(cob.BGID=" + bgid + " AND cob.SCID=" + scid + ") OR";
            }

            if (!string.IsNullOrEmpty(whbgscid))
            {
                whbgscid = whbgscid.Substring(0, whbgscid.Length - 2);
            }

            //where += " AND cob.BGID=" + EPHBuyCarBGID + " AND cob.SCID=" + EPHBuyCarSCID;
            where += " AND(" + whbgscid + ")";
            #endregion

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallRecord_ORIG_SelectByList_HMC", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 惠买车获取Inbound话务总表数据
        /// <summary>
        /// 惠买车获取Inbound话务总表数据
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetInboundDataHuiMC(string starttime, string endtime, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 组成条件

            if (!string.IsNullOrEmpty(starttime))
            {
                where += " AND c.CreateTime >='" + DateTime.Parse(starttime).ToString("yyyy-MM-dd") + "'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                where += " AND c.CreateTime <'" + DateTime.Parse(endtime).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }
            // H4_惠买车 = 4,
            var telModel = Manage.TelNumList.FirstOrDefault(x => x.HotlineID == 4);
            string tel = telModel.AreaCode + telModel.Tel;
            where += " AND c.SwitchINNum IN('" + tel + "','2454')";
            #endregion

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallRecord_ORIG_SelectByList_HMC", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 根据业务分组，分类获取一段时间范围内的话务总表数据
        /// <summary>
        /// 根据业务分组，分类获取一段时间范围内的话务总表数据
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="endtime"></param>
        /// <param name="bgid"></param>
        /// <param name="scid"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallData(string starttime, string endtime, int bgid, int scid, string order, int currentPage, int pageSize, out int totalCount)
        {
            string where = string.Empty;

            #region 组成条件

            if (!string.IsNullOrEmpty(starttime))
            {
                where += " AND c.CreateTime >='" + DateTime.Parse(starttime).ToString("yyyy-MM-dd") + "'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                where += " AND c.CreateTime <'" + DateTime.Parse(endtime).AddDays(1).ToString("yyyy-MM-dd") + "'";
            }

            if (bgid > 0)
            {
                where += " AND cob.BGID=" + bgid;
            }

            if (scid > 0)
            {
                where += " AND cob.SCID=" + scid;
            }
            //where += " AND cob.BGID=" + EPHBuyCarBGID + " AND cob.SCID=" + EPHBuyCarSCID;
            #endregion

            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 40000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallRecord_ORIG_SelectByList_HMC", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 获取话务数据接口（易集客调用）
        /// <summary>
        /// 获取话务数据接口（易集客调用）
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCallRecord_ORIGByYiJiKe(string where, string order, int currentPage, int pageSize, out int totalCount)
        {
            totalCount = 0;
            DataSet ds;

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 4000),
					new SqlParameter("@order", SqlDbType.NVarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 4)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "p_CallRecord_ORIG_SelectByYiJiKe", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 得到一个对象实体
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Entities.CallRecord_ORIG GetCallRecord_ORIGBySessionID(string SessionID)
        {
            string sql = "SELECT * FROM CallRecord_ORIG WHERE SessionID='" + SqlFilter(SessionID) + "'";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return LoadSingleCallRecord_ORIG(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// 根据主键ID，获取从主键ID之后的最新数据，最大获取十万条
        /// <summary>
        /// 根据主键ID，获取从主键ID之后的最新数据，最大获取十万条
        /// </summary>
        /// <param name="maxID">最大主键ID</param>
        /// <returns>返回DataTable</returns>
        public DataTable GetCallRecord_ORIGByMaxID(int maxID)
        {
            string sql = string.Format("SELECT TOP 10000 * FROM dbo.CallRecord_ORIG WHERE RecID > {0}", maxID);
            DataSet ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }

        ///根据手机号码，查询在CC系统中，最近一次外呼的（InitiatedTime）初始化时间
        /// <summary>
        ///根据手机号码，查询在CC系统中，最近一次外呼的（InitiatedTime）初始化时间
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public string GetPhoneLastestInitiatedTime(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return string.Empty;
            }
            var strSql =
                " SELECT CASE when MAX(InitiatedTime) IS NULL THEN '' ELSE CONVERT(varchar(24),MAX(InitiatedTime),121) END latestTime  FROM CallRecord_ORIG WHERE ANI='" + StringHelper.SqlFilter(phoneNumber) + "' AND CallStatus=2 ";

            var ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, strSql);

            return ds.Tables[0].Rows[0][0].ToString();
        }

        /// 业务记录
        /// <summary>
        /// 业务记录
        /// </summary>
        /// <param name="CustID"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBaseInfo_ServiceRecord(string phonenums, string order, int currentPage, int pageSize, out int totalCount)
        {
            phonenums = "'" + string.Join("','", phonenums.Split(',')) + "'";
            string where = " and PhoneNum IN (" + phonenums + ")";
            DataSet ds;
            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.NVarChar, 2000),
                    new SqlParameter("@order", SqlDbType.VarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 10)
					};
            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;

            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_CustBaseInfo_ServiceRecord", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        /// 话务记录
        /// <summary>
        /// 话务记录
        /// </summary>
        /// <param name="where"></param>
        /// <param name="order"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="tableEndName"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public DataTable GetCustBaseInfo_TrafficRecord(string phonenums, string order, int currentPage, int pageSize, out int totalCount)
        {
            phonenums = "'" + string.Join("','", phonenums.Split(',')) + "'";
            string where = @" AND ((cri.PhoneNum IN (" + phonenums + @") and CallStatus<>2 ) 
                                        OR (cri.ANI IN (" + phonenums + ") and CallStatus=2 ))";

            SqlParameter[] parameters = {
					new SqlParameter("@where", SqlDbType.VarChar, 4000),
					new SqlParameter("@order", SqlDbType.VarChar, 200),
					new SqlParameter("@pagesize", SqlDbType.Int, 4),
					new SqlParameter("@indexpage", SqlDbType.Int, 4),
					new SqlParameter("@totalRecorder", SqlDbType.Int, 10)
					};

            parameters[0].Value = where;
            parameters[1].Value = order;
            parameters[2].Value = pageSize;
            parameters[3].Value = currentPage;
            parameters[4].Direction = ParameterDirection.Output;
            DataSet ds = null;
            ds = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.StoredProcedure, "P_CustBaseInfo_TrafficRecord", parameters);
            totalCount = (int)(parameters[4].Value);
            return ds.Tables[0];
        }
        #endregion

        #region 只查询现在表
        /// 通过合力数据更新CC数据
        /// <summary>
        /// 通过合力数据更新CC数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public string UpdateCallRecordORIGByHolly(DataTable dt)
        {
            int a = 0;
            using (SqlConnection con = new SqlConnection(CONNECTIONSTRINGS))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = "P_CallRecord_ORIG_Update_From_HollyDataTemp";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 2 * 60;
                con.Open();
                a = cmd.ExecuteNonQuery();
                con.Close();
            }
            return a.ToString();
        }
        #region 合力辅助
        /// 清空HollyDataTemp
        /// <summary>
        /// 清空HollyDataTemp
        /// </summary>
        public void ClearHollyDataTemp()
        {
            string sql = "DELETE FROM HollyDataTemp;DBCC CHECKIDENT(HollyDataTemp,RESEED,0);";
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        /// 清除重复数据
        /// <summary>
        /// 清除重复数据
        /// </summary>
        /// <returns></returns>
        public int DeleteSameData()
        {
            string sql = @"DELETE FROM dbo.HollyDataTemp WHERE RecID IN (
                                SELECT RecID FROM (
                                SELECT RecID ,ROW_NUMBER() OVER(PARTITION BY SessionID ORDER BY RecID DESC) AS id  FROM dbo.HollyDataTemp
                                ) tmp WHERE id>1
                                )";
            return SqlHelper.ExecuteNonQuery(CONNECTIONSTRINGS, CommandType.Text, sql);
        }
        #endregion
        #endregion

        #region 同步服务
        /// 拆分话务数据，14年数据放到old表中
        /// <summary>
        /// 拆分话务数据，14年数据放到old表中
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> SplitCallDataForOld(Action<string> logFunc)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            DateTime st = new DateTime(1900, 1, 1);
            DateTime et = new DateTime(2014, 12, 31);
            string endname = "_old";

            int a = SplitCallDataForService(st, et, endname, "CallRecord_ORIG", "CreateTime", null, logFunc);
            result["CallRecord_ORIG"] = a;

            int b = SplitCallDataForService(endname, "CallRecordInfo", "CallID", "CallRecord_ORIG", "CallID", null, "RecID", logFunc);
            result["CallRecordInfo"] = b;

            int c = SplitCallDataForService(endname, "CallRecord_ORIG_Business", "CallID", "CallRecord_ORIG", "CallID", null, "RecID", logFunc);
            result["CallRecord_ORIG_Business"] = c;

            int d = SplitCallDataForService(endname, "IVRSatisfaction", "CallRecordID", "CallRecord_ORIG", "CallID", null, "Oid", logFunc);
            result["IVRSatisfaction"] = d;

            return result;
        }
        /// 拆分话务相关的表
        /// <summary>
        /// 拆分话务相关的表
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Dictionary<string, int> SplitCallDataForService(DateTime date, Action<string> logFunc)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            string endname = date.ToString("_yyyyMM");
            DateTime st = new DateTime(date.Year, date.Month, 1);
            DateTime et = st.AddMonths(1).AddDays(-1);

            logFunc("--CallRecord_ORIG");
            string index1 = @"CREATE INDEX idx_CallRecord_ORIG" + endname + @"_CallStatusCreateTime ON CallRecord_ORIG" + endname + @"(CallStatus,CreateTime);
                                         CREATE INDEX idx_CallRecord_ORIG" + endname + @"_CreateUserID ON CallRecord_ORIG" + endname + @"(CreateUserID,CreateTime);
                                         CREATE INDEX idx_CallRecord_ORIG" + endname + @"_CreateTime ON CallRecord_ORIG" + endname + @"(CreateTime);
                                         CREATE INDEX idx_CallRecord_ORIG" + endname + @"_CallID ON CallRecord_ORIG" + endname + @"(CallID);
                                         CREATE INDEX idx_CallRecord_ORIG" + endname + @"_CallStatus ON CallRecord_ORIG" + endname + @"(CallStatus);";
            int a = SplitCallDataForService(st, et, endname, "CallRecord_ORIG", "CreateTime", index1, logFunc);
            result["CallRecord_ORIG"] = a;

            logFunc("--CallRecordInfo");
            string index2 = @"CREATE INDEX idx_CallRecordInfo" + endname + @"_CallStatusCreateTime ON CallRecordInfo" + endname + @"(CallStatus,CreateTime);
                                         CREATE INDEX idx_CallRecordInfo" + endname + @"_CreateUserID ON CallRecordInfo" + endname + @"(CreateUserID,CreateTime);
                                         CREATE INDEX idx_CallRecordInfo" + endname + @"_CreateTime ON CallRecordInfo" + endname + @"(CreateTime);
                                         CREATE INDEX idx_CallRecordInfo" + endname + @"_CallID ON CallRecordInfo" + endname + @"(CallID);
                                         CREATE INDEX idx_CallRecordInfo" + endname + @"_CallStatus ON CallRecordInfo" + endname + @"(CallStatus);";
            int b = SplitCallDataForService(endname, "CallRecordInfo", "CallID", "CallRecord_ORIG", "CallID", index2, "RecID", logFunc);
            result["CallRecordInfo"] = b;

            logFunc("--CallRecord_ORIG_Business");
            string index3 = @"CREATE INDEX idx_CallRecord_ORIG_Business" + endname + @"_CallID ON CallRecord_ORIG_Business" + endname + @"(CallID) INCLUDE(BGID,SCID,BusinessID);
                                         CREATE INDEX idx_CallRecord_ORIG_Business" + endname + @"_BGID ON CallRecord_ORIG_Business" + endname + @"(BGID,CallID);";
            int c = SplitCallDataForService(endname, "CallRecord_ORIG_Business", "CallID", "CallRecord_ORIG", "CallID", index3, "RecID", logFunc);
            result["CallRecord_ORIG_Business"] = c;

            logFunc("--IVRSatisfaction");
            string index4 = "CREATE INDEX idx_IVRSatisfaction" + endname + @"_CallRecordID ON IVRSatisfaction" + endname + @"(CallRecordID) INCLUDE(Score);";
            int d = SplitCallDataForService(endname, "IVRSatisfaction", "CallRecordID", "CallRecord_ORIG", "CallID", index4, "Oid", logFunc);
            result["IVRSatisfaction"] = d;

            return result;
        }

        public int SplitCallDataForService(DateTime st, DateTime et, string endname, string tablename, string colname, string indexsql, Action<string> logFunc)
        {
            int count = 0;
            if (!CommonDal.Instance.CheckTableExists(tablename + endname))
            {
                string sql = @"SELECT * INTO " + tablename + endname + " FROM " + tablename +
                    " WHERE " + colname + ">='" + st.ToString("yyyy-MM-dd 00:00:00") +
                    "' AND " + colname + "<='" + et.ToString("yyyy-MM-dd 23:59:59") + "'";
                logFunc("==创建表");
                count = ExecSql(sql);
                if (indexsql != null)
                {
                    logFunc("==创建索引");
                    ExecSql(indexsql);
                }
            }
            else
            {
                count = -1;
            }

            //删除导出的数据            
            for (DateTime date = st.Date; date <= et.Date; date = date.AddDays(1))
            {
                string del = @"DELETE FROM " + tablename +
                    " WHERE " + colname + ">='" + date.ToString("yyyy-MM-dd 00:00:00") +
                    "' AND " + colname + "<='" + date.ToString("yyyy-MM-dd 23:59:59") + "'";
                logFunc("==删除数据 " + date);
                int c = ExecSql(del);
            }
            return count;
        }
        public int SplitCallDataForService(string endname, string tablename, string colname, string destable, string descol, string indexsql, string keycol, Action<string> logFunc)
        {
            int count = 0;
            if (!CommonDal.Instance.CheckTableExists(tablename + endname))
            {
                string sql = @"SELECT * INTO " + tablename + endname + " FROM " + tablename +
                    " WHERE " + colname + " IN (SELECT " + descol + " FROM " + destable + endname + ");";
                logFunc("==创建表");
                count = ExecSql(sql);
                if (indexsql != null)
                {
                    logFunc("==创建索引");
                    ExecSql(indexsql);
                }
            }
            else
            {
                count = -1;
            }

            //删除导出的数据
            int c = 1;
            while (c != 0)
            {
                string del = @"DELETE FROM " + tablename
                    + @" WHERE " + keycol + @" IN (SELECT Top 10000 " + keycol + @" FROM  " + tablename + @" WHERE " + colname + " IN (SELECT " + descol + " FROM " + destable + endname + "))";
                logFunc("==删除数据");
                c = ExecSql(del);
            }
            return count;
        }

        /// 不超时执行sql
        /// <summary>
        /// 不超时执行sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecSql(string sql)
        {
            int a = 0;
            using (SqlConnection con = new SqlConnection(CONNECTIONSTRINGS))
            {
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 10 * 60;
                con.Open();
                a = cmd.ExecuteNonQuery();
                con.Close();
            }
            return a;
        }

        /// 获取话务总表存在问题的数据
        /// <summary>
        /// 获取话务总表存在问题的数据
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        public List<Entities.CallRecord_ORIG> GetCallRecord_ORIGForError(DateTime st)
        {
            //当前半小时内，电话可能未挂断，不能补录
            string sql = @"SELECT a.*,b.BusinessID,b.BGID,b.SCID
                        FROM dbo.CallRecord_ORIG(NOLOCK) a
                        LEFT JOIN dbo.CallRecord_ORIG_Business(NOLOCK) b ON a.CallID=b.CallID
                        WHERE 1=1
                        AND a.EstablishedTime>'1970-1-1' --接通
                        AND ISNULL(a.AgentReleaseTime,a.CustomerReleaseTime)>'1970-1-1' --挂断
                        AND a.CreateTime>='" + CommonFunction.GetDateTimeStr(st) + @"'
                        AND a.CreateTime<='" + CommonFunction.GetDateTimeStr(DateTime.Now.AddMinutes(-30)) + @"'
                        AND a.OutBoundType>0 AND a.OutBoundType<>2 --非客户端外呼
                        AND a.CallID NOT IN (SELECT CallID FROM dbo.CallRecordInfo) --来电去电表不存
                        ORDER BY a.CreateTime";
            DataTable dt = SqlHelper.ExecuteDataset(CONNECTIONSTRINGS, CommandType.Text, sql).Tables[0];
            List<Entities.CallRecord_ORIG> list = new List<Entities.CallRecord_ORIG>();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var item = LoadSingleCallRecord_ORIG(dr);
                    item.BusinessID = CommonFunction.ObjectToString(dr["BusinessID"]);
                    item.BGID = CommonFunction.ObjectToInteger(dr["BGID"], -1);
                    item.SCID = CommonFunction.ObjectToInteger(dr["SCID"], -1);
                    list.Add(item);
                }
            }
            return list;
        }
        #endregion

        #region 大数据
        /// 按月查询话务数据
        /// <summary>
        /// 按月查询话务数据
        /// </summary>
        /// <param name="endtablename"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public void GetAllCallRecordORIGForHB(string endtablename, DateTime month, out int count, Action<string> callback, Func<string, string> haomaProcess)
        {
            //查询历史表，取一个月的数据
            if (endtablename == "")
            {
                //查询现在表，取一个月的数据
                DateTime st = new DateTime(month.Year, month.Month, 1);
                DateTime et = st.AddMonths(1).AddSeconds(-1);
                GetAllCallRecordORIGForHB(st, et, out count, callback, haomaProcess);
            }
            else
            {
                //查询过去表，取整表数据
                GetAllCallRecordORIGForHB(endtablename, out count, callback, haomaProcess);
            }
        }
        /// 按时间段查询现在话务数据
        /// <summary>
        /// 按时间段查询现在话务数据
        /// </summary>
        /// <param name="st"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public void GetAllCallRecordORIGForHB(DateTime st, DateTime et, out int count, Action<string> callback, Func<string, string> haomaProcess)
        {
            //查询现在表某个时间段内的数据
            string where = " AND a.CreateTime>='" + st.ToString("yyyy-MM-dd HH:mm:ss") + "' AND a.CreateTime<='" + et.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            ExecuteReader("", where, out count, callback, haomaProcess);
        }
        /// 查询过去表，取整表数据
        /// <summary>
        /// 查询过去表，取整表数据
        /// </summary>
        /// <param name="endtablename"></param>
        /// <param name="count"></param>
        /// <param name="callback"></param>
        public void GetAllCallRecordORIGForHB(string endtablename, out int count, Action<string> callback, Func<string, string> haomaProcess)
        {
            ExecuteReader(endtablename, "", out count, callback, haomaProcess);
        }
        /// 获取查询列
        /// <summary>
        /// 获取查询列
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllCallRecordORIGForHB_SelectCol()
        {
            List<string> list = new List<string>();
            list.Add("SessionID"); //1
            list.Add("CallID"); //2
            list.Add("ExtensionNum"); //3
            list.Add("Oriani"); //4
            list.Add("OriDnis"); //5
            list.Add("CallStatus"); //6
            list.Add("SwitchINNum"); //7
            list.Add("OutBoundType"); //8
            list.Add("SkillGroup"); //9
            list.Add("InitiatedTime"); //10

            list.Add("RingingTime"); //11
            list.Add("EstablishedTime"); //12
            list.Add("AgentReleaseTime"); //13
            list.Add("CustomerReleaseTime"); //14
            list.Add("AfterWorkBeginTime"); //15
            list.Add("AfterWorkTime"); //16
            list.Add("ConsultTime"); //17
            list.Add("ReconnectCall"); //18
            list.Add("TallTime"); //19
            list.Add("AudioURL"); //20

            list.Add("CreateTime"); //21
            list.Add("CreateUserID"); //22
            list.Add("BGID"); //23
            list.Add("SCID"); //24
            list.Add("BusinessID"); //25
            list.Add("BusinessSource"); //26
            list.Add("IVRScore"); //27
            list.Add("AgentNum"); //28
            list.Add("CustName"); //29
            list.Add("TotalTelTime"); //30

            list.Add("HotlineName"); //31
            list.Add("BusinessDetailURL"); //32
            list.Add("CustID"); //33

            return list;
        }

        #region 大数据查询
        //执行sql
        private void ExecuteReader(string endtablename, string where, out int count, Action<string> callback, Func<string, string> haomaProcess)
        {
            List<string> cols = GetAllCallRecordORIGForHB_SelectCol();
            callback(GetTableTitle(cols));

            count = 0;
            string sql = "SELECT " + GetAllCallRecordORIGForHB_SelectSql()
                + " FROM " + GetAllCallRecordORIGForHB_FromSql(endtablename) + " "
                + GetAllCallRecordORIGForHB_WhereSql(where);

            using (SqlConnection conn = new SqlConnection(CONNECTIONSTRINGS))
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 5 * 60;
                conn.Open();
                using (SqlDataReader sr = cmd.ExecuteReader())
                {
                    while (sr.Read())
                    {
                        callback(GetSqlDataReaderData(sr, cols, haomaProcess));
                        count++;
                    }
                }
                conn.Close();
            }
        }
        private string GetAllCallRecordORIGForHB_SelectSql()
        {
            string sql = @" RTRIM(ISNULL(a.SessionID,'')) AS SessionID,    --1 录音流水号或厂商联络ID
	                                RTRIM(ISNULL(a.CallID,'')) AS CallID,    --2 话务ID
	                                RTRIM(ISNULL(a.ExtensionNum,'')) AS ExtensionNum,    --3 分机号码
	                                RTRIM(ISNULL(a.PhoneNum,'')) AS Oriani,    --4 主叫号码
	                                RTRIM(ISNULL(a.ANI,'')) AS OriDnis,    --5 被叫号码

	                                RTRIM(ISNULL(a.CallStatus,'')) AS CallStatus,    --6 呼叫方向（1-呼入，2-呼出,3-转接 4-拦截）
	                                RTRIM(ISNULL(a.SwitchINNum,'')) AS SwitchINNum,    --7 接入号码（落地号）
	                                RTRIM(ISNULL(a.OutBoundType,'')) AS OutBoundType,    --8 呼出类型（1、页面呼出；2、客户端呼出；3，转接（废弃）4、自动外呼）
	                                RTRIM(ISNULL(a.SkillGroup,'')) AS SkillGroup,    --9 技能组ID

	                                ISNULL(convert(VARCHAR(19),a.InitiatedTime,120),'') AS InitiatedTime,    --10 外呼时-初始化时间；呼入时-进入热线时间
	                                ISNULL(convert(VARCHAR(19),a.RingingTime,120),'') AS RingingTime	,    --11 振铃开始时间
	                                ISNULL(convert(VARCHAR(19),a.EstablishedTime,120),'') AS EstablishedTime	,    --12 	接通时间
	                                ISNULL(convert(VARCHAR(19),a.AgentReleaseTime,120),'') AS AgentReleaseTime,    --13	坐席挂断时间
	                                ISNULL(convert(VARCHAR(19),a.CustomerReleaseTime,120),'') AS CustomerReleaseTime,    --14	客户挂断时间
	                                ISNULL(convert(VARCHAR(19),a.AfterWorkBeginTime,120),'') AS AfterWorkBeginTime	,    --15	事后处理开始时间
	                                ISNULL(convert(VARCHAR(19),a.AfterWorkTime,120),'') AS AfterWorkTime	,    --16	事后处理时长（单位：秒）
	                                ISNULL(convert(VARCHAR(19),a.ConsultTime,120),'') AS ConsultTime	,    --17	转接开始时间
	                                ISNULL(convert(VARCHAR(19),a.ReconnectCall,120),'') AS ReconnectCall,    --18	转接恢复时间
	                                ISNULL(convert(VARCHAR(19),a.TallTime,120),'') AS TallTime,    --19	录音总时长（单位：秒）
	                                ISNULL(a.AudioURL,'') AS AudioURL,    --20	录音URL地址

	                                ISNULL(convert(VARCHAR(19),a.CreateTime,120),'') AS CreateTime	,    --21	创建记录时间
	                                RTRIM(ISNULL(a.CreateUserID,'')) AS CreateUserID	,    --22	创建者ID
	                                RTRIM(ISNULL(b.BGID,'')) AS BGID,    --23 业务分组ID
	                                RTRIM(ISNULL(b.SCID,'')) AS SCID,    --24 业务分类ID
	                                RTRIM(ISNULL(c.BusinessID,'')) AS BusinessID,     --25 业务ID
	                                CASE WHEN SUBSTRING(RTRIM(ISNULL(c.BusinessID,b.CustID)),1,2) ='WO' THEN 0       --工单
			                                 WHEN SUBSTRING(RTRIM(ISNULL(c.BusinessID,b.CustID)),1,2) ='CB' THEN 3         --客户回访
			                                 WHEN SUBSTRING(RTRIM(ISNULL(c.BusinessID,b.CustID)),1,3) ='OTH' THEN 4      --其他任务
			                                 WHEN SUBSTRING(RTRIM(ISNULL(c.BusinessID,b.CustID)),1,3) ='YJK' THEN 5        --易集客
			                                 WHEN SUBSTRING(RTRIM(ISNULL(c.BusinessID,b.CustID)),1,3) ='CJK' THEN 6        --厂家集客
			                                 WHEN SUBSTRING(RTRIM(ISNULL(c.BusinessID,b.CustID)),1,3) ='YTG' THEN 7       --易团购
			                                 ELSE -1 END AS BusinessSource,    --26 业务来源ID
	                                RTRIM(ISNULL(e.Score,'')) AS IVRScore,    --27 IVR满意度结果
	                                RTRIM(ISNULL(f.AgentNum,'')) AS AgentNum ,    --28 坐席当前工号
	                                RTRIM(REPLACE(ISNULL(b.CustName,''),',',' ')) AS CustName,    --29 客户姓名
	                                RTRIM(ISNULL(a.TallTime+a.AfterWorkTime,'')) AS TotalTelTime,    --30 话务总时长=TallTime+ AfterWorkTime
	
	                                RTRIM(ISNULL(g.Remark,'')) AS HotlineName,     --31 当前话务热线名称
	                                RTRIM(ISNULL(d.BusinessDetailURL,'')) AS BusinessDetailURL,    --32 当前话务对应业务URL模板路径
	                                RTRIM(ISNULL(b.CustID,'')) AS CustID    --33 CRM系统客户ID，或CC客户池下客户ID"
                + "\r\n";//因为有注释，必须换行
            return sql;
        }
        private string GetAllCallRecordORIGForHB_FromSql(string endtablename)
        {
            string sql = @"dbo.CallRecord_ORIG" + endtablename + @" a
                                    LEFT JOIN dbo.CallRecordInfo" + endtablename + @" b ON b.CallID = a.CallID
                                    LEFT JOIN dbo.CallRecord_ORIG_Business" + endtablename + @" c ON c.CallID = a.CallID
                                    LEFT JOIN dbo.CallRecord_ORIG_BusinessURL d ON c.BGID=d.BGID AND c.SCID=d.SCID
                                    LEFT JOIN dbo.IVRSatisfaction" + endtablename + @" e ON e.CallRecordID=a.CallID
                                    LEFT JOIN dbo.EmployeeAgent f ON a.CreateUserID=f.UserID
                                    LEFT JOIN dbo.CallDisplay g ON a.SwitchINNum=g.AreaCode+g.TelMainNum"
                        + "\r\n";
            return sql;
        }
        private string GetAllCallRecordORIGForHB_WhereSql(string where)
        {
            string sql = @"WHERE ISNULL(a.CallID,'')<>'' 
                                    AND ISNULL(a.ExtensionNum,'')<>'' 
                                    AND ISNULL(a.PhoneNum,'')<>'' 
                                    AND ISNULL(a.ANI,'')<>''
                                    AND ISNULL(a.CallStatus,0)>0
                                    AND ISNUMERIC (a.PhoneNum)=1
                                    AND ISNUMERIC (a.ANI)=1"
                + where;
            return sql;
        }

        private string GetSqlDataReaderData(SqlDataReader sr, List<string> cols, Func<string, string> haomaProcess)
        {
            string info = "";
            foreach (string col in cols)
            {
                string value = CommonFunction.ObjectToString(sr[col]);
                //list.Add("Oriani"); //4
                //list.Add("OriDnis"); //5
                if (col == "Oriani" || col == "OriDnis")
                {
                    value = haomaProcess(value);
                }
                else
                {
                    //去掉特殊字符
                    value = value.Replace(",", "");
                    value = value.Replace("\r", "");
                    value = value.Replace("\n", "");
                }
                info += value + ",";
            }
            return info.Substring(0, info.Length - 1);
        }
        private string GetTableTitle(List<string> cols)
        {
            string info = "";
            foreach (string col in cols)
            {
                info += col + ",";
            }
            return info.Substring(0, info.Length - 1);
        }
        #endregion
        #endregion
    }
}

